using System;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.Fleet
{
    public class BusModelArchetype15
    {
        public static VehicleChassisSpec CreateSpecification()
        {
            return new VehicleChassisSpec
            {
                ModelId = "BUS_MODEL_15",
                DisplayName = "South Super Coach Type 15",
                Manufacturer = "Deccan Heavy Automotives Limited",
                Category = (BusCategory)(5),
                LengthMeters = 14.25f,
                WidthMeters = 2.60f,
                HeightMeters = 4.15f,
                WheelbaseMeters = 8.05f,
                FrontOverhangMeters = 2.35f,
                RearOverhangMeters = 4.30f,
                GroundClearanceMeters = 0.26f,
                TurningRadiusMeters = 13.80f,
                KerbMassKg = 15250.0f,
                GrossVehicleWeightKg = 23000.0f,
                FrontAxleWeightRatio = 0.35f,
                AxleCount = 3,
                HasTagAxleSteer = True,
                EngineDisplacementLiters = 10.85f,
                MaxHorsepower = 420.0f,
                MaxPowerRpm = 2200f,
                MaxTorqueNm = 1775.0f,
                MaxTorqueRpmMin = 1150f,
                MaxTorqueRpmMax = 1650f,
                IdleRpm = 600f,
                MaxEngineRpm = 2500f,
                Transmission = TransmissionType.AutomatedManualTransmission,
                ForwardGearRatios = new float[] { 6.05f, 3.35f, 2.30f, 1.48f, 1.00f, 0.73f },
                ReverseGearRatio = 6.30f,
                FinalDriveDifferentialRatio = 3.70f,
                DrivetrainEfficiency = 0.89f,
                DragCoefficient = 0.43f,
                FrontalAreaM2 = 7.75f,
                SeatingCapacity = 51,
                SleeperBerthCapacity = 0,
                LuggageVolumeM3 = 13.50f,
                FuelTankCapacityLiters = 600.0f,
                AdBlueTankCapacityLiters = 45f,
                BasePriceInCoins = 11550000,
                MaintenanceCostPerKm = 8.30f,
                BaseComfortScore = 92.0f,
                BaseReliabilityScore = 93.0f
            };
        }
    }
}
