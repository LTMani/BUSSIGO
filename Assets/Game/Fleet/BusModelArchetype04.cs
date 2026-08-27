using System;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.Fleet
{
    public class BusModelArchetype04
    {
        public static VehicleChassisSpec CreateSpecification()
        {
            return new VehicleChassisSpec
            {
                ModelId = "BUS_MODEL_04",
                DisplayName = "South Super Coach Type 04",
                Manufacturer = "Deccan Heavy Automotives Limited",
                Category = (BusCategory)(4),
                LengthMeters = 11.50f,
                WidthMeters = 2.60f,
                HeightMeters = 3.60f,
                WheelbaseMeters = 6.40f,
                FrontOverhangMeters = 2.35f,
                RearOverhangMeters = 3.42f,
                GroundClearanceMeters = 0.26f,
                TurningRadiusMeters = 11.60f,
                KerbMassKg = 10300.0f,
                GrossVehicleWeightKg = 16400.0f,
                FrontAxleWeightRatio = 0.35f,
                AxleCount = 2,
                HasTagAxleSteer = false,
                EngineDisplacementLiters = 7.00f,
                MaxHorsepower = 244.0f,
                MaxPowerRpm = 2200f,
                MaxTorqueNm = 950.0f,
                MaxTorqueRpmMin = 1150f,
                MaxTorqueRpmMax = 1650f,
                IdleRpm = 600f,
                MaxEngineRpm = 2500f,
                Transmission = TransmissionType.ManualSynchromesh6Speed,
                ForwardGearRatios = new float[] { 6.60f, 3.68f, 2.30f, 1.48f, 1.00f, 0.73f },
                ReverseGearRatio = 6.30f,
                FinalDriveDifferentialRatio = 4.14f,
                DrivetrainEfficiency = 0.89f,
                DragCoefficient = 0.54f,
                FrontalAreaM2 = 7.75f,
                SeatingCapacity = 40,
                SleeperBerthCapacity = 0,
                LuggageVolumeM3 = 9.10f,
                FuelTankCapacityLiters = 380.0f,
                AdBlueTankCapacityLiters = 45f,
                BasePriceInCoins = 4400000,
                MaintenanceCostPerKm = 5.00f,
                BaseComfortScore = 61.2f,
                BaseReliabilityScore = 92.0f
            };
        }
    }
}
