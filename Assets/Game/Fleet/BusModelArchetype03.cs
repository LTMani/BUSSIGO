using System;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.Fleet
{
    public class BusModelArchetype03
    {
        public static VehicleChassisSpec CreateSpecification()
        {
            return new VehicleChassisSpec
            {
                ModelId = "BUS_MODEL_03",
                DisplayName = "South Super Coach Type 03",
                Manufacturer = "Deccan Heavy Automotives Limited",
                Category = (BusCategory)(3),
                LengthMeters = 11.25f,
                WidthMeters = 2.60f,
                HeightMeters = 3.55f,
                WheelbaseMeters = 6.25f,
                FrontOverhangMeters = 2.35f,
                RearOverhangMeters = 3.34f,
                GroundClearanceMeters = 0.26f,
                TurningRadiusMeters = 11.40f,
                KerbMassKg = 9850.0f,
                GrossVehicleWeightKg = 15800.0f,
                FrontAxleWeightRatio = 0.35f,
                AxleCount = 2,
                HasTagAxleSteer = false,
                EngineDisplacementLiters = 6.65f,
                MaxHorsepower = 228.0f,
                MaxPowerRpm = 2200f,
                MaxTorqueNm = 875.0f,
                MaxTorqueRpmMin = 1150f,
                MaxTorqueRpmMax = 1650f,
                IdleRpm = 600f,
                MaxEngineRpm = 2500f,
                Transmission = TransmissionType.ManualSynchromesh6Speed,
                ForwardGearRatios = new float[] { 6.65f, 3.71f, 2.30f, 1.48f, 1.00f, 0.73f },
                ReverseGearRatio = 6.30f,
                FinalDriveDifferentialRatio = 4.18f,
                DrivetrainEfficiency = 0.89f,
                DragCoefficient = 0.55f,
                FrontalAreaM2 = 7.75f,
                SeatingCapacity = 39,
                SleeperBerthCapacity = 0,
                LuggageVolumeM3 = 8.70f,
                FuelTankCapacityLiters = 360.0f,
                AdBlueTankCapacityLiters = 45f,
                BasePriceInCoins = 3750000,
                MaintenanceCostPerKm = 4.70f,
                BaseComfortScore = 58.4f,
                BaseReliabilityScore = 91.0f
            };
        }
    }
}
