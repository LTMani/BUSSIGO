using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Traffic
{
    public class IndianHighwayVehicleEntity21
    {
        public int VehicleInstanceId { get; set; } = 1021;
        public IndianTrafficVehicleProfile Profile { get; set; }
        public Vector3D Position { get; set; } = Vector3D.Zero;
        public float SpeedKmh { get; set; } = 80.0f;
        public int CurrentLane { get; set; } = 1;
        public bool IsOvertaking { get; set; } = false;
        public float DistanceToLeaderMeters { get; set; } = 108.0f;

        public IndianHighwayVehicleEntity21()
        {
            Profile = IndianTrafficVehicleProfile.CreateDefault((IndianVehicleType)(0));
        }

        public void UpdateVehiclePhysics(float deltaTime)
        {
            float speedMps = SpeedKmh * CoreMath.KmhToMps;
            Position = new Vector3D(Position.X, Position.Y, Position.Z + speedMps * deltaTime);
        }
    }
}
