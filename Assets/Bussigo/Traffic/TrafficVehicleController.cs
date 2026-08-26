using System;
using UnityEngine;

namespace Bussigo.Traffic
{
    public enum DriverBehaviorState
    {
        FreeFlow = 0,
        Following = 1,
        Braking = 2,
        Stopped = 3,
        Accelerating = 4
    }

    /// <summary>
    /// Microscopic Intelligent Driver Model (IDM) vehicle physics and longitudinal controller.
    /// </summary>
    public class TrafficVehicleController : MonoBehaviour
    {
        public TrafficVehicleProfile profile;
        public TrafficLaneAgent laneAgent;

        [Header("Telemetry State")]
        public float currentSpeedMps = 0f;
        public float currentAccelerationMss = 0f;
        public float longitudinalDistanceMeters = 0f;
        public DriverBehaviorState behaviorState = DriverBehaviorState.FreeFlow;

        [Header("Environmental Constraints")]
        public float activeSpeedLimitKmh = 90.0f;
        public float speedLimitMps => (activeSpeedLimitKmh / 3.6f);

        public float currentSpeedKmh => currentSpeedMps * 3.6f;

        public void Initialize(TrafficVehicleProfile p, float initialSpeedKmh, float initialDistMeters)
        {
            profile = p;
            currentSpeedMps = initialSpeedKmh / 3.6f;
            longitudinalDistanceMeters = initialDistMeters;
            if (laneAgent == null) laneAgent = gameObject.AddComponent<TrafficLaneAgent>();
            laneAgent.currentLaneIndex = profile.preferredLaneIndex;
            laneAgent.targetLaneIndex = profile.preferredLaneIndex;
            laneAgent.currentLateralOffsetMeters = laneAgent.targetLateralOffsetMeters;
        }

        public void StepIDM(float deltaTime, float distanceToLeadMeters, float leadSpeedMps)
        {
            if (profile == null) return;

            float v = currentSpeedMps;
            float desiredSpeedMps = Mathf.Min(profile.desiredSpeedKmh / 3.6f, speedLimitMps);
            float aMax = profile.maxAccelerationMss;
            float bComf = profile.comfortableBrakingMss;
            float s0 = profile.minFollowingDistanceMeters;
            float T = profile.headwayTimeSeconds;

            float netDistance = distanceToLeadMeters - profile.lengthMeters;

            // IDM Desired Dynamic Headway
            float deltaV = v - leadSpeedMps;
            float sStar = s0 + (v * T) + ((v * deltaV) / (2.0f * Mathf.Sqrt(aMax * bComf)));

            // Free flow acceleration component: aMax * (1 - (v / v0)^4)
            float freeFlowTerm = 1.0f - Mathf.Pow(Mathf.Clamp01(v / Mathf.Max(0.1f, desiredSpeedMps)), 4.0f);
            
            // Interaction braking component: (s* / s)^2
            float interactionTerm = 0f;
            if (netDistance > 0.1f && distanceToLeadMeters < 300.0f)
            {
                interactionTerm = Mathf.Pow(Mathf.Max(0f, sStar) / netDistance, 2.0f);
            }

            // Net Acceleration
            float targetAccel = aMax * (freeFlowTerm - interactionTerm);

            // Emergency braking cap
            if (netDistance < s0 && distanceToLeadMeters < 150.0f)
            {
                targetAccel = -profile.emergencyBrakingMss;
            }

            targetAccel = Mathf.Clamp(targetAccel, -profile.emergencyBrakingMss, aMax);
            currentAccelerationMss = targetAccel;

            // Integrate Kinematics
            currentSpeedMps = Mathf.Max(0f, currentSpeedMps + currentAccelerationMss * deltaTime);
            longitudinalDistanceMeters += currentSpeedMps * deltaTime;

            // Update behavior state
            if (currentSpeedMps < 0.1f) behaviorState = DriverBehaviorState.Stopped;
            else if (currentAccelerationMss < -0.8f) behaviorState = DriverBehaviorState.Braking;
            else if (currentAccelerationMss > 0.4f) behaviorState = DriverBehaviorState.Accelerating;
            else if (interactionTerm > 0.3f) behaviorState = DriverBehaviorState.Following;
            else behaviorState = DriverBehaviorState.FreeFlow;

            // Update lateral lane positioning
            if (laneAgent != null) laneAgent.UpdateLateralKinematics(deltaTime);

            // Update 3D World Transform
            float lateralX = laneAgent != null ? laneAgent.currentLateralOffsetMeters : 0f;
            transform.position = new Vector3(lateralX, 0.4f, longitudinalDistanceMeters);
        }
    }
}
