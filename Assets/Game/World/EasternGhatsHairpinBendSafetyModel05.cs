using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.World
{
    public class EasternGhatsHairpinBendSafetyModel05
    {
        public string HairpinCurveId => "GHAT-HAIRPIN-HP-05";
        public float CurveRadiusMeters { get; set; } = 26.5f; // Tight mountain radius 14m to 28m
        public float SuperelevationBankingAngleDegrees { get; set; } = 6.7f;
        public float DownhillGradientPercent { get; set; } = 8.5f; // Steep 8.5% to 14% descent
        public bool HasRunawayTruckEscapeRamp { get; set; } = false;
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
