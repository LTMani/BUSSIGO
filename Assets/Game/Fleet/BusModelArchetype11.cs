using System;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.Fleet
{
    public class BusModelArchetype11
    {
        public static VehicleChassisSpec CreateSpecification()
        {
            return new VehicleChassisSpec
            {
                ModelId = "BUS_MODEL_11",
                DisplayName = "South Super Coach Type 11",
                Manufacturer = "Deccan Heavy Automotives Limited",
                Category = (BusCategory)(1),
                LengthMeters = 13.25f,
                WidthMeters = 2.60f,
                HeightMeters = 3.95f,
                WheelbaseMeters = 7.45f,
                FrontOverhangMeters = 2.35f,
                RearOverhangMeters = 3.98f,
                GroundClearanceMeters = 0.26f,
                TurningRadiusMeters = 13.00f,
                KerbMassKg = 13450.0f,
                GrossVehicleWeightKg = 20600.0f,
                FrontAxleWeightRatio = 0.35f,
                AxleCount = 2,
                HasTagAxleSteer = False,
                EngineDisplacementLiters = 9.45f,
                MaxHorsepower = 356.0f,
                MaxPowerRpm = 2200f,
                MaxTorqueNm = 1475.0f,
                MaxTorqueRpmMin = 1150f,
                MaxTorqueRpmMax = 1650f,
                IdleRpm = 600f,
                MaxEngineRpm = 2500f,
                Transmission = TransmissionType.AutomatedManualTransmission,
                ForwardGearRatios = new float[] { 6.25f, 3.47f, 2.30f, 1.48f, 1.00f, 0.73f },
                ReverseGearRatio = 6.30f,
                FinalDriveDifferentialRatio = 3.86f,
                DrivetrainEfficiency = 0.89f,
                DragCoefficient = 0.47f,
                FrontalAreaM2 = 7.75f,
                SeatingCapacity = 47,
                SleeperBerthCapacity = 0,
                LuggageVolumeM3 = 11.90f,
                FuelTankCapacityLiters = 520.0f,
                AdBlueTankCapacityLiters = 45f,
                BasePriceInCoins = 8950000,
                MaintenanceCostPerKm = 7.10f,
                BaseComfortScore = 80.8f,
                BaseReliabilityScore = 89.0f
            };
        }
    }
}
