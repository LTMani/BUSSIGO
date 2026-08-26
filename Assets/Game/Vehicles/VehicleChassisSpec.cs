using System;

namespace Bussigo.Game.Vehicles
{
    public enum BusCategory
    {
        RuralOrdinary,       // Pallevelugu
        CityCommuter,        // Metro Express / Mitra
        IntercityExpress,    // Express 3+2
        UltraDeluxe,         // 2+2 Semi-luxury pushback
        SuperLuxury,         // 2+2 Air suspension luxury
        GarudaAC,            // 2+2 Volvo/Scania AC recliner
        GarudaPlusMultiAxle, // 6x2 Multi-axle 13.8m / 14.5m luxury
        AmaravatiMultiAxle,  // Premium Scania/Volvo 410HP
        VennelaACSleeper,    // 2+1 AC Berth Sleeper
        PrivateLuxurySleeper // High-deck private coach
    }

    public enum TransmissionType
    {
        ManualSynchromesh6Speed,
        ManualSynchromesh8Speed,
        AutomatedManualTransmission,
        FullyAutomaticTorqueConverter
    }

    public enum FuelType
    {
        DieselBS6,
        CNG,
        ElectricBattery
    }

    public class VehicleChassisSpec
    {
        public string ModelId { get; set; }
        public string DisplayName { get; set; }
        public string Manufacturer { get; set; }
        public BusCategory Category { get; set; }
        public FuelType EngineFuelType { get; set; } = FuelType.DieselBS6;

        // Dimensions (Meters)
        public float LengthMeters { get; set; } = 12.0f;
        public float WidthMeters { get; set; } = 2.6f;
        public float HeightMeters { get; set; } = 3.6f;
        public float WheelbaseMeters { get; set; } = 6.2f;
        public float FrontOverhangMeters { get; set; } = 2.4f;
        public float RearOverhangMeters { get; set; } = 3.4f;
        public float GroundClearanceMeters { get; set; } = 0.28f;
        public float TurningRadiusMeters { get; set; } = 11.5f;

        // Mass (Kilograms)
        public float KerbMassKg { get; set; } = 10500.0f;
        public float GrossVehicleWeightKg { get; set; } = 16200.0f;
        public float FrontAxleWeightRatio { get; set; } = 0.35f; // Unladen
        public int AxleCount { get; set; } = 2; // 2 or 3 (Multi-axle)
        public bool HasTagAxleSteer { get; set; } = false;

        // Powertrain Parameters
        public float EngineDisplacementLiters { get; set; } = 7.7f;
        public float MaxHorsepower { get; set; } = 280.0f;
        public float MaxPowerRpm { get; set; } = 2200.0f;
        public float MaxTorqueNm { get; set; } = 1100.0f;
        public float MaxTorqueRpmMin { get; set; } = 1200.0f;
        public float MaxTorqueRpmMax { get; set; } = 1600.0f;
        public float IdleRpm { get; set; } = 600.0f;
        public float MaxEngineRpm { get; set; } = 2500.0f;

        // Transmission
        public TransmissionType Transmission { get; set; } = TransmissionType.ManualSynchromesh6Speed;
        public float[] ForwardGearRatios { get; set; } = new float[] { 6.81f, 3.82f, 2.30f, 1.48f, 1.00f, 0.73f };
        public float ReverseGearRatio { get; set; } = 6.30f;
        public float FinalDriveDifferentialRatio { get; set; } = 4.30f;
        public float DrivetrainEfficiency { get; set; } = 0.88f;

        // Aerodynamics
        public float DragCoefficient { get; set; } = 0.55f;
        public float FrontalAreaM2 { get; set; } = 7.8f;

        // Capacities
        public int SeatingCapacity { get; set; } = 49;
        public int SleeperBerthCapacity { get; set; } = 0;
        public float LuggageVolumeM3 { get; set; } = 8.5f;
        public float FuelTankCapacityLiters { get; set; } = 350.0f;
        public float AdBlueTankCapacityLiters { get; set; } = 45.0f;

        // Pricing & Maintenance
        public long BasePriceInCoins { get; set; } = 3500000;
        public float MaintenanceCostPerKm { get; set; } = 4.5f;
        public float BaseComfortScore { get; set; } = 75.0f;
        public float BaseReliabilityScore { get; set; } = 92.0f;
    }
}
