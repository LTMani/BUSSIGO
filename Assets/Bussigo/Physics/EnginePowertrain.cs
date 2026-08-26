using System;
using UnityEngine;

namespace Bussigo.Physics
{
    [Serializable]
    public class EnginePowertrain
    {
        [Header("Engine Specifications")]
        public float idleRpm = 650f;
        public float maxRpm = 2400f;
        public float peakTorqueRpmMin = 1200f;
        public float peakTorqueRpmMax = 1600f;
        public float maxTorqueNm = 1400f;
        public float currentRpm = 650f;

        [Header("Transmission & Gearing")]
        public int currentGear = 1; // -1 = Reverse, 0 = Neutral, 1..6 = Forward
        public float finalDriveRatio = 3.42f;
        public float[] forwardGearRatios = new float[] { 6.82f, 3.68f, 2.19f, 1.41f, 1.00f, 0.74f };
        public float reverseGearRatio = -6.42f;

        public float GetCurrentGearRatio()
        {
            if (currentGear == 0) return 0f;
            if (currentGear == -1) return reverseGearRatio;
            if (currentGear >= 1 && currentGear <= forwardGearRatios.Length)
            {
                return forwardGearRatios[currentGear - 1];
            }
            return 0f;
        }

        public float CalculateEngineTorque(float throttleInput01)
        {
            float rpm = Mathf.Clamp(currentRpm, idleRpm, maxRpm);
            float torqueCurveFactor = 0f;

            if (rpm < peakTorqueRpmMin)
            {
                torqueCurveFactor = Mathf.Lerp(0.70f, 1.0f, (rpm - idleRpm) / (peakTorqueRpmMin - idleRpm));
            }
            else if (rpm <= peakTorqueRpmMax)
            {
                torqueCurveFactor = 1.0f;
            }
            else
            {
                torqueCurveFactor = Mathf.Lerp(1.0f, 0.75f, (rpm - peakTorqueRpmMax) / (maxRpm - peakTorqueRpmMax));
            }

            return maxTorqueNm * torqueCurveFactor * Mathf.Clamp01(throttleInput01);
        }

        public float CalculateWheelTorque(float throttleInput01)
        {
            float engineTorque = CalculateEngineTorque(throttleInput01);
            float gearRatio = GetCurrentGearRatio();
            float transmissionEfficiency = 0.92f;

            return engineTorque * gearRatio * finalDriveRatio * transmissionEfficiency;
        }

        public void UpdateEngineRpm(float vehicleSpeedKmh, float wheelRadiusMeters, float throttleInput01, float dt)
        {
            float gearRatio = GetCurrentGearRatio();
            if (currentGear == 0 || Mathf.Abs(gearRatio) < 0.01f)
            {
                // In neutral, RPM responds to throttle free-revving
                float targetRpm = Mathf.Lerp(idleRpm, maxRpm, throttleInput01);
                currentRpm = Mathf.Lerp(currentRpm, targetRpm, dt * 6.0f);
            }
            else
            {
                // In gear, RPM is coupled to wheel rotational speed
                float wheelCircumference = 2.0f * Mathf.PI * wheelRadiusMeters;
                float wheelRpm = (vehicleSpeedKmh / 3.6f) / wheelCircumference * 60.0f;
                float mechanicalRpm = Mathf.Abs(wheelRpm * gearRatio * finalDriveRatio);

                currentRpm = Mathf.Clamp(Mathf.Max(idleRpm, mechanicalRpm), idleRpm, maxRpm);
            }
        }

        public void ShiftUp()
        {
            if (currentGear < forwardGearRatios.Length)
            {
                currentGear++;
            }
        }

        public void ShiftDown()
        {
            if (currentGear > -1)
            {
                currentGear--;
            }
        }
    }
}
