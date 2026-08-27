using System;
using UnityEngine;
using Bussigo.Physics;

namespace Bussigo.Vehicle
{
    [RequireComponent(typeof(Rigidbody))]
    public class BusChassisController : MonoBehaviour
    {
        [Header("Physics Core Model")]
        public HeavyVehiclePhysicsModel physicsModel = new HeavyVehiclePhysicsModel();

        [Header("Runtime State")]
        public bool isDoorOpen = false;
        public bool isHeadlightsActive = false;
        public bool isHornSounding = false;
        public float currentSpeedKmh = 0f;

        public float currentEngineRpm => physicsModel.Powertrain.currentRpm;
        public int currentGear => physicsModel.Powertrain.currentGear;
        public float primaryAirPressureBar => physicsModel.AirBrakes.currentReservoirPressureBar;
        public int retarderLevel => physicsModel.Retarder.currentStage;

        public float SteerInput => currentSteerInput;
        public float ThrottleInput => currentThrottleInput;
        public float BrakeInput => currentBrakeInput;

        private Rigidbody rb;
        private float currentSteerInput = 0f;
        private float currentThrottleInput = 0f;
        private float currentBrakeInput = 0f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            UpdateRigidbodyMass();
            rb.centerOfMass = new Vector3(0f, -0.65f, 0.2f);
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }

        public void SetDriverInputs(float steer, float throttle, float brake)
        {
            currentSteerInput = Mathf.Clamp(steer, -1f, 1f);
            currentThrottleInput = Mathf.Clamp01(throttle);
            currentBrakeInput = Mathf.Clamp01(brake);
        }

        public void ToggleGliderDoors()
        {
            if (Mathf.Abs(currentSpeedKmh) < 2.0f)
            {
                isDoorOpen = !isDoorOpen;
            }
        }

        public void CycleRetarder()
        {
            physicsModel.Retarder.CycleStage();
        }

        public void ShiftUp() => physicsModel.Powertrain.ShiftUp();
        public void ShiftDown() => physicsModel.Powertrain.ShiftDown();

        public void UpdatePayloadMass(int passengerCount)
        {
            physicsModel.UpdatePayload(passengerCount);
            UpdateRigidbodyMass();
        }

        private void UpdateRigidbodyMass()
        {
            if (rb != null)
            {
                rb.mass = physicsModel.TotalMassKg;
            }
        }

        private void FixedUpdate()
        {
            currentSpeedKmh = Vector3.Dot(rb.linearVelocity, transform.forward) * 3.6f;

            // Step core physics subsystem
            physicsModel.StepPhysics(Time.fixedDeltaTime, currentThrottleInput, currentBrakeInput, currentSteerInput, currentSpeedKmh);

            // Steering Yaw Application
            float targetSteerAngle = physicsModel.CalculateSteerAngle(currentSteerInput, currentSpeedKmh);
            if (Mathf.Abs(currentSteerInput) > 0.01f && Mathf.Abs(currentSpeedKmh) > 0.5f)
            {
                float turnDir = Mathf.Sign(currentSpeedKmh);
                rb.AddTorque(transform.up * (targetSteerAngle * 950f * turnDir * Time.fixedDeltaTime), ForceMode.Acceleration);
            }

            // Driveline Propulsion
            if (currentThrottleInput > 0.01f && !isDoorOpen)
            {
                float wheelTorque = physicsModel.Powertrain.CalculateWheelTorque(currentThrottleInput);
                float driveForce = wheelTorque / physicsModel.wheelRadiusMeters;
                rb.AddForce(transform.forward * driveForce, ForceMode.Force);
            }

            // Total Braking (Air Brakes + Retarder)
            if (currentBrakeInput > 0.01f || physicsModel.Retarder.currentStage > 0)
            {
                float totalBrakeForce = physicsModel.CalculateTotalBrakingForce(currentBrakeInput, currentSpeedKmh);
                if (Mathf.Abs(currentSpeedKmh) > 0.1f)
                {
                    rb.AddForce(-transform.forward * (totalBrakeForce * Mathf.Sign(currentSpeedKmh)), ForceMode.Force);
                }
            }
        }
    }
}
