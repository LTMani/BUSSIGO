using System;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.Fleet
{
    public class BusModelArchetype06
    {
        public static VehicleChassisSpec CreateSpecification()
        {
            return new VehicleChassisSpec
            {
                ModelId = "BUS_MODEL_06",
                DisplayName = "South Super Coach Type 06",
                Manufacturer = "Deccan Heavy Automotives Limited",
                Category = (BusCategory)(6),
                LengthMeters = 12.00f,
                WidthMeters = 2.60f,
                HeightMeters = 3.70f,
                WheelbaseMeters = 6.70f,
                FrontOverhangMeters = 2.35f,
                RearOverhangMeters = 3.58f,
                GroundClearanceMeters = 0.26f,
                TurningRadiusMeters = 12.00f,
                KerbMassKg = 11200.0f,
                GrossVehicleWeightKg = 17600.0f,
                FrontAxleWeightRatio = 0.35f,
                AxleCount = 2,
                HasTagAxleSteer = false,
                EngineDisplacementLiters = 7.70f,
                MaxHorsepower = 276.0f,
                MaxPowerRpm = 2200f,
                MaxTorqueNm = 1100.0f,
                MaxTorqueRpmMin = 1150f,
                MaxTorqueRpmMax = 1650f,
                IdleRpm = 600f,
                MaxEngineRpm = 2500f,
                Transmission = TransmissionType.ManualSynchromesh6Speed,
                ForwardGearRatios = new float[] { 6.50f, 3.62f, 2.30f, 1.48f, 1.00f, 0.73f },
                ReverseGearRatio = 6.30f,
                FinalDriveDifferentialRatio = 4.06f,
                DrivetrainEfficiency = 0.89f,
                DragCoefficient = 0.52f,
                FrontalAreaM2 = 7.75f,
                SeatingCapacity = 42,
                SleeperBerthCapacity = 0,
                LuggageVolumeM3 = 9.90f,
                FuelTankCapacityLiters = 420.0f,
                AdBlueTankCapacityLiters = 45f,
                BasePriceInCoins = 5700000,
                MaintenanceCostPerKm = 5.60f,
                BaseComfortScore = 66.8f,
                BaseReliabilityScore = 94.0f
            };
        }
    }
}
