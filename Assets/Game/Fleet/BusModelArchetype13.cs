using System;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.Fleet
{
    public class BusModelArchetype13
    {
        public static VehicleChassisSpec CreateSpecification()
        {
            return new VehicleChassisSpec
            {
                ModelId = "BUS_MODEL_13",
                DisplayName = "South Super Coach Type 13",
                Manufacturer = "Deccan Heavy Automotives Limited",
                Category = (BusCategory)(3),
                LengthMeters = 13.75f,
                WidthMeters = 2.60f,
                HeightMeters = 4.05f,
                WheelbaseMeters = 7.75f,
                FrontOverhangMeters = 2.35f,
                RearOverhangMeters = 4.14f,
                GroundClearanceMeters = 0.26f,
                TurningRadiusMeters = 13.40f,
                KerbMassKg = 14350.0f,
                GrossVehicleWeightKg = 21800.0f,
                FrontAxleWeightRatio = 0.35f,
                AxleCount = 3,
                HasTagAxleSteer = False,
                EngineDisplacementLiters = 10.15f,
                MaxHorsepower = 388.0f,
                MaxPowerRpm = 2200f,
                MaxTorqueNm = 1625.0f,
                MaxTorqueRpmMin = 1150f,
                MaxTorqueRpmMax = 1650f,
                IdleRpm = 600f,
                MaxEngineRpm = 2500f,
                Transmission = TransmissionType.AutomatedManualTransmission,
                ForwardGearRatios = new float[] { 6.15f, 3.41f, 2.30f, 1.48f, 1.00f, 0.73f },
                ReverseGearRatio = 6.30f,
                FinalDriveDifferentialRatio = 3.78f,
                DrivetrainEfficiency = 0.89f,
                DragCoefficient = 0.45f,
                FrontalAreaM2 = 7.75f,
                SeatingCapacity = 49,
                SleeperBerthCapacity = 0,
                LuggageVolumeM3 = 12.70f,
                FuelTankCapacityLiters = 560.0f,
                AdBlueTankCapacityLiters = 45f,
                BasePriceInCoins = 10250000,
                MaintenanceCostPerKm = 7.70f,
                BaseComfortScore = 86.4f,
                BaseReliabilityScore = 91.0f
            };
        }
    }
}
