using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Weather
{
    public class TropicalPuddleHydrodynamicsSolver30
    {
        public string DrainageZoneId => "DRAINAGE-ZONE-AP-030";
        public float StandingWaterDepthMm { get; private set; } = 0.0f;
        public float MaxPuddleDepthCapacityMm { get; set; } = 25.0f;
        public float CrossSlopeDrainageRateMmPerMin { get; set; } = 17.0f;

        public void AccumulateRainfall(float rainfallRateMmPerHour, float deltaTime)
        {
            float rainfallMmPerSec = rainfallRateMmPerHour / 3600.0f;
            float drainageMmPerSec = CrossSlopeDrainageRateMmPerMin / 60.0f;

            float netWaterGain = (rainfallMmPerSec - drainageMmPerSec) * deltaTime;
            StandingWaterDepthMm = CoreMath.Clamp(StandingWaterDepthMm + netWaterGain, 0.0f, MaxPuddleDepthCapacityMm);
        }

        public (float frictionMultiplier, bool isHydroplaning) CalculateTyreHydroplaningRisk(float busSpeedKmh, float tyreTreadDepthMm)
        {
            // NASA Hydroplaning Velocity Formula: V_h = 6.35 * sqrt(p_psi) km/h
            // Commercial bus tyre pressure ~ 120 PSI -> V_h ~ 69.5 knots = 128 km/h on deep water
            float effectiveWaterDepthMm = MathF.Max(0.0f, StandingWaterDepthMm - tyreTreadDepthMm);

            if (effectiveWaterDepthMm <= 1.0f)
            {
                return (1.0f - (StandingWaterDepthMm / 30.0f) * 0.35f, false);
            }

            float criticalSpeedKmh = 6.35f * MathF.Sqrt(120.0f) * 1.852f * (1.0f - (effectiveWaterDepthMm / MaxPuddleDepthCapacityMm) * 0.4f);

            if (busSpeedKmh >= criticalSpeedKmh)
            {
                return (0.18f, true); // Complete water film hydroplaning loss of control
            }

            float friction = CoreMath.Lerp(0.85f, 0.40f, busSpeedKmh / criticalSpeedKmh);
            return (friction, false);
        }
    }
}
