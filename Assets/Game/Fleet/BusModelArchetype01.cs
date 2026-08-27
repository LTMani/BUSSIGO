using System;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.Fleet
{
    public class BusModelArchetype01
    {
        public static VehicleChassisSpec CreateSpecification()
        {
            return new VehicleChassisSpec
            {
                ModelId = "BUS_MODEL_01",
                DisplayName = "South Super Coach Type 01",
                Manufacturer = "Deccan Heavy Automotives Limited",
                Category = (BusCategory)(1),
                LengthMeters = 10.75f,
                WidthMeters = 2.60f,
                HeightMeters = 3.45f,
                WheelbaseMeters = 5.95f,
                FrontOverhangMeters = 2.35f,
                RearOverhangMeters = 3.18f,
                GroundClearanceMeters = 0.26f,
                TurningRadiusMeters = 11.00f,
                KerbMassKg = 8950.0f,
                GrossVehicleWeightKg = 14600.0f,
                FrontAxleWeightRatio = 0.35f,
                AxleCount = 2,
                HasTagAxleSteer = false,
                EngineDisplacementLiters = 5.95f,
                MaxHorsepower = 196.0f,
                MaxPowerRpm = 2200f,
                MaxTorqueNm = 725.0f,
                MaxTorqueRpmMin = 1150f,
                MaxTorqueRpmMax = 1650f,
                IdleRpm = 600f,
                MaxEngineRpm = 2500f,
                Transmission = TransmissionType.ManualSynchromesh6Speed,
                ForwardGearRatios = new float[] { 6.75f, 3.77f, 2.30f, 1.48f, 1.00f, 0.73f },
                ReverseGearRatio = 6.30f,
                FinalDriveDifferentialRatio = 4.26f,
                DrivetrainEfficiency = 0.89f,
                DragCoefficient = 0.57f,
                FrontalAreaM2 = 7.75f,
                SeatingCapacity = 37,
                SleeperBerthCapacity = 0,
                LuggageVolumeM3 = 7.90f,
                FuelTankCapacityLiters = 320.0f,
                AdBlueTankCapacityLiters = 45f,
                BasePriceInCoins = 2450000,
                MaintenanceCostPerKm = 4.10f,
                BaseComfortScore = 52.8f,
                BaseReliabilityScore = 89.0f
            };
        }
    }
}
