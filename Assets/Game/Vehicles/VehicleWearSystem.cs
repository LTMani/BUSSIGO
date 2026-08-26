using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Vehicles
{
    public class VehicleWearSystem
    {
        // Wear factors (1.0 = Brand new 100%, 0.0 = Completely worn out / failure)
        public float TyreTreadCondition { get; set; } = 1.0f;
        public float FrontBrakeLiningCondition { get; set; } = 1.0f;
        public float RearBrakeLiningCondition { get; set; } = 1.0f;
        public float ClutchPlateCondition { get; set; } = 1.0f;
        public float EngineOilHealth { get; set; } = 1.0f;
        public float AirFilterCondition { get; set; } = 1.0f;
        public float SuspensionBushingsCondition { get; set; } = 1.0f;

        public float OdometerKm { get; set; } = 0.0f;
        public float KmSinceLastFullService { get; set; } = 0.0f;

        public bool ServiceRequiredWarning => KmSinceLastFullService >= 15000.0f || EngineOilHealth < 0.20f;
        public bool BrakeWarningLight => FrontBrakeLiningCondition < 0.15f || RearBrakeLiningCondition < 0.15f;

        public void AccumulateWear(float distanceTraveledKm, float brakeEnergyJoules, float clutchSlipEnergyJoules, float engineRpmHours, float roadRoughness)
        {
            OdometerKm += distanceTraveledKm;
            KmSinceLastFullService += distanceTraveledKm;

            // Tyre wear: ~80,000 km normal tyre life
            float tyreWearRate = (distanceTraveledKm / 80000.0f) * (1.0f + roadRoughness * 0.5f);
            TyreTreadCondition = CoreMath.Clamp01(TyreTreadCondition - tyreWearRate);

            // Brake wear: ~45,000 km or accelerated by harsh braking
            float brakeWear = (distanceTraveledKm / 45000.0f) + (brakeEnergyJoules * 1e-9f);
            FrontBrakeLiningCondition = CoreMath.Clamp01(FrontBrakeLiningCondition - brakeWear * 1.2f);
            RearBrakeLiningCondition = CoreMath.Clamp01(RearBrakeLiningCondition - brakeWear * 0.8f);

            // Clutch wear
            float clutchWear = (distanceTraveledKm / 120000.0f) + (clutchSlipEnergyJoules * 1e-8f);
            ClutchPlateCondition = CoreMath.Clamp01(ClutchPlateCondition - clutchWear);

            // Oil degradation: ~15,000 km oil change interval
            float oilWear = (distanceTraveledKm / 15000.0f) + (engineRpmHours / 500.0f);
            EngineOilHealth = CoreMath.Clamp01(EngineOilHealth - oilWear);

            // Air filter: ~20,000 km (faster on dusty rural Andhra roads)
            float filterWear = (distanceTraveledKm / 20000.0f) * (1.0f + roadRoughness * 0.8f);
            AirFilterCondition = CoreMath.Clamp01(AirFilterCondition - filterWear);

            // Suspension wear: ~100,000 km
            float suspWear = (distanceTraveledKm / 100000.0f) * (1.0f + roadRoughness * 1.5f);
            SuspensionBushingsCondition = CoreMath.Clamp01(SuspensionBushingsCondition - suspWear);
        }

        public void PerformFullService()
        {
            EngineOilHealth = 1.0f;
            AirFilterCondition = 1.0f;
            KmSinceLastFullService = 0.0f;
        }

        public void OverhaulBrakes()
        {
            FrontBrakeLiningCondition = 1.0f;
            RearBrakeLiningCondition = 1.0f;
        }

        public void ReplaceTyres()
        {
            TyreTreadCondition = 1.0f;
        }

        public void ReplaceClutch()
        {
            ClutchPlateCondition = 1.0f;
        }
    }
}
