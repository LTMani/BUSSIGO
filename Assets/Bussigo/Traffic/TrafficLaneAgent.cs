using System;
using UnityEngine;
using Bussigo.Route;

namespace Bussigo.Traffic
{
    public enum LaneChangeState
    {
        FollowingLane = 0,
        ChangingLeft = 1,
        ChangingRight = 2
    }

    /// <summary>
    /// Manages lane discipline, lateral centering, and MOBIL overtaking transitions.
    /// </summary>
    public class TrafficLaneAgent : MonoBehaviour
    {
        public int currentLaneIndex = 0;
        public int targetLaneIndex = 0;
        public LaneChangeState changeState = LaneChangeState.FollowingLane;

        public float currentLateralOffsetMeters = 0f;
        public float lateralVelocity = 0f;
        public float laneWidthMeters = 3.75f;
        public float maxLateralSpeedMps = 1.6f;

        public float targetLateralOffsetMeters => targetLaneIndex * laneWidthMeters;

        public void UpdateLateralKinematics(float deltaTime)
        {
            if (changeState != LaneChangeState.FollowingLane)
            {
                float targetOffset = targetLateralOffsetMeters;
                float diff = targetOffset - currentLateralOffsetMeters;

                if (Mathf.Abs(diff) < 0.05f)
                {
                    currentLateralOffsetMeters = targetOffset;
                    currentLaneIndex = targetLaneIndex;
                    changeState = LaneChangeState.FollowingLane;
                    lateralVelocity = 0f;
                }
                else
                {
                    float step = Mathf.Sign(diff) * Mathf.Min(Mathf.Abs(diff), maxLateralSpeedMps * deltaTime);
                    currentLateralOffsetMeters += step;
                }
            }
        }

        public bool EvaluateMOBILSafeLaneChange(int newLaneIndex, float frontGapMeters, float rearGapMeters, float minSafeGap = 12.0f)
        {
            if (changeState != LaneChangeState.FollowingLane) return false;
            if (newLaneIndex < 0 || newLaneIndex > 1) return false; // 2-lane single direction highway

            // Safety Condition: Front and rear gaps must exceed safety margins
            if (frontGapMeters < minSafeGap || rearGapMeters < minSafeGap)
            {
                return false;
            }

            // Initiate Change
            targetLaneIndex = newLaneIndex;
            changeState = (newLaneIndex < currentLaneIndex) ? LaneChangeState.ChangingLeft : LaneChangeState.ChangingRight;
            return true;
        }
    }
}
