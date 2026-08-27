using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.World
{
    public class HighwayInfrastructureSection11
    {
        public string SectionId => "HWY-SEC-AP-TEL-011";
        public float SectionLengthMeters { get; set; } = 2850.0f;
        public int NumberOfLanes { get; set; } = 4;
        public float AsphaltFrictionCoefficient { get; set; } = 0.935f;
        public bool HasReflectiveCatsEyes { get; set; } = true;
        public bool HasOverheadSignageGantry { get; set; } = false;
        public bool HasGuardRailsBothSides { get; set; } = true;
        public float RoadElevationGradientPercent { get; set; } = -4.28f;

        public Vector3D CalculateSurfaceNormal(float distanceAlongSectionMeters)
        {
            float bankAngleRad = (RoadElevationGradientPercent / 100.0f) * 0.5f;
            float cosB = MathF.Cos(bankAngleRad);
            float sinB = MathF.Sin(bankAngleRad);
            return new Vector3D(-sinB, cosB, 0.0f).Normalized;
        }

        public float GetPermissibleSpeedKmh()
        {
            if (MathF.Abs(RoadElevationGradientPercent) > 6.0f) return 50.0f; // Mountain slope
            if (NumberOfLanes >= 6) return 100.0f;
            return 80.0f;
        }
    }
}
