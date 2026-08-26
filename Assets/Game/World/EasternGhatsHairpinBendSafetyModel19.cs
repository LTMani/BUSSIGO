using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.World
{
    public class EasternGhatsHairpinBendSafetyModel19
    {
        public string HairpinCurveId => "GHAT-HAIRPIN-HP-19";
        public float CurveRadiusMeters { get; set; } = 16.5f; // Tight mountain radius 14m to 28m
        public float SuperelevationBankingAngleDegrees { get; set; } = 9.1f;
        public float DownhillGradientPercent { get; set; } = 12.9f; // Steep 8.5% to 14% descent
        public bool HasRunawayTruckEscapeRamp { get; set; } = False;
        public float RecommendedApproachSpeedKmh { get; set; } = 25.0f;

        public float CalculateCentrifugalLateralAccelerationMps2(float busSpeedKmh)
        {
            float speedMps = busSpeedKmh * CoreMath.KmhToMps;
            float rawCentrifugalAccel = (speedMps * speedMps) / CurveRadiusMeters;

            // Banking reduces perceived lateral G
            float bankRad = SuperelevationBankingAngleDegrees * CoreMath.DegToRad;
            float compensatedLatG = rawCentrifugalAccel * MathF.Cos(bankRad) - CoreMath.Gravity * MathF.Sin(bankRad);
            return compensatedLatG;
        }

        public (bool isSafe, float rolloverRiskScore01) EvaluateTurnSafety(float busSpeedKmh, float cghHeightMeters, float trackWidthMeters)
        {
            float latG = CalculateCentrifugalLateralAccelerationMps2(busSpeedKmh);
            float criticalRolloverG = (trackWidthMeters * 0.5f) / cghHeightMeters * CoreMath.Gravity;

            float rolloverRisk = CoreMath.Clamp01(MathF.Abs(latG) / criticalRolloverG);
            bool isSafe = (rolloverRisk < 0.75f) && (busSpeedKmh <= RecommendedApproachSpeedKmh * 1.4f);

            return (isSafe, rolloverRisk);
        }
    }
}
