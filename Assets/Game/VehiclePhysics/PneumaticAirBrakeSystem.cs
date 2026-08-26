using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.VehiclePhysics
{
    public class PneumaticAirBrakeSystem
    {
        public float PrimaryReservoirPressureBar { get; private set; } = 8.5f;   // Rear circuit (8.5 bar normal)
        public float SecondaryReservoirPressureBar { get; private set; } = 8.5f; // Front circuit
        public float MaxCompressorGovernorCutoutBar { get; set; } = 9.2f;
        public float CompressorGovernorCutinBar { get; set; } = 7.0f;
        public bool CompressorLoaded { get; private set; } = false;

        public bool ParkingBrakeEngaged { get; private set; } = false;
        public float ServiceBrakeTreadleApplication { get; private set; } = 0.0f;
        public float RetarderApplicationRatio { get; private set; } = 0.0f; // 0.0 to 1.0 (5 stages)

        public bool LowAirPressureAlarm => PrimaryReservoirPressureBar < 5.5f || SecondaryReservoirPressureBar < 5.5f;
        public bool SpringBrakeEmergencyLocked => PrimaryReservoirPressureBar < 3.8f; // Maxi brakes auto-apply

        public event Action OnAirPurgeBlowoff;

        public void SetTreadleFootValve(float input)
        {
            ServiceBrakeTreadleApplication = CoreMath.Clamp01(input);
        }

        public void SetParkingBrake(bool engaged)
        {
            ParkingBrakeEngaged = engaged;
        }

        public void SetRetarderLevel(int stage) // 0 to 4
        {
            RetarderApplicationRatio = CoreMath.Clamp01(stage / 4.0f);
        }

        public void Update(float deltaTime, float engineRpm, bool engineRunning)
        {
            // Air compressor pump simulation
            if (engineRunning)
            {
                if (PrimaryReservoirPressureBar <= CompressorGovernorCutinBar || SecondaryReservoirPressureBar <= CompressorGovernorCutinBar)
                {
                    CompressorLoaded = true;
                }
                else if (PrimaryReservoirPressureBar >= MaxCompressorGovernorCutoutBar && SecondaryReservoirPressureBar >= MaxCompressorGovernorCutoutBar)
                {
                    if (CompressorLoaded)
                    {
                        OnAirPurgeBlowoff?.Invoke();
                    }
                    CompressorLoaded = false;
                }

                if (CompressorLoaded)
                {
                    float pumpRateBarPerSec = 0.25f * (engineRpm / 1500.0f);
                    PrimaryReservoirPressureBar = MathF.Min(MaxCompressorGovernorCutoutBar, PrimaryReservoirPressureBar + pumpRateBarPerSec * deltaTime);
                    SecondaryReservoirPressureBar = MathF.Min(MaxCompressorGovernorCutoutBar, SecondaryReservoirPressureBar + pumpRateBarPerSec * deltaTime);
                }
            }

            // Air consumption when applying service brakes
            if (ServiceBrakeTreadleApplication > 0.05f)
            {
                float airConsumptionRate = ServiceBrakeTreadleApplication * 0.15f * deltaTime;
                PrimaryReservoirPressureBar = MathF.Max(0.0f, PrimaryReservoirPressureBar - airConsumptionRate);
                SecondaryReservoirPressureBar = MathF.Max(0.0f, SecondaryReservoirPressureBar - airConsumptionRate);
            }
        }

        public float CalculateBrakeTorqueNm(float maxServiceBrakeTorqueNm, bool isFrontAxle)
        {
            if (ParkingBrakeEngaged || SpringBrakeEmergencyLocked)
            {
                return maxServiceBrakeTorqueNm * 0.95f; // Heavy spring brake lock
            }

            float reservoirPressure = isFrontAxle ? SecondaryReservoirPressureBar : PrimaryReservoirPressureBar;
            float pressureFactor = CoreMath.Clamp01(reservoirPressure / 6.5f);

            float serviceTorque = maxServiceBrakeTorqueNm * ServiceBrakeTreadleApplication * pressureFactor;
            float retarderTorque = isFrontAxle ? 0.0f : (maxServiceBrakeTorqueNm * 0.45f * RetarderApplicationRatio);

            return serviceTorque + retarderTorque;
        }
    }
}
