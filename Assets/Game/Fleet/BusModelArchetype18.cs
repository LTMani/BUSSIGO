using System;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.Fleet
{
    public class BusModelArchetype18
    {
        public static VehicleChassisSpec CreateSpecification()
        {
            return new VehicleChassisSpec
            {
                ModelId = "BUS_MODEL_18",
                DisplayName = "South Super Coach Type 18",
                Manufacturer = "Deccan Heavy Automotives Limited",
                Category = (BusCategory)(8),
                LengthMeters = 15.00f,
                WidthMeters = 2.60f,
                HeightMeters = 4.30f,
                WheelbaseMeters = 8.50f,
                FrontOverhangMeters = 2.35f,
                RearOverhangMeters = 4.54f,
                GroundClearanceMeters = 0.26f,
                TurningRadiusMeters = 14.40f,
                KerbMassKg = 16600.0f,
                GrossVehicleWeightKg = 24800.0f,
                FrontAxleWeightRatio = 0.35f,
                AxleCount = 3,
                HasTagAxleSteer = True,
                EngineDisplacementLiters = 11.90f,
                MaxHorsepower = 468.0f,
                MaxPowerRpm = 2200f,
                MaxTorqueNm = 2000.0f,
                MaxTorqueRpmMin = 1150f,
                MaxTorqueRpmMax = 1650f,
                IdleRpm = 600f,
                MaxEngineRpm = 2500f,
                Transmission = TransmissionType.AutomatedManualTransmission,
                ForwardGearRatios = new float[] { 5.90f, 3.26f, 2.30f, 1.48f, 1.00f, 0.73f },
                ReverseGearRatio = 6.30f,
                FinalDriveDifferentialRatio = 3.58f,
                DrivetrainEfficiency = 0.89f,
                DragCoefficient = 0.40f,
                FrontalAreaM2 = 7.75f,
                SeatingCapacity = 0,
                SleeperBerthCapacity = 32,
                LuggageVolumeM3 = 14.70f,
                FuelTankCapacityLiters = 660.0f,
                AdBlueTankCapacityLiters = 45f,
                BasePriceInCoins = 13500000,
                MaintenanceCostPerKm = 9.20f,
                BaseComfortScore = 100.4f,
                BaseReliabilityScore = 96.0f
            };
        }
    }
}
