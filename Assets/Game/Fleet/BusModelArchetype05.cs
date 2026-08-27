using System;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.Fleet
{
    public class BusModelArchetype05
    {
        public static VehicleChassisSpec CreateSpecification()
        {
            return new VehicleChassisSpec
            {
                ModelId = "BUS_MODEL_05",
                DisplayName = "South Super Coach Type 05",
                Manufacturer = "Deccan Heavy Automotives Limited",
                Category = (BusCategory)(5),
                LengthMeters = 11.75f,
                WidthMeters = 2.60f,
                HeightMeters = 3.65f,
                WheelbaseMeters = 6.55f,
                FrontOverhangMeters = 2.35f,
                RearOverhangMeters = 3.50f,
                GroundClearanceMeters = 0.26f,
                TurningRadiusMeters = 11.80f,
                KerbMassKg = 10750.0f,
                GrossVehicleWeightKg = 17000.0f,
                FrontAxleWeightRatio = 0.35f,
                AxleCount = 2,
                HasTagAxleSteer = false,
                EngineDisplacementLiters = 7.35f,
                MaxHorsepower = 260.0f,
                MaxPowerRpm = 2200f,
                MaxTorqueNm = 1025.0f,
                MaxTorqueRpmMin = 1150f,
                MaxTorqueRpmMax = 1650f,
                IdleRpm = 600f,
                MaxEngineRpm = 2500f,
                Transmission = TransmissionType.ManualSynchromesh6Speed,
                ForwardGearRatios = new float[] { 6.55f, 3.65f, 2.30f, 1.48f, 1.00f, 0.73f },
                ReverseGearRatio = 6.30f,
                FinalDriveDifferentialRatio = 4.10f,
                DrivetrainEfficiency = 0.89f,
                DragCoefficient = 0.53f,
                FrontalAreaM2 = 7.75f,
                SeatingCapacity = 41,
                SleeperBerthCapacity = 0,
                LuggageVolumeM3 = 9.50f,
                FuelTankCapacityLiters = 400.0f,
                AdBlueTankCapacityLiters = 45f,
                BasePriceInCoins = 5050000,
                MaintenanceCostPerKm = 5.30f,
                BaseComfortScore = 64.0f,
                BaseReliabilityScore = 93.0f
            };
        }
    }
}
