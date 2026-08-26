using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bussigo.Game.Runtime3D.Traffic
{
    public class UnityTrafficVehicleAI : MonoBehaviour
    {
        public enum VehicleType
        {
            TataHeavyTruck = 0,
            AutoRickshaw = 1,
            ExpressBus = 2,
            PassengerCar = 3
        }

        [Header("Vehicle Properties")]
        public VehicleType vehicleType = VehicleType.TataHeavyTruck;
        public float desiredSpeedKmh = 65f;
        public float currentSpeedKmh = 0f;
        public float maxAccelerationMs2 = 1.8f;
        public float comfortableDecelMs2 = 2.4f;
        public float safeFollowDistanceMeters = 12f;

        [Header("Waypoints & Path")]
        public List<Vector3> pathWaypoints;
        public int currentWaypointIndex = 0;
        public bool isLooping = true;

        [Header("Obstacle Detection")]
        public float forwardRaycastDistance = 28f;
        public LayerMask obstacleLayerMask = ~0;

        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
                rb.mass = 3500f;
                rb.isKinematic = true; // Use kinematic waypoint following for rock-solid traffic stability
            }
        }

        private void Update()
        {
            if (pathWaypoints == null || pathWaypoints.Count == 0) return;

            // 1. Raycast Obstacle & Leader Detection (IDM)
            float detectedLeaderDistance = forwardRaycastDistance;
            bool hasObstacleAhead = false;

            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 1.0f, transform.forward, out hit, forwardRaycastDistance, obstacleLayerMask))
            {
                if (hit.collider.gameObject != gameObject)
                {
                    detectedLeaderDistance = hit.distance;
                    hasObstacleAhead = true;
                }
            }

            // 2. IDM Speed Calculation
            float targetSpeedKmh = desiredSpeedKmh;
            if (hasObstacleAhead && detectedLeaderDistance < safeFollowDistanceMeters)
            {
                // Brake to prevent collision
                float brakeFactor = Mathf.Clamp01((detectedLeaderDistance - 4f) / (safeFollowDistanceMeters - 4f));
                targetSpeedKmh = desiredSpeedKmh * brakeFactor;
            }

            currentSpeedKmh = Mathf.MoveTowards(currentSpeedKmh, targetSpeedKmh, (targetSpeedKmh > currentSpeedKmh ? maxAccelerationMs2 : comfortableDecelMs2) * 3.6f * Time.deltaTime);

            // 3. Move towards current waypoint
            Vector3 targetWaypoint = pathWaypoints[currentWaypointIndex];
            Vector3 directionToWaypoint = (targetWaypoint - transform.position);
            directionToWaypoint.y = 0f;

            if (directionToWaypoint.magnitude < 5f)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= pathWaypoints.Count)
                {
                    if (isLooping) currentWaypointIndex = 0;
                    else currentWaypointIndex = pathWaypoints.Count - 1;
                }
            }

            if (directionToWaypoint.sqrMagnitude > 0.01f)
            {
                Quaternion targetRot = Quaternion.LookRotation(directionToWaypoint);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 4.0f);
            }

            float moveDistance = (currentSpeedKmh / 3.6f) * Time.deltaTime;
            transform.position += transform.forward * moveDistance;
        }
    }
}
