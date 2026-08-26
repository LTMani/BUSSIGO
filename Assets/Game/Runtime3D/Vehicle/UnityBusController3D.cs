using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bussigo.Game.Core;
using Bussigo.Game.VehiclePhysics;

namespace Bussigo.Game.Runtime3D.Vehicle
{
    [RequireComponent(typeof(Rigidbody))]
    public class UnityBusController3D : MonoBehaviour
    {
        [Header("Chassis & Physics Parameters")]
        public float curbMassKg = 12500f;
        public Vector3 centerOfMassOffset = new Vector3(0f, -0.6f, 0.2f);
        public float maxSteeringAngleDegrees = 38f;
        public float engineHorsepower = 240f;
        public float maxEngineTorqueNm = 920f;
        public float idleRpm = 650f;
        public float maxRpm = 2400f;

        [Header("Pneumatic Air Brake System")]
        public float primaryAirPressureBar = 8.5f;
        public float secondaryAirPressureBar = 8.5f;
        public float compressorGovernorCutInBar = 7.0f;
        public float compressorGovernorCutOutBar = 9.2f;
        public bool isAirCompressorActive = false;
        public bool isSpringEmergencyBrakeEngaged = false;
        public float airConsumptionPerFullBrakeBar = 0.35f;

        [Header("Retarder Braking System")]
        public int currentRetarderLevel = 0; // 0 = Off, 1 = 25%, 2 = 50%, 3 = 75%, 4 = 100%
        public float maxRetarderBrakingTorqueNm = 1800f;

        [Header("Transmission & Gears")]
        public bool isAutomaticTransmission = false;
        public int currentGearIndex = 1; // -1 = Reverse, 0 = Neutral, 1..6 = Forward Gears
        public float[] forwardGearRatios = new float[] { 6.81f, 3.78f, 2.24f, 1.45f, 1.00f, 0.73f };
        public float reverseGearRatio = -6.20f;
        public float finalDriveAxleRatio = 4.10f;

        [Header("Consumables & Wear")]
        public float fuelTankCapacityLiters = 350f;
        public float currentFuelLiters = 280f;
        public float engineCoolantTempCelsius = 82f;
        public float brakeLiningHealth01 = 1.0f;
        public float tyreTreadCondition01 = 1.0f;

        [Header("Visual & Functional Elements")]
        public Transform[] frontLeftWheelTransforms;
        public Transform[] frontRightWheelTransforms;
        public Transform[] rearLeftWheelTransforms;
        public Transform[] rearRightWheelTransforms;
        public Light[] headlightLowBeams;
        public Light[] headlightHighBeams;
        public Light[] brakeTailLights;
        public Light[] reverseLights;
        public Light[] leftTurnIndicatorLights;
        public Light[] rightTurnIndicatorLights;
        public Transform passengerGliderDoorTransform;

        [Header("Live Telemetry Outputs")]
        public float currentSpeedKmh = 0f;
        public float currentEngineRpm = 650f;
        public float currentThrottleInput01 = 0f;
        public float currentBrakeInput01 = 0f;
        public float currentSteeringInput = 0f;
        public bool isDoorOpen = false;
        public bool isLeftIndicatorActive = false;
        public bool isRightIndicatorActive = false;
        public bool isHazardActive = false;
        public bool isHighBeamActive = false;
        public bool isHornSounding = false;

        private Rigidbody rb;
        private float indicatorBlinkTimer = 0f;
        private bool indicatorBlinkState = false;
        private float doorAnimationProgress01 = 0f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.mass = curbMassKg;
            rb.centerOfMass = centerOfMassOffset;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }

