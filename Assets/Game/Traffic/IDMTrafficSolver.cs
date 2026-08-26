using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Traffic
{
    public class IDMParameters
    {
        public float DesiredVelocityMps { get; set; } = 22.2f; // 80 km/h
        public float SafeTimeHeadwaySec { get; set; } = 1.5f;
        public float MaxAccelerationMps2 { get; set; } = 1.4f;
        public float ComfortableDecelerationMps2 { get; set; } = 2.0f;
        public float MinimumJamDistanceMeters { get; set; } = 3.0f;
        public float AccelerationExponent { get; set; } = 4.0f;
    }

    public static class IDMTrafficSolver
    {
        public static float CalculateIDMAcceleration(
            float currentVelocityMps,
            float leaderVelocityMps,
            float actualNetDistanceMeters,
            IDMParameters p)
        {
            float v = MathF.Max(0.0f, currentVelocityMps);
            float deltaV = v - leaderVelocityMps;

            // Desired dynamic distance: s*(v, dv) = s0 + v*T + (v*dv)/(2*sqrt(a*b))
            float term1 = p.MinimumJamDistanceMeters + v * p.SafeTimeHeadwaySec;
            float term2 = (v * deltaV) / (2.0f * MathF.Sqrt(p.MaxAccelerationMps2 * p.ComfortableDecelerationMps2));
            float sStar = term1 + MathF.Max(0.0f, term2);

            float sActual = MathF.Max(0.5f, actualNetDistanceMeters);

            // Free road term
            float freeRoadRatio = v / MathF.Max(0.1f, p.DesiredVelocityMps);
            float freeRoadTerm = MathF.Pow(freeRoadRatio, p.AccelerationExponent);

            // Interaction term
            float interactionRatio = sStar / sActual;
            float interactionTerm = interactionRatio * interactionRatio;

            float accel = p.MaxAccelerationMps2 * (1.0f - freeRoadTerm - interactionTerm);
            return CoreMath.Clamp(accel, -p.ComfortableDecelerationMps2 * 2.5f, p.MaxAccelerationMps2);
        }
    }
}
