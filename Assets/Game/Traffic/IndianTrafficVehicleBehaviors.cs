using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Traffic
{
    public enum IndianVehicleType
    {
        MultiAxleHeavyLorry, // Tata Prima / Ashok Leyland 14-wheeler
        StateRTCBus,          // Express / Pallevelugu RTC coach
        AutoRickshaw3Wheeler, // Bajaj RE / Piaggio Ape
        Motorcycle2Wheeler,   // Hero Splendor / Honda Activa
        PassengerCarHatchback,// Maruti Swift / Hyundai i20
        HighwaySUV,           // Mahindra Scorpio / Toyota Innova
        EmergencyAmbulance108 // 108 Emergency GVK Ambulance
    }

    public class IndianTrafficVehicleProfile
    {
        public IndianVehicleType VehicleType { get; set; }
        public string ModelName { get; set; }
        public float LengthMeters { get; set; }
        public float WidthMeters { get; set; }
        public float MaxSpeedKmh { get; set; }
        public float AccelerationCapabilityMps2 { get; set; }
        public float HornLikelihood01 { get; set; }
        public float LaneDiscipline01 { get; set; } // 1.0 = strict lane following, 0.3 = aggressive weaving
        public bool SoundHornAtOvertake { get; set; } = true;

        public static IndianTrafficVehicleProfile CreateDefault(IndianVehicleType type)
        {
            switch (type)
            {
                case IndianVehicleType.MultiAxleHeavyLorry:
                    return new IndianTrafficVehicleProfile
                    {
                        VehicleType = type,
                        ModelName = "Tata 1618 Cargo Lorry",
                        LengthMeters = 9.8f,
                        WidthMeters = 2.5f,
                        MaxSpeedKmh = 65.0f,
                        AccelerationCapabilityMps2 = 0.65f,
                        HornLikelihood01 = 0.85f,
                        LaneDiscipline01 = 0.45f
                    };
                case IndianVehicleType.AutoRickshaw3Wheeler:
                    return new IndianTrafficVehicleProfile
                    {
                        VehicleType = type,
                        ModelName = "Bajaj Compact 3-Wheeler",
                        LengthMeters = 2.7f,
                        WidthMeters = 1.3f,
                        MaxSpeedKmh = 50.0f,
                        AccelerationCapabilityMps2 = 1.2f,
                        HornLikelihood01 = 0.90f,
                        LaneDiscipline01 = 0.25f // Often hugs road shoulder or cuts across
                    };
                case IndianVehicleType.Motorcycle2Wheeler:
                    return new IndianTrafficVehicleProfile
                    {
                        VehicleType = type,
                        ModelName = "125cc Commuter Bike",
                        LengthMeters = 2.0f,
                        WidthMeters = 0.8f,
                        MaxSpeedKmh = 75.0f,
                        AccelerationCapabilityMps2 = 2.2f,
                        HornLikelihood01 = 0.70f,
                        LaneDiscipline01 = 0.20f // Filters between traffic lanes
                    };
                case IndianVehicleType.StateRTCBus:
                    return new IndianTrafficVehicleProfile
                    {
                        VehicleType = type,
                        ModelName = "Ashok Leyland Viking RTC",
                        LengthMeters = 11.5f,
                        WidthMeters = 2.6f,
                        MaxSpeedKmh = 85.0f,
                        AccelerationCapabilityMps2 = 1.1f,
                        HornLikelihood01 = 0.95f,
                        LaneDiscipline01 = 0.60f
                    };
                case IndianVehicleType.EmergencyAmbulance108:
                    return new IndianTrafficVehicleProfile
                    {
                        VehicleType = type,
                        ModelName = "108 Force Emergency Ambulance",
                        LengthMeters = 5.4f,
                        WidthMeters = 2.0f,
                        MaxSpeedKmh = 110.0f,
                        AccelerationCapabilityMps2 = 2.5f,
                        HornLikelihood01 = 1.0f,
                        LaneDiscipline01 = 0.50f
                    };
                case IndianVehicleType.HighwaySUV:
                default:
                    return new IndianTrafficVehicleProfile
                    {
                        VehicleType = IndianVehicleType.HighwaySUV,
                        ModelName = "Highway Cruiser SUV",
                        LengthMeters = 4.8f,
                        WidthMeters = 1.9f,
                        MaxSpeedKmh = 120.0f,
                        AccelerationCapabilityMps2 = 2.4f,
                        HornLikelihood01 = 0.60f,
                        LaneDiscipline01 = 0.75f
                    };
            }
        }
    }
}
