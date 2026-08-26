using System;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.Fleet
{
    public class BusModelArchetype02
    {
        public static VehicleChassisSpec CreateSpecification()
        {
            return new VehicleChassisSpec
            {
                ModelId = "BUS_MODEL_02",
                DisplayName = "South Super Coach Type 02",
                Manufacturer = "Deccan Heavy Automotives Limited",
                Category = (BusCategory)(2),
                LengthMeters = 11.00f,
                WidthMeters = 2.60f,
                HeightMeters = 3.50f,
                WheelbaseMeters = 6.10f,
                FrontOverhangMeters = 2.35f,
                RearOverhangMeters = 3.26f,
                GroundClearanceMeters = 0.26f,
                TurningRadiusMeters = 11.20f,
                KerbMassKg = 9400.0f,
                GrossVehicleWeightKg = 15200.0f,
                FrontAxleWeightRatio = 0.35f,
                AxleCount = 2,
                HasTagAxleSteer = False,
                EngineDisplacementLiters = 6.30f,
                MaxHorsepower = 212.0f,
                MaxPowerRpm = 2200f,
                MaxTorqueNm = 800.0f,
                MaxTorqueRpmMin = 1150f,
                MaxTorqueRpmMax = 1650f,
                IdleRpm = 600f,
                MaxEngineRpm = 2500f,
                Transmission = TransmissionType.ManualSynchromesh6Speed,
                ForwardGearRatios = new float[] { 6.70f, 3.74f, 2.30f, 1.48f, 1.00f, 0.73f },
                ReverseGearRatio = 6.30f,
                FinalDriveDifferentialRatio = 4.22f,
                DrivetrainEfficiency = 0.89f,
                DragCoefficient = 0.56f,
                FrontalAreaM2 = 7.75f,
                SeatingCapacity = 38,
                SleeperBerthCapacity = 0,
                LuggageVolumeM3 = 8.30f,
                FuelTankCapacityLiters = 340.0f,
                AdBlueTankCapacityLiters = 45f,
                BasePriceInCoins = 3100000,
                MaintenanceCostPerKm = 4.40f,
                BaseComfortScore = 55.6f,
                BaseReliabilityScore = 90.0f
            };
        }
    }
}
