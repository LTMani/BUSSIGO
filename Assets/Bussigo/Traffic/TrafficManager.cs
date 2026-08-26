using System;
using System.Collections.Generic;
using UnityEngine;
using Bussigo.Core;
using Bussigo.Vehicle;

namespace Bussigo.Traffic
{
    /// <summary>
    /// Master traffic simulation manager updating microscopic IDM physics, lane-based lead search, and toll queues.
    /// </summary>
    public class TrafficManager : MonoBehaviour, IService
    {
        public TrafficSpawner spawner;
        public BusChassisController playerBus;

        [Header("Toll Plaza Zone")]
        public float tollPlazaLocationZ = 32800f; // Kanchikacherla FASTag Toll Plaza
        public float tollApproachRadius = 300f;

        public void Initialize()
        {
            if (spawner == null)
            {
                spawner = gameObject.AddComponent<TrafficSpawner>();
                spawner.InitializePool(40);
            }
            ServiceLocator.Register<TrafficManager>(this);
            Debug.Log("[BUSSIGO] TrafficManager initialized with microscopic IDM simulation.");
        }

        public void Shutdown()
        {
            // Clean shutdown
        }

        private void Update()
        {
            if (spawner == null) return;

            float deltaTime = Time.deltaTime;
            if (deltaTime <= 0f || deltaTime > 0.1f) deltaTime = 0.02f;

            float playerZ = (playerBus != null) ? playerBus.transform.position.z : 0f;
            float playerSpeedMps = (playerBus != null) ? Mathf.Abs(playerBus.currentSpeedKmh) / 3.6f : 0f;
            int playerLane = 0;

            var activeList = spawner.ActiveVehicles;

            // Update each traffic vehicle with microscopic IDM
            for (int i = 0; i < activeList.Count; i++)
            {
                var vehicle = activeList[i];
                int vLane = vehicle.laneAgent != null ? vehicle.laneAgent.currentLaneIndex : 0;
                float vZ = vehicle.longitudinalDistanceMeters;

                // 1. Find immediate lead vehicle in same lane
                float nearestLeadDist = 9999f;
                float leadSpeedMps = 90f / 3.6f;

                // Check other traffic vehicles
                for (int j = 0; j < activeList.Count; j++)
                {
                    if (i == j) continue;
                    var other = activeList[j];
                    int oLane = other.laneAgent != null ? other.laneAgent.currentLaneIndex : 0;

                    if (oLane == vLane && other.longitudinalDistanceMeters > vZ)
                    {
                        float gap = other.longitudinalDistanceMeters - vZ;
                        if (gap < nearestLeadDist)
                        {
                            nearestLeadDist = gap;
                            leadSpeedMps = other.currentSpeedMps;
                        }
                    }
                }

                // Check Player Bus as lead obstacle
                if (playerLane == vLane && playerZ > vZ)
                {
                    float gapToPlayer = playerZ - vZ;
                    if (gapToPlayer < nearestLeadDist)
                    {
                        nearestLeadDist = gapToPlayer;
                        leadSpeedMps = playerSpeedMps;
                    }
                }

                // Check Toll Plaza slowing/queueing
                if (Mathf.Abs(vZ - tollPlazaLocationZ) < tollApproachRadius && vZ < tollPlazaLocationZ)
                {
                    float gapToToll = tollPlazaLocationZ - vZ;
                    if (gapToToll < nearestLeadDist)
                    {
                        nearestLeadDist = gapToToll;
                        leadSpeedMps = 15f / 3.6f; // 15 km/h toll crawling speed
                    }
                }

                // Step Microscopic IDM physics
                vehicle.StepIDM(deltaTime, nearestLeadDist, leadSpeedMps);

                // Check Overtaking Decision (if vehicle is stuck behind slower lead)
                if (nearestLeadDist < 25.0f && vehicle.behaviorState == DriverBehaviorState.Following)
                {
                    int altLane = (vLane == 0) ? 1 : 0;
                    vehicle.laneAgent.EvaluateMOBILSafeLaneChange(altLane, frontGapMeters: 35.0f, rearGapMeters: 30.0f);
                }
            }

            // Stream & recycle vehicles around player
            spawner.UpdateStreamingCull(playerZ);
        }
    }
}
