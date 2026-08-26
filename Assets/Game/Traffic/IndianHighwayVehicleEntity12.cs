using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Traffic
{
    public class IndianHighwayVehicleEntity12
    {
        public int VehicleInstanceId { get; set; } = 1012;
        public IndianTrafficVehicleProfile Profile { get; set; }
        public Vector3D Position { get; set; } = Vector3D.Zero;
        public float SpeedKmh { get; set; } = 75.0f;
        public int CurrentLane { get; set; } = 1;
        public bool IsOvertaking { get; set; } = false;
        public float DistanceToLeaderMeters { get; set; } = 81.0f;

        public IndianHighwayVehicleEntity12()
        {
            Profile = IndianTrafficVehicleProfile.CreateDefault((IndianVehicleType)(5));
        }

        public void UpdateVehiclePhysics(float deltaTime)
        {
            float speedMps = SpeedKmh * CoreMath.KmhToMps;
            Position = new Vector3D(Position.X, Position.Y, Position.Z + speedMps * deltaTime);
        }
    }
}
