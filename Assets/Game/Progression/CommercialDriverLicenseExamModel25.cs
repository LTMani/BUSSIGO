using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Progression
{
    public class CommercialDriverLicenseExamModel25
    {
        public string ExamCode => "RTO-EXAM-AP-TEL-025";
        public LicenseTier TargetLicenseTier { get; set; } = (LicenseTier)(1);
        public int RequiredDriverLevel { get; set; } = 13;
        public float MinimumPassScorePercentage { get; set; } = 85.0f;

        public bool ScoreExamCandidate(float parkingPrecisionScore, float smoothDrivingScore, float speedLimitComplianceScore)
        {
            float averageScore = (parkingPrecisionScore * 0.35f) + (smoothDrivingScore * 0.35f) + (speedLimitComplianceScore * 0.30f);
            return averageScore >= MinimumPassScorePercentage;
        }
    }
}
