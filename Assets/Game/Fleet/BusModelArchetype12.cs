using System;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.Fleet
{
    public class BusModelArchetype12
    {
        public static VehicleChassisSpec CreateSpecification()
        {
            return new VehicleChassisSpec
            {
                ModelId = "BUS_MODEL_12",
                DisplayName = "South Super Coach Type 12",
                Manufacturer = "Deccan Heavy Automotives Limited",
                Category = (BusCategory)(2),
                LengthMeters = 13.50f,
                WidthMeters = 2.60f,
                HeightMeters = 4.00f,
                WheelbaseMeters = 7.60f,
                FrontOverhangMeters = 2.35f,
                RearOverhangMeters = 4.06f,
                GroundClearanceMeters = 0.26f,
                TurningRadiusMeters = 13.20f,
                KerbMassKg = 13900.0f,
                GrossVehicleWeightKg = 21200.0f,
                FrontAxleWeightRatio = 0.35f,
                AxleCount = 2,
                HasTagAxleSteer = False,
                EngineDisplacementLiters = 9.80f,
                MaxHorsepower = 372.0f,
                MaxPowerRpm = 2200f,
                MaxTorqueNm = 1550.0f,
                MaxTorqueRpmMin = 1150f,
                MaxTorqueRpmMax = 1650f,
                IdleRpm = 600f,
                MaxEngineRpm = 2500f,
                Transmission = TransmissionType.AutomatedManualTransmission,
                ForwardGearRatios = new float[] { 6.20f, 3.44f, 2.30f, 1.48f, 1.00f, 0.73f },
                ReverseGearRatio = 6.30f,
                FinalDriveDifferentialRatio = 3.82f,
                DrivetrainEfficiency = 0.89f,
                DragCoefficient = 0.46f,
                FrontalAreaM2 = 7.75f,
                SeatingCapacity = 48,
                SleeperBerthCapacity = 0,
                LuggageVolumeM3 = 12.30f,
                FuelTankCapacityLiters = 540.0f,
                AdBlueTankCapacityLiters = 45f,
                BasePriceInCoins = 9600000,
                MaintenanceCostPerKm = 7.40f,
                BaseComfortScore = 83.6f,
                BaseReliabilityScore = 90.0f
            };
        }
    }
}
