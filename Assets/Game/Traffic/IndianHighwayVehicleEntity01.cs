using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Traffic
{
    public class IndianHighwayVehicleEntity01
    {
        public int VehicleInstanceId { get; set; } = 1001;
        public IndianTrafficVehicleProfile Profile { get; set; }
        public Vector3D Position { get; set; } = Vector3D.Zero;
        public float SpeedKmh { get; set; } = 60.0f;
        public int CurrentLane { get; set; } = 2;
        public bool IsOvertaking { get; set; } = false;
        public float DistanceToLeaderMeters { get; set; } = 48.0f;

        public IndianHighwayVehicleEntity01()
        {
            Profile = IndianTrafficVehicleProfile.CreateDefault((IndianVehicleType)(1));
        }

        public void UpdateVehiclePhysics(float deltaTime)
        {
            float speedMps = SpeedKmh * CoreMath.KmhToMps;
            Position = new Vector3D(Position.X, Position.Y, Position.Z + speedMps * deltaTime);
        }
    }
}
