using System;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.Fleet
{
    public class BusModelArchetype16
    {
        public static VehicleChassisSpec CreateSpecification()
        {
            return new VehicleChassisSpec
            {
                ModelId = "BUS_MODEL_16",
                DisplayName = "South Super Coach Type 16",
                Manufacturer = "Deccan Heavy Automotives Limited",
                Category = (BusCategory)(6),
                LengthMeters = 14.50f,
                WidthMeters = 2.60f,
                HeightMeters = 4.20f,
                WheelbaseMeters = 8.20f,
                FrontOverhangMeters = 2.35f,
                RearOverhangMeters = 4.38f,
                GroundClearanceMeters = 0.26f,
                TurningRadiusMeters = 14.00f,
                KerbMassKg = 15700.0f,
                GrossVehicleWeightKg = 23600.0f,
                FrontAxleWeightRatio = 0.35f,
                AxleCount = 3,
                HasTagAxleSteer = true,
                EngineDisplacementLiters = 11.20f,
                MaxHorsepower = 436.0f,
                MaxPowerRpm = 2200f,
                MaxTorqueNm = 1850.0f,
                MaxTorqueRpmMin = 1150f,
                MaxTorqueRpmMax = 1650f,
                IdleRpm = 600f,
                MaxEngineRpm = 2500f,
                Transmission = TransmissionType.AutomatedManualTransmission,
                ForwardGearRatios = new float[] { 6.00f, 3.32f, 2.30f, 1.48f, 1.00f, 0.73f },
                ReverseGearRatio = 6.30f,
                FinalDriveDifferentialRatio = 3.66f,
                DrivetrainEfficiency = 0.89f,
                DragCoefficient = 0.42f,
                FrontalAreaM2 = 7.75f,
                SeatingCapacity = 52,
                SleeperBerthCapacity = 0,
                LuggageVolumeM3 = 13.90f,
                FuelTankCapacityLiters = 620.0f,
                AdBlueTankCapacityLiters = 45f,
                BasePriceInCoins = 12200000,
                MaintenanceCostPerKm = 8.60f,
                BaseComfortScore = 94.8f,
                BaseReliabilityScore = 94.0f
            };
        }
    }
}
