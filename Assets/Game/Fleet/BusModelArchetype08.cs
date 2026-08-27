using System;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.Fleet
{
    public class BusModelArchetype08
    {
        public static VehicleChassisSpec CreateSpecification()
        {
            return new VehicleChassisSpec
            {
                ModelId = "BUS_MODEL_08",
                DisplayName = "South Super Coach Type 08",
                Manufacturer = "Deccan Heavy Automotives Limited",
                Category = (BusCategory)(8),
                LengthMeters = 12.50f,
                WidthMeters = 2.60f,
                HeightMeters = 3.80f,
                WheelbaseMeters = 7.00f,
                FrontOverhangMeters = 2.35f,
                RearOverhangMeters = 3.74f,
                GroundClearanceMeters = 0.26f,
                TurningRadiusMeters = 12.40f,
                KerbMassKg = 12100.0f,
                GrossVehicleWeightKg = 18800.0f,
                FrontAxleWeightRatio = 0.35f,
                AxleCount = 2,
                HasTagAxleSteer = false,
                EngineDisplacementLiters = 8.40f,
                MaxHorsepower = 308.0f,
                MaxPowerRpm = 2200f,
                MaxTorqueNm = 1250.0f,
                MaxTorqueRpmMin = 1150f,
                MaxTorqueRpmMax = 1650f,
                IdleRpm = 600f,
                MaxEngineRpm = 2500f,
                Transmission = TransmissionType.ManualSynchromesh6Speed,
                ForwardGearRatios = new float[] { 6.40f, 3.56f, 2.30f, 1.48f, 1.00f, 0.73f },
                ReverseGearRatio = 6.30f,
                FinalDriveDifferentialRatio = 3.98f,
                DrivetrainEfficiency = 0.89f,
                DragCoefficient = 0.50f,
                FrontalAreaM2 = 7.75f,
                SeatingCapacity = 44,
                SleeperBerthCapacity = 0,
                LuggageVolumeM3 = 10.70f,
                FuelTankCapacityLiters = 460.0f,
                AdBlueTankCapacityLiters = 45f,
                BasePriceInCoins = 7000000,
                MaintenanceCostPerKm = 6.20f,
                BaseComfortScore = 72.4f,
                BaseReliabilityScore = 96.0f
            };
        }
    }
}
