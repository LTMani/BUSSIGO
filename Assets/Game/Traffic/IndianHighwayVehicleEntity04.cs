using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Traffic
{
    public class IndianHighwayVehicleEntity04
    {
        public int VehicleInstanceId { get; set; } = 1004;
        public IndianTrafficVehicleProfile Profile { get; set; }
        public Vector3D Position { get; set; } = Vector3D.Zero;
        public float SpeedKmh { get; set; } = 75.0f;
        public int CurrentLane { get; set; } = 2;
        public bool IsOvertaking { get; set; } = false;
        public float DistanceToLeaderMeters { get; set; } = 57.0f;

        public IndianHighwayVehicleEntity04()
        {
            Profile = IndianTrafficVehicleProfile.CreateDefault((IndianVehicleType)(4));
        }

        public void UpdateVehiclePhysics(float deltaTime)
        {
            float speedMps = SpeedKmh * CoreMath.KmhToMps;
            Position = new Vector3D(Position.X, Position.Y, Position.Z + speedMps * deltaTime);
        }
    }
}
