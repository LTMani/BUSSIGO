using System;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.Fleet
{
    public class BusModelArchetype14
    {
        public static VehicleChassisSpec CreateSpecification()
        {
            return new VehicleChassisSpec
            {
                ModelId = "BUS_MODEL_14",
                DisplayName = "South Super Coach Type 14",
                Manufacturer = "Deccan Heavy Automotives Limited",
                Category = (BusCategory)(4),
                LengthMeters = 14.00f,
                WidthMeters = 2.60f,
                HeightMeters = 4.10f,
                WheelbaseMeters = 7.90f,
                FrontOverhangMeters = 2.35f,
                RearOverhangMeters = 4.22f,
                GroundClearanceMeters = 0.26f,
                TurningRadiusMeters = 13.60f,
                KerbMassKg = 14800.0f,
                GrossVehicleWeightKg = 22400.0f,
                FrontAxleWeightRatio = 0.35f,
                AxleCount = 3,
                HasTagAxleSteer = false,
                EngineDisplacementLiters = 10.50f,
                MaxHorsepower = 404.0f,
                MaxPowerRpm = 2200f,
                MaxTorqueNm = 1700.0f,
                MaxTorqueRpmMin = 1150f,
                MaxTorqueRpmMax = 1650f,
                IdleRpm = 600f,
                MaxEngineRpm = 2500f,
                Transmission = TransmissionType.AutomatedManualTransmission,
                ForwardGearRatios = new float[] { 6.10f, 3.38f, 2.30f, 1.48f, 1.00f, 0.73f },
                ReverseGearRatio = 6.30f,
                FinalDriveDifferentialRatio = 3.74f,
                DrivetrainEfficiency = 0.89f,
                DragCoefficient = 0.44f,
                FrontalAreaM2 = 7.75f,
                SeatingCapacity = 50,
                SleeperBerthCapacity = 0,
                LuggageVolumeM3 = 13.10f,
                FuelTankCapacityLiters = 580.0f,
                AdBlueTankCapacityLiters = 45f,
                BasePriceInCoins = 10900000,
                MaintenanceCostPerKm = 8.00f,
                BaseComfortScore = 89.2f,
                BaseReliabilityScore = 92.0f
            };
        }
    }
}
