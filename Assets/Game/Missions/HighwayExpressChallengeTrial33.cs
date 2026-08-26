using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Missions
{
    public class HighwayExpressChallengeTrial33
    {
        public string ChallengeId => "CHALLENGE-EXPRESS-NH65-033";
        public string Title => "Timed Express Run Sector 33";
        public float TargetTimeMinutes { get; set; } = 195.0f;
        public float MinimumRequiredPunctualityPercent { get; set; } = 92.0f;
        public float RewardMultiplier => 1.70f;

        public (bool isCompleted, float rewardBonus) EvaluateTrialResult(float actualTimeMinutes, float passengerComfortScore)
        {
            bool onTime = actualTimeMinutes <= TargetTimeMinutes;
            bool comfortable = passengerComfortScore >= 85.0f;

            if (onTime && comfortable)
            {
                float timeSaved = TargetTimeMinutes - actualTimeMinutes;
                float bonus = timeSaved * 250.0f * RewardMultiplier;
                return (true, MathF.Max(5000f, bonus));
            }
            return (false, 0.0f);
        }
    }
}
