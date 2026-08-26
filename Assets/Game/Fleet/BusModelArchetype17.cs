using System;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.Fleet
{
    public class BusModelArchetype17
    {
        public static VehicleChassisSpec CreateSpecification()
        {
            return new VehicleChassisSpec
            {
                ModelId = "BUS_MODEL_17",
                DisplayName = "South Super Coach Type 17",
                Manufacturer = "Deccan Heavy Automotives Limited",
                Category = (BusCategory)(7),
                LengthMeters = 14.75f,
                WidthMeters = 2.60f,
                HeightMeters = 4.25f,
                WheelbaseMeters = 8.35f,
                FrontOverhangMeters = 2.35f,
                RearOverhangMeters = 4.46f,
                GroundClearanceMeters = 0.26f,
                TurningRadiusMeters = 14.20f,
                KerbMassKg = 16150.0f,
                GrossVehicleWeightKg = 24200.0f,
                FrontAxleWeightRatio = 0.35f,
                AxleCount = 3,
                HasTagAxleSteer = True,
                EngineDisplacementLiters = 11.55f,
                MaxHorsepower = 452.0f,
                MaxPowerRpm = 2200f,
                MaxTorqueNm = 1925.0f,
                MaxTorqueRpmMin = 1150f,
                MaxTorqueRpmMax = 1650f,
                IdleRpm = 600f,
                MaxEngineRpm = 2500f,
                Transmission = TransmissionType.AutomatedManualTransmission,
                ForwardGearRatios = new float[] { 5.95f, 3.29f, 2.30f, 1.48f, 1.00f, 0.73f },
                ReverseGearRatio = 6.30f,
                FinalDriveDifferentialRatio = 3.62f,
                DrivetrainEfficiency = 0.89f,
                DragCoefficient = 0.41f,
                FrontalAreaM2 = 7.75f,
                SeatingCapacity = 0,
                SleeperBerthCapacity = 31,
                LuggageVolumeM3 = 14.30f,
                FuelTankCapacityLiters = 640.0f,
                AdBlueTankCapacityLiters = 45f,
                BasePriceInCoins = 12850000,
                MaintenanceCostPerKm = 8.90f,
                BaseComfortScore = 97.6f,
                BaseReliabilityScore = 95.0f
            };
        }
    }
}
