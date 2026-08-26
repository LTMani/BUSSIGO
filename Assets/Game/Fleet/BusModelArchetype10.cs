using System;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.Fleet
{
    public class BusModelArchetype10
    {
        public static VehicleChassisSpec CreateSpecification()
        {
            return new VehicleChassisSpec
            {
                ModelId = "BUS_MODEL_10",
                DisplayName = "South Super Coach Type 10",
                Manufacturer = "Deccan Heavy Automotives Limited",
                Category = (BusCategory)(0),
                LengthMeters = 13.00f,
                WidthMeters = 2.60f,
                HeightMeters = 3.90f,
                WheelbaseMeters = 7.30f,
                FrontOverhangMeters = 2.35f,
                RearOverhangMeters = 3.90f,
                GroundClearanceMeters = 0.26f,
                TurningRadiusMeters = 12.80f,
                KerbMassKg = 13000.0f,
                GrossVehicleWeightKg = 20000.0f,
                FrontAxleWeightRatio = 0.35f,
                AxleCount = 2,
                HasTagAxleSteer = False,
                EngineDisplacementLiters = 9.10f,
                MaxHorsepower = 340.0f,
                MaxPowerRpm = 2200f,
                MaxTorqueNm = 1400.0f,
                MaxTorqueRpmMin = 1150f,
                MaxTorqueRpmMax = 1650f,
                IdleRpm = 600f,
                MaxEngineRpm = 2500f,
                Transmission = TransmissionType.ManualSynchromesh6Speed,
                ForwardGearRatios = new float[] { 6.30f, 3.50f, 2.30f, 1.48f, 1.00f, 0.73f },
                ReverseGearRatio = 6.30f,
                FinalDriveDifferentialRatio = 3.90f,
                DrivetrainEfficiency = 0.89f,
                DragCoefficient = 0.48f,
                FrontalAreaM2 = 7.75f,
                SeatingCapacity = 0,
                SleeperBerthCapacity = 32,
                LuggageVolumeM3 = 11.50f,
                FuelTankCapacityLiters = 500.0f,
                AdBlueTankCapacityLiters = 45f,
                BasePriceInCoins = 8300000,
                MaintenanceCostPerKm = 6.80f,
                BaseComfortScore = 78.0f,
                BaseReliabilityScore = 88.0f
            };
        }
    }
}
