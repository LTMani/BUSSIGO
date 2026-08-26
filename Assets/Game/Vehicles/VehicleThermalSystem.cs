using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Vehicles
{
    public class VehicleThermalSystem
    {
        public float AmbientTemperatureCelsius { get; set; } = 34.0f; // Typical South India ambient
        public float EngineCoolantTemperature { get; private set; } = 34.0f;
        public float EngineOilTemperature { get; private set; } = 34.0f;
        public float TransmissionFluidTemperature { get; private set; } = 34.0f;
        public float BrakeRotorsTemperatureFront { get; private set; } = 34.0f;
        public float BrakeRotorsTemperatureRear { get; private set; } = 34.0f;

        public float ThermostatOpeningTempCelsius { get; set; } = 82.0f;
        public float RadiatorFanKickInTempCelsius { get; set; } = 92.0f;
        public bool RadiatorFanActive { get; private set; } = false;

        public bool OverheatWarningLight => EngineCoolantTemperature >= 105.0f;
        public bool EngineDeratedDueToHeat => EngineCoolantTemperature >= 112.0f;

        public void Update(float deltaTime, float engineRpm, float engineLoadRatio, float speedKmh, float brakeInput, bool engineRunning)
        {
            float targetCoolantTemp = AmbientTemperatureCelsius;
            float targetOilTemp = AmbientTemperatureCelsius;

            if (engineRunning)
            {
                // Engine thermal heat generation
                float heatGenerationKw = 15.0f + 75.0f * (engineLoadRatio * (engineRpm / 2200.0f));
                
                // Airflow cooling over radiator
                float vehicleAirflowSpeed = speedKmh * CoreMath.KmhToMps;
                float coolingEfficiency = 0.4f + (vehicleAirflowSpeed / 30.0f) * 0.6f;

                RadiatorFanActive = EngineCoolantTemperature >= RadiatorFanKickInTempCelsius;
                if (RadiatorFanActive) coolingEfficiency += 0.45f;

                float thermostatFlow = CoreMath.Clamp01((EngineCoolantTemperature - ThermostatOpeningTempCelsius) / 10.0f);
                float heatDissipationKw = (EngineCoolantTemperature - AmbientTemperatureCelsius) * coolingEfficiency * (0.2f + 0.8f * thermostatFlow);

                float netHeat = (heatGenerationKw - heatDissipationKw) * deltaTime * 0.08f;
                EngineCoolantTemperature = CoreMath.Clamp(EngineCoolantTemperature + netHeat, AmbientTemperatureCelsius, 125.0f);

                // Oil follows coolant with thermal lag
                EngineOilTemperature = CoreMath.MoveTowards(EngineOilTemperature, EngineCoolantTemperature + (engineLoadRatio * 15.0f), deltaTime * 0.5f);
                TransmissionFluidTemperature = CoreMath.MoveTowards(TransmissionFluidTemperature, 75.0f + (speedKmh / 100.0f * 20.0f), deltaTime * 0.2f);
            }
            else
            {
                // Cool down naturally towards ambient
                EngineCoolantTemperature = CoreMath.MoveTowards(EngineCoolantTemperature, AmbientTemperatureCelsius, deltaTime * 0.15f);
                EngineOilTemperature = CoreMath.MoveTowards(EngineOilTemperature, AmbientTemperatureCelsius, deltaTime * 0.12f);
                TransmissionFluidTemperature = CoreMath.MoveTowards(TransmissionFluidTemperature, AmbientTemperatureCelsius, deltaTime * 0.1f);
                RadiatorFanActive = false;
            }

            // Brake thermal model (Ghat descents heat up drums/rotors significantly)
            if (brakeInput > 0.05f)
            {
                float brakeHeatRate = brakeInput * (speedKmh * 0.8f) * deltaTime * 4.5f;
                BrakeRotorsTemperatureFront += brakeHeatRate * 0.6f;
                BrakeRotorsTemperatureRear += brakeHeatRate * 0.4f;
            }
            else
            {
                float airCoolRate = (1.0f + (speedKmh / 40.0f)) * deltaTime * 1.5f;
                BrakeRotorsTemperatureFront = CoreMath.MoveTowards(BrakeRotorsTemperatureFront, AmbientTemperatureCelsius, airCoolRate);
                BrakeRotorsTemperatureRear = CoreMath.MoveTowards(BrakeRotorsTemperatureRear, AmbientTemperatureCelsius, airCoolRate);
            }
        }
    }
}
