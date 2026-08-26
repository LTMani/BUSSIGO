using System;
using UnityEngine;

namespace Bussigo.Game.Runtime3D.Vehicle
{
    public class Cockpit3DInstrumentAnimator : MonoBehaviour
    {
        public UnityBusController3D busController;

        [Header("3D Cockpit Transforms")]
        public Transform steeringWheelTransform;
        public Transform speedometerNeedleTransform;
        public Transform tachometerNeedleTransform;

        [Header("Instrument Calibration")]
        public float maxSteeringWheelTurnDegrees = 450f; // 2.5 turns lock-to-lock
        public float speedometerMinAngle = -135f;
        public float speedometerMaxAngle = 135f;
        public float tachometerMinAngle = -135f;
        public float tachometerMaxAngle = 135f;

        private float currentWheelAngle = 0f;

        private void Update()
        {
            if (busController == null) return;

            // 1. Steering Wheel Rotation
            if (steeringWheelTransform != null)
            {
                float targetWheelAngle = busController.currentSteeringInput * maxSteeringWheelTurnDegrees;
                currentWheelAngle = Mathf.MoveTowards(currentWheelAngle, targetWheelAngle, Time.deltaTime * 900f);
                steeringWheelTransform.localRotation = Quaternion.Euler(22f, 0f, -currentWheelAngle);
            }

            // 2. Speedometer Needle
            if (speedometerNeedleTransform != null)
            {
                float normSpeed = Mathf.Clamp01(Mathf.Abs(busController.currentSpeedKmh) / 140f);
                float needleAngle = Mathf.Lerp(speedometerMinAngle, speedometerMaxAngle, normSpeed);
                speedometerNeedleTransform.localRotation = Quaternion.Euler(0f, 0f, -needleAngle);
            }

            // 3. Tachometer RPM Needle
            if (tachometerNeedleTransform != null)
            {
                float normRpm = Mathf.InverseLerp(busController.idleRpm, busController.maxRpm, busController.currentEngineRpm);
                float rpmAngle = Mathf.Lerp(tachometerMinAngle, tachometerMaxAngle, normRpm);
                tachometerNeedleTransform.localRotation = Quaternion.Euler(0f, 0f, -rpmAngle);
            }
        }
    }
}
