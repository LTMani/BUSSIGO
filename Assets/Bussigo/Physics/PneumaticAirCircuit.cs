using System;
using UnityEngine;

namespace Bussigo.Physics
{
    [Serializable]
    public class PneumaticAirCircuit
    {
        [Header("Pressure Specs (Bar)")]
        public float currentReservoirPressureBar = 8.5f;
        public float maxReservoirPressureBar = 9.2f;
        public float minOperatingPressureBar = 4.5f;
        public float compressorChargeRateBarPerSec = 0.25f;
        public float consumptionPerFullBrakeBar = 0.35f;

        public bool isLowAirPressureWarning => currentReservoirPressureBar < minOperatingPressureBar;
        public bool isSpringBrakeLocked => currentReservoirPressureBar < 3.8f;

        public void UpdateCircuit(float dt, float brakeInput01, bool engineRunning)
        {
            if (brakeInput01 > 0.05f)
            {
                currentReservoirPressureBar = Mathf.Max(0f, currentReservoirPressureBar - (consumptionPerFullBrakeBar * brakeInput01 * dt));
            }

            if (engineRunning && currentReservoirPressureBar < maxReservoirPressureBar)
            {
                currentReservoirPressureBar = Mathf.Min(maxReservoirPressureBar, currentReservoirPressureBar + (compressorChargeRateBarPerSec * dt));
            }
        }

        public float GetBrakingEfficiency()
        {
            if (isSpringBrakeLocked) return 1.0f; // Spring emergency brake lock
            return Mathf.Clamp01(currentReservoirPressureBar / 8.5f);
        }
    }
}
