using System;
using UnityEngine;

namespace Bussigo.Vehicle
{
    [RequireComponent(typeof(Rigidbody))]
    public class BusChassisController : MonoBehaviour
    {
        [Header("Chassis & Powertrain")]
        public float curbMassKg = 14500f;
        public float engineHorsepower = 360f;
        public float maxEngineTorqueNm = 1400f;
        public float maxSteeringAngleDegrees = 34f;

        [Header("Pneumatics & Retarder")]
        public float primaryAirPressureBar = 8.5f;
        public int retarderLevel = 0; // 0 = Off, 1..4 = 25%..100%

        [Header("Actuators & State")]
        public bool isDoorOpen = false;
        public bool isHeadlightsActive = false;
        public bool isHornSounding = false;
        public float currentSpeedKmh = 0f;
        public float currentEngineRpm = 650f;

        private Rigidbody rb;
        private float currentSteerInput = 0f;
        private float currentThrottleInput = 0f;
        private float currentBrakeInput = 0f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.mass = curbMassKg;
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
            retarderLevel = (retarderLevel + 1) % 5;
        }

        public void UpdatePayloadMass(int passengerCount)
        {
            rb.mass = curbMassKg + (passengerCount * 75f);
        }

        private void FixedUpdate()
        {
            currentSpeedKmh = Vector3.Dot(rb.linearVelocity, transform.forward) * 3.6f;

            // Speed-sensitive steering curve
            float speedFactor = Mathf.Clamp01(1.0f - (Mathf.Abs(currentSpeedKmh) / 110.0f));
            float targetSteerAngle = currentSteerInput * maxSteeringAngleDegrees * (0.35f + 0.65f * speedFactor);

            if (Mathf.Abs(currentSteerInput) > 0.01f && Mathf.Abs(currentSpeedKmh) > 0.5f)
            {
                float turnDir = Mathf.Sign(currentSpeedKmh);
                rb.AddTorque(transform.up * (targetSteerAngle * 900f * turnDir * Time.fixedDeltaTime), ForceMode.Acceleration);
            }

            // Propulsion & Braking
            if (currentThrottleInput > 0.01f && !isDoorOpen)
            {
                float driveForce = maxEngineTorqueNm * currentThrottleInput * 3.5f;
                rb.AddForce(transform.forward * driveForce, ForceMode.Force);
            }

            if (currentBrakeInput > 0.01f)
            {
                float brakeForce = 52000f * currentBrakeInput * (primaryAirPressureBar / 8.5f);
                rb.AddForce(-transform.forward * (brakeForce * Mathf.Sign(currentSpeedKmh)), ForceMode.Force);
            }

            // Retarder Drag
            if (retarderLevel > 0 && Mathf.Abs(currentSpeedKmh) > 5.0f)
            {
                float retarderForce = retarderLevel * 4500f;
                rb.AddForce(-transform.forward * (retarderForce * Mathf.Sign(currentSpeedKmh)), ForceMode.Force);
            }
        }
    }
}
