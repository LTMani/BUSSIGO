using System;
using UnityEngine;

namespace Bussigo.Traffic
{
    public enum VehicleCategory
    {
        CarSedan = 0,
        SUV = 1,
        HeavyTruck10Wheel = 2,
        TataAceMiniTruck = 3,
        IntercityBus = 4,
        AutoRickshaw = 5,
        Motorcycle = 6
    }

    [Serializable]
    public class TrafficVehicleProfile
    {
        public VehicleCategory category;
        public string typeName;
        public float lengthMeters;
        public float widthMeters;
        public float massKg;
        public float desiredSpeedKmh;
        public float maxAccelerationMss;
        public float comfortableBrakingMss;
        public float emergencyBrakingMss;
        public float minFollowingDistanceMeters;
        public float headwayTimeSeconds;
        public int preferredLaneIndex; // 0 = Overtaking/Fast, 1 = Cruising, 2 = Slow/Shoulder
        public Color vehicleColor;

        public TrafficVehicleProfile(VehicleCategory cat, string name, float len, float wid, float mass, float maxSpd, float accel, float brake, float s0, float timeHeadway, int prefLane, Color col)
        {
            category = cat;
            typeName = name;
            lengthMeters = len;
            widthMeters = wid;
            massKg = mass;
            desiredSpeedKmh = maxSpd;
            maxAccelerationMss = accel;
            comfortableBrakingMss = brake;
            emergencyBrakingMss = brake * 1.8f;
            minFollowingDistanceMeters = s0;
            headwayTimeSeconds = timeHeadway;
            preferredLaneIndex = prefLane;
            vehicleColor = col;
        }

        public static TrafficVehicleProfile CreateDefault(VehicleCategory cat)
        {
            switch (cat)
            {
                case VehicleCategory.CarSedan:
                    return new TrafficVehicleProfile(cat, "Sedan", 4.6f, 1.8f, 1400f, 95f, 2.4f, 3.2f, 3.0f, 1.2f, 0, new Color(0.85f, 0.2f, 0.2f));
                case VehicleCategory.SUV:
                    return new TrafficVehicleProfile(cat, "SUV", 4.9f, 1.9f, 2100f, 90f, 2.0f, 3.0f, 3.5f, 1.3f, 0, new Color(0.9f, 0.9f, 0.95f));
                case VehicleCategory.HeavyTruck10Wheel:
                    return new TrafficVehicleProfile(cat, "10-Wheel Heavy Truck", 9.8f, 2.5f, 16000f, 65f, 0.8f, 1.8f, 5.0f, 2.0f, 1, new Color(0.95f, 0.6f, 0.1f));
                case VehicleCategory.TataAceMiniTruck:
                    return new TrafficVehicleProfile(cat, "Tata Goods Vehicle", 4.1f, 1.6f, 1800f, 60f, 1.2f, 2.2f, 3.0f, 1.5f, 1, new Color(0.2f, 0.5f, 0.85f));
                case VehicleCategory.IntercityBus:
                    return new TrafficVehicleProfile(cat, "Private Sleeper Coach", 12.0f, 2.6f, 14000f, 85f, 1.1f, 2.0f, 6.0f, 1.8f, 0, new Color(0.1f, 0.7f, 0.4f));
                case VehicleCategory.AutoRickshaw:
                    return new TrafficVehicleProfile(cat, "Auto-Rickshaw", 2.8f, 1.3f, 650f, 50f, 1.4f, 2.5f, 2.0f, 1.2f, 1, new Color(0.95f, 0.85f, 0.1f));
                case VehicleCategory.Motorcycle:
                    return new TrafficVehicleProfile(cat, "Motorcycle", 2.1f, 0.8f, 160f, 80f, 2.8f, 3.5f, 1.5f, 1.0f, 1, new Color(0.15f, 0.15f, 0.2f));
                default:
                    return new TrafficVehicleProfile(VehicleCategory.CarSedan, "Sedan", 4.6f, 1.8f, 1400f, 90f, 2.0f, 3.0f, 3.0f, 1.2f, 0, Color.white);
            }
        }
    }
}
