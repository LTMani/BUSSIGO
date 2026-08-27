using System;
using UnityEngine;

namespace Bussigo.Vehicle
{
    /// <summary>
    /// Drives 3D cockpit steering wheel rotation and dashboard gauge needle telemetry.
    /// </summary>
    public class BusCockpitController : MonoBehaviour
    {
        public BusChassisController chassisController;
        public BusModelRigHierarchy rigHierarchy;

        [Header("Gauge Needle Transforms")]
        public Transform speedometerNeedleTransform;
        public Transform tachometerNeedleTransform;
        public Transform airPressureNeedleTransform;

        [Header("Needle Angle Ranges (Degrees)")]
        public float speedoMinAngle = 0f;
        public float speedoMaxAngle = 240f;
        public float speedoMaxKmh = 140f;

        public float tachoMinAngle = 0f;
        public float tachoMaxAngle = 240f;
        public float tachoMaxRpm = 2600f;

        public float airMinAngle = 0f;
        public float airMaxAngle = 180f;
        public float airMaxBar = 10f;

        [Header("Steering Wheel Max Degrees")]
        public float steeringWheelMaxRotationDegrees = 540f; // 1.5 turns lock-to-lock

        private void Update()
        {
            if (chassisController == null || rigHierarchy == null) return;

            // 1. Steering Wheel Rotation
            float steerInput = chassisController.SteerInput;
            if (rigHierarchy.steeringWheelTransform != null)
            {
                float targetZRotation = -steerInput * steeringWheelMaxRotationDegrees;
                Quaternion targetRot = Quaternion.Euler(25f, 0f, targetZRotation);
                rigHierarchy.steeringWheelTransform.localRotation = Quaternion.Slerp(
                    rigHierarchy.steeringWheelTransform.localRotation,
                    targetRot,
                    Time.deltaTime * 10f
                );
            }

            // 2. Speedometer Needle
            if (speedometerNeedleTransform != null)
            {
                float speedFraction = Mathf.Clamp01(Mathf.Abs(chassisController.currentSpeedKmh) / speedoMaxKmh);
                float angle = Mathf.Lerp(speedoMinAngle, speedoMaxAngle, speedFraction);
                speedometerNeedleTransform.localRotation = Quaternion.Euler(0f, 0f, -angle);
            }

            // 3. Tachometer Needle
            if (tachometerNeedleTransform != null)
            {
                float rpmFraction = Mathf.Clamp01(chassisController.currentEngineRpm / tachoMaxRpm);
                float angle = Mathf.Lerp(tachoMinAngle, tachoMaxAngle, rpmFraction);
                tachometerNeedleTransform.localRotation = Quaternion.Euler(0f, 0f, -angle);
            }

            // 4. Air Pressure Needle
            if (airPressureNeedleTransform != null)
            {
                float airFraction = Mathf.Clamp01(chassisController.primaryAirPressureBar / airMaxBar);
                float angle = Mathf.Lerp(airMinAngle, airMaxAngle, airFraction);
                airPressureNeedleTransform.localRotation = Quaternion.Euler(0f, 0f, -angle);
            }
        }
    }
}