        private void Update()
        {
            HandlePlayerInputs();
            UpdateLightingAndBlinkers(Time.deltaTime);
            UpdateDoorAnimation(Time.deltaTime);
            UpdateEngineAndPneumatics(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            CalculateChassisDynamics();
        }

        private void HandlePlayerInputs()
        {
            // Steering & Throttle / Brake
            currentSteeringInput = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if (vertical >= 0f)
            {
                currentThrottleInput01 = vertical;
                currentBrakeInput01 = 0f;
            }
            else
            {
                currentThrottleInput01 = 0f;
                currentBrakeInput01 = -vertical;
            }

            // Handbrake / Air Parking Brake
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isSpringEmergencyBrakeEngaged = !isSpringEmergencyBrakeEngaged;
            }

            // Gear shifting
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                ShiftGearUp();
            }
            else if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                ShiftGearDown();
            }

            // Retarder Stage Cycle (Key R)
            if (Input.GetKeyDown(KeyCode.R))
            {
                currentRetarderLevel = (currentRetarderLevel + 1) % 5;
            }

            // Horn (Key H)
            isHornSounding = Input.GetKey(KeyCode.H);

            // Door Toggle (Key E)
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (currentSpeedKmh < 3f) // Safe door interlock
                {
                    isDoorOpen = !isDoorOpen;
                }
            }

            // Headlights (Key L)
            if (Input.GetKeyDown(KeyCode.L))
            {
                isHighBeamActive = !isHighBeamActive;
            }

            // Turn Indicators (Keys Q / Z / X)
            if (Input.GetKeyDown(KeyCode.Q))
            {
                isLeftIndicatorActive = !isLeftIndicatorActive;
                if (isLeftIndicatorActive) isRightIndicatorActive = false;
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                isRightIndicatorActive = !isRightIndicatorActive;
                if (isRightIndicatorActive) isLeftIndicatorActive = false;
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                isHazardActive = !isHazardActive;
            }
        }

        public void ShiftGearUp()
        {
            if (currentGearIndex < forwardGearRatios.Length)
            {
                currentGearIndex++;
            }
        }

        public void ShiftGearDown()
        {
            if (currentGearIndex > -1)
            {
                currentGearIndex--;
            }
        }

        private void CalculateChassisDynamics()
        {
            Vector3 forwardVelocity = Vector3.Project(rb.linearVelocity, transform.forward);
            currentSpeedKmh = forwardVelocity.magnitude * 3.6f * Mathf.Sign(Vector3.Dot(rb.linearVelocity, transform.forward));

            // Spring brake interlock
            if (primaryAirPressureBar < 3.8f || isSpringEmergencyBrakeEngaged)
            {
                rb.linearDamping = 12f;
                rb.angularDamping = 12f;
                return;
            }
            else
            {
                rb.linearDamping = 0.05f;
                rb.angularDamping = 1.2f;
            }

            // Speed-sensitive Ackermann steering curve
            float speedFactor = Mathf.Clamp01(1.0f - (Mathf.Abs(currentSpeedKmh) / 120.0f));
            float targetSteerAngle = currentSteeringInput * maxSteeringAngleDegrees * (0.35f + 0.65f * speedFactor);

            // Apply steering yaw torque
            if (Mathf.Abs(currentSteeringInput) > 0.01f && Mathf.Abs(currentSpeedKmh) > 1.0f)
            {
                float turnDir = Mathf.Sign(currentSpeedKmh);
                float yawTorque = targetSteerAngle * 850f * turnDir;
                rb.AddTorque(transform.up * (yawTorque * Time.fixedDeltaTime), ForceMode.Acceleration);
            }

            // Powertrain propulsion force
            float effectiveRatio = (currentGearIndex > 0) ? forwardGearRatios[currentGearIndex - 1] : (currentGearIndex == -1 ? reverseGearRatio : 0f);
            float totalDriveRatio = effectiveRatio * finalDriveAxleRatio;
            
            float wheelRadiusMeters = 0.52f;
            float availableWheelTorque = (maxEngineTorqueNm * currentThrottleInput01 * totalDriveRatio) / wheelRadiusMeters;

            if (currentGearIndex != 0 && currentThrottleInput01 > 0.01f && currentFuelLiters > 0.1f)
            {
                Vector3 driveForce = transform.forward * availableWheelTorque;
                rb.AddForce(driveForce, ForceMode.Force);

                // Diesel fuel consumption (BSFC calculation: ~210 g/kWh)
                float fuelBurnRateLps = (maxEngineTorqueNm * currentThrottleInput01 * (currentEngineRpm / 9549f) * 0.210f) / (835f * 3600f);
                currentFuelLiters = Mathf.Max(0f, currentFuelLiters - fuelBurnRateLps * Time.fixedDeltaTime);
            }

            // Service braking force (Pneumatic disk/drum brakes)
            if (currentBrakeInput01 > 0.01f && primaryAirPressureBar > 4.0f)
            {
                float maxBrakeForceNewtons = 48000f * (primaryAirPressureBar / 8.5f);
                Vector3 brakeForce = -transform.forward * (maxBrakeForceNewtons * currentBrakeInput01 * Mathf.Sign(currentSpeedKmh));
                rb.AddForce(brakeForce, ForceMode.Force);
                
                // Air consumption
                primaryAirPressureBar = Mathf.Max(0f, primaryAirPressureBar - (airConsumptionPerFullBrakeBar * currentBrakeInput01 * Time.fixedDeltaTime * 0.5f));
            }

            // Retarder hydrodynamic continuous braking
            if (currentRetarderLevel > 0 && Mathf.Abs(currentSpeedKmh) > 5.0f)
            {
                float retarderPercent = currentRetarderLevel * 0.25f;
                float retarderForceNewtons = (maxRetarderBrakingTorqueNm * retarderPercent) / wheelRadiusMeters;
                Vector3 retarderVector = -transform.forward * (retarderForceNewtons * Mathf.Sign(currentSpeedKmh));
                rb.AddForce(retarderVector, ForceMode.Force);
            }

            // Wheel visual rotations and steering angles
            ApplyVisualWheelRotations(targetSteerAngle);
        }

        private void ApplyVisualWheelRotations(float steerAngle)
        {
            float wheelSpinDeltaDegrees = (currentSpeedKmh / 3.6f) / (Mathf.PI * 1.04f) * 360f * Time.fixedDeltaTime;

            if (frontLeftWheelTransforms != null)
            {
                foreach (var t in frontLeftWheelTransforms)
                {
                    if (t != null)
                    {
                        t.localRotation = Quaternion.Euler(0f, steerAngle, 0f);
                        t.Rotate(Vector3.right, wheelSpinDeltaDegrees, Space.Self);
                    }
                }
            }
            if (frontRightWheelTransforms != null)
            {
                foreach (var t in frontRightWheelTransforms)
                {
                    if (t != null)
                    {
                        t.localRotation = Quaternion.Euler(0f, steerAngle, 0f);
                        t.Rotate(Vector3.right, wheelSpinDeltaDegrees, Space.Self);
                    }
                }
            }
            if (rearLeftWheelTransforms != null)
            {
                foreach (var t in rearLeftWheelTransforms)
                {
                    if (t != null) t.Rotate(Vector3.right, wheelSpinDeltaDegrees, Space.Self);
                }
            }
            if (rearRightWheelTransforms != null)
            {
                foreach (var t in rearRightWheelTransforms)
                {
                    if (t != null) t.Rotate(Vector3.right, wheelSpinDeltaDegrees, Space.Self);
                }
            }
        }

        private void UpdateEngineAndPneumatics(float dt)
        {
            // RPM calculation based on speed and gear ratio
            if (currentGearIndex != 0)
            {
                float effectiveRatio = (currentGearIndex > 0) ? forwardGearRatios[currentGearIndex - 1] : reverseGearRatio;
                float targetRpm = (Mathf.Abs(currentSpeedKmh) / (3.6f * Mathf.PI * 1.04f)) * effectiveRatio * finalDriveAxleRatio * 60f;
                currentEngineRpm = Mathf.Clamp(targetRpm + (currentThrottleInput01 * 450f), idleRpm, maxRpm);
            }
            else
            {
                currentEngineRpm = Mathf.MoveTowards(currentEngineRpm, idleRpm + (currentThrottleInput01 * 1600f), dt * 1800f);
            }

            // Air Compressor Governor
            if (primaryAirPressureBar <= compressorGovernorCutInBar)
            {
                isAirCompressorActive = true;
            }
            else if (primaryAirPressureBar >= compressorGovernorCutOutBar)
            {
                isAirCompressorActive = false;
            }

            if (isAirCompressorActive)
            {
                float rechargeRateBarPerSec = 0.25f * (currentEngineRpm / 1500f);
                primaryAirPressureBar = Mathf.Min(compressorGovernorCutOutBar, primaryAirPressureBar + rechargeRateBarPerSec * dt);
                secondaryAirPressureBar = primaryAirPressureBar;
            }
        }

        private void UpdateLightingAndBlinkers(float dt)
        {
            indicatorBlinkTimer += dt;
            if (indicatorBlinkTimer >= 0.45f)
            {
                indicatorBlinkTimer = 0f;
                indicatorBlinkState = !indicatorBlinkState;
            }

            // Low / High Beams
            if (headlightHighBeams != null)
            {
                foreach (var l in headlightHighBeams) if (l != null) l.enabled = isHighBeamActive;
            }

            // Brake Tail Lights
            bool isBraking = currentBrakeInput01 > 0.05f || isSpringEmergencyBrakeEngaged;
            if (brakeTailLights != null)
            {
                foreach (var l in brakeTailLights) if (l != null) l.enabled = isBraking;
            }

            // Reverse Lights
            bool isReversing = currentGearIndex == -1;
            if (reverseLights != null)
            {
                foreach (var l in reverseLights) if (l != null) l.enabled = isReversing;
            }

            // Turn Signals
            if (leftTurnIndicatorLights != null)
            {
                foreach (var l in leftTurnIndicatorLights)
                {
                    if (l != null) l.enabled = (isLeftIndicatorActive || isHazardActive) && indicatorBlinkState;
                }
            }
            if (rightTurnIndicatorLights != null)
            {
                foreach (var l in rightTurnIndicatorLights)
                {
                    if (l != null) l.enabled = (isRightIndicatorActive || isHazardActive) && indicatorBlinkState;
                }
            }
        }

        private void UpdateDoorAnimation(float dt)
        {
            float targetProgress = isDoorOpen ? 1.0f : 0.0f;
            doorAnimationProgress01 = Mathf.MoveTowards(doorAnimationProgress01, targetProgress, dt * 1.8f);

            if (passengerGliderDoorTransform != null)
            {
                passengerGliderDoorTransform.localRotation = Quaternion.Euler(0f, doorAnimationProgress01 * -85f, 0f);
            }
        }
    }
}
