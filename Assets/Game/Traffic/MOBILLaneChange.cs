using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Traffic
{
    public class MOBILLaneChange
    {
        public float PolitenessFactor { get; set; } = 0.35f;
        public float AccelerationThresholdMps2 { get; set; } = 0.2f;
        public float SafeBrakingLimitMps2 { get; set; } = 4.0f;

        public bool EvaluateLaneChangeDecision(
            float currentAccelMps2,
            float newLaneAccelMps2,
            float currentFollowerAccelMps2,
            float newLaneFollowerAccelMps2,
            float followerSafeBrakingLimit)
        {
            // Safety Criterion: new follower must not brake harder than safe limit
            if (newLaneFollowerAccelMps2 < -followerSafeBrakingLimit)
            {
                return false;
            }

            // Incentive Criterion: delta_self + p * (delta_new_follower + delta_old_follower) > a_th
            float selfAdvantage = newLaneAccelMps2 - currentAccelMps2;
            float otherDisadvantage = (newLaneFollowerAccelMps2 - 0.0f) + (currentFollowerAccelMps2 - 0.0f);

            float totalAdvantage = selfAdvantage + PolitenessFactor * otherDisadvantage;
            return totalAdvantage > AccelerationThresholdMps2;
        }
    }
}
