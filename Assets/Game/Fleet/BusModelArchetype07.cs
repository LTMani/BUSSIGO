using System;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.Fleet
{
    public class BusModelArchetype07
    {
        public static VehicleChassisSpec CreateSpecification()
        {
            return new VehicleChassisSpec
            {
                ModelId = "BUS_MODEL_07",
                DisplayName = "South Super Coach Type 07",
                Manufacturer = "Deccan Heavy Automotives Limited",
                Category = (BusCategory)(7),
                LengthMeters = 12.25f,
                WidthMeters = 2.60f,
                HeightMeters = 3.75f,
                WheelbaseMeters = 6.85f,
                FrontOverhangMeters = 2.35f,
                RearOverhangMeters = 3.66f,
                GroundClearanceMeters = 0.26f,
                TurningRadiusMeters = 12.20f,
                KerbMassKg = 11650.0f,
                GrossVehicleWeightKg = 18200.0f,
                FrontAxleWeightRatio = 0.35f,
                AxleCount = 2,
                HasTagAxleSteer = False,
                EngineDisplacementLiters = 8.05f,
                MaxHorsepower = 292.0f,
                MaxPowerRpm = 2200f,
                MaxTorqueNm = 1175.0f,
                MaxTorqueRpmMin = 1150f,
                MaxTorqueRpmMax = 1650f,
                IdleRpm = 600f,
                MaxEngineRpm = 2500f,
                Transmission = TransmissionType.ManualSynchromesh6Speed,
                ForwardGearRatios = new float[] { 6.45f, 3.59f, 2.30f, 1.48f, 1.00f, 0.73f },
                ReverseGearRatio = 6.30f,
                FinalDriveDifferentialRatio = 4.02f,
                DrivetrainEfficiency = 0.89f,
                DragCoefficient = 0.51f,
                FrontalAreaM2 = 7.75f,
                SeatingCapacity = 43,
                SleeperBerthCapacity = 0,
                LuggageVolumeM3 = 10.30f,
                FuelTankCapacityLiters = 440.0f,
                AdBlueTankCapacityLiters = 45f,
                BasePriceInCoins = 6350000,
                MaintenanceCostPerKm = 5.90f,
                BaseComfortScore = 69.6f,
                BaseReliabilityScore = 95.0f
            };
        }
    }
}
