using System;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.Fleet
{
    public class BusModelArchetype09
    {
        public static VehicleChassisSpec CreateSpecification()
        {
            return new VehicleChassisSpec
            {
                ModelId = "BUS_MODEL_09",
                DisplayName = "South Super Coach Type 09",
                Manufacturer = "Deccan Heavy Automotives Limited",
                Category = (BusCategory)(9),
                LengthMeters = 12.75f,
                WidthMeters = 2.60f,
                HeightMeters = 3.85f,
                WheelbaseMeters = 7.15f,
                FrontOverhangMeters = 2.35f,
                RearOverhangMeters = 3.82f,
                GroundClearanceMeters = 0.26f,
                TurningRadiusMeters = 12.60f,
                KerbMassKg = 12550.0f,
                GrossVehicleWeightKg = 19400.0f,
                FrontAxleWeightRatio = 0.35f,
                AxleCount = 2,
                HasTagAxleSteer = false,
                EngineDisplacementLiters = 8.75f,
                MaxHorsepower = 324.0f,
                MaxPowerRpm = 2200f,
                MaxTorqueNm = 1325.0f,
                MaxTorqueRpmMin = 1150f,
                MaxTorqueRpmMax = 1650f,
                IdleRpm = 600f,
                MaxEngineRpm = 2500f,
                Transmission = TransmissionType.ManualSynchromesh6Speed,
                ForwardGearRatios = new float[] { 6.35f, 3.53f, 2.30f, 1.48f, 1.00f, 0.73f },
                ReverseGearRatio = 6.30f,
                FinalDriveDifferentialRatio = 3.94f,
                DrivetrainEfficiency = 0.89f,
                DragCoefficient = 0.49f,
                FrontalAreaM2 = 7.75f,
                SeatingCapacity = 0,
                SleeperBerthCapacity = 31,
                LuggageVolumeM3 = 11.10f,
                FuelTankCapacityLiters = 480.0f,
                AdBlueTankCapacityLiters = 45f,
                BasePriceInCoins = 7650000,
                MaintenanceCostPerKm = 6.50f,
                BaseComfortScore = 75.2f,
                BaseReliabilityScore = 97.0f
            };
        }
    }
}
