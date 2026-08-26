using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Vehicles
{
    public class VehicleFuelSystem
    {
        public float FuelTankCapacityLiters { get; set; } = 350.0f;
        public float CurrentFuelLiters { get; private set; } = 280.0f;
        public float AdBlueCapacityLiters { get; set; } = 45.0f;
        public float CurrentAdBlueLiters { get; private set; } = 40.0f;

        public float InstantaneousFuelRateLitersPerHour { get; private set; } = 0.0f;
        public float AverageFuelConsumptionLitersPer100Km { get; private set; } = 24.5f;
        public float TotalFuelConsumedLiters { get; private set; } = 0.0f;
        public float TotalDistanceTraveledKm { get; private set; } = 0.0f;

        public bool FuelFilterClogged { get; set; } = false;
        public bool LowFuelWarningLight => (CurrentFuelLiters / FuelTankCapacityLiters) < 0.12f;
        public bool LowAdBlueWarningLight => (CurrentAdBlueLiters / AdBlueCapacityLiters) < 0.10f;
        public bool IsEmpty => CurrentFuelLiters <= 0.05f;

        public float FuelPercent => CoreMath.Clamp01(CurrentFuelLiters / MathF.Max(1.0f, FuelTankCapacityLiters)) * 100.0f;

        public VehicleFuelSystem(float tankCapacity = 350.0f)
        {
            FuelTankCapacityLiters = tankCapacity;
            CurrentFuelLiters = tankCapacity * 0.85f;
        }

        public void Refuel(float liters)
        {
            CurrentFuelLiters = CoreMath.Clamp(CurrentFuelLiters + liters, 0.0f, FuelTankCapacityLiters);
        }

        public void RefillAdBlue(float liters)
        {
            CurrentAdBlueLiters = CoreMath.Clamp(CurrentAdBlueLiters + liters, 0.0f, AdBlueCapacityLiters);
        }

        public void UpdateFuelConsumption(float deltaTime, float engineRpm, float engineLoadRatio, float speedKmh, bool engineRunning)
        {
            if (!engineRunning || IsEmpty)
            {
                InstantaneousFuelRateLitersPerHour = 0.0f;
                return;
            }

            // BSFC (Brake-Specific Fuel Consumption) diesel curve
            // Base idle rate: ~2.2 L/h
            // Full load 280HP @ 1800 RPM: ~42 L/h
            float idleRate = 2.2f * (engineRpm / 650.0f);
            float loadRate = 42.0f * MathF.Pow(CoreMath.Clamp01(engineLoadRatio), 1.25f) * (engineRpm / 2200.0f);
            
            if (FuelFilterClogged)
            {
                loadRate *= 1.15f; // Extra inefficient burning
            }

            InstantaneousFuelRateLitersPerHour = idleRate + loadRate;
            float fuelConsumedThisStep = (InstantaneousFuelRateLitersPerHour / 3600.0f) * deltaTime;

            CurrentFuelLiters = MathF.Max(0.0f, CurrentFuelLiters - fuelConsumedThisStep);
            TotalFuelConsumedLiters += fuelConsumedThisStep;

            // AdBlue / DEF consumption is typically ~5% of diesel consumption for BS6
            float adBlueConsumed = fuelConsumedThisStep * 0.045f;
            CurrentAdBlueLiters = MathF.Max(0.0f, CurrentAdBlueLiters - adBlueConsumed);

            // Rolling distance & average calculation
            float distStepKm = (speedKmh * deltaTime) / 3600.0f;
            TotalDistanceTraveledKm += distStepKm;

            if (TotalDistanceTraveledKm > 1.0f)
            {
                AverageFuelConsumptionLitersPer100Km = (TotalFuelConsumedLiters / TotalDistanceTraveledKm) * 100.0f;
            }
        }
    }
}
