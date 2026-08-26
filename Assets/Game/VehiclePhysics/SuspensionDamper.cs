using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.VehiclePhysics
{
    public class SuspensionDamper
    {
        public float SpringRateNewtonPerMeter { get; set; } = 85000.0f; // Heavy commercial bus spring
        public float BumpDampingRateNewtonSecPerMeter { get; set; } = 12000.0f;
        public float ReboundDampingRateNewtonSecPerMeter { get; set; } = 18000.0f;
        public float RestLengthMeters { get; set; } = 0.65f;
        public float MaxTravelCompressionMeters { get; set; } = 0.18f;
        public float MaxTravelDroopMeters { get; set; } = 0.14f;

        public float CurrentLengthMeters { get; private set; } = 0.65f;
        public float CompressionMeters => RestLengthMeters - CurrentLengthMeters;
        public float CompressionVelocityMps { get; private set; } = 0.0f;

        public float CalculateSpringForce(float currentLengthMeters, float compressionVelocityMps)
        {
            CurrentLengthMeters = CoreMath.Clamp(
                currentLengthMeters,
                RestLengthMeters - MaxTravelCompressionMeters,
                RestLengthMeters + MaxTravelDroopMeters
            );
            CompressionVelocityMps = compressionVelocityMps;

            float x = CompressionMeters;
            float springForce = SpringRateNewtonPerMeter * x;

            // Bump vs Rebound asymmetric damping
            float damperRate = (compressionVelocityMps > 0.0f) ? BumpDampingRateNewtonSecPerMeter : ReboundDampingRateNewtonSecPerMeter;
            float dampingForce = damperRate * compressionVelocityMps;

            float totalForce = springForce + dampingForce;
            return MathF.Max(0.0f, totalForce); // Ground contact normal force cannot be negative
        }
    }
}
