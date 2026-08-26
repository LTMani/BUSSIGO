using System;
using System.Collections.Generic;
using UnityEngine;
using Bussigo.Route;

namespace Bussigo.Traffic
{
    public enum TrafficDensity
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    /// <summary>
    /// Spawns, despawns, and object-pools traffic vehicles along the active highway corridor.
    /// </summary>
    public class TrafficSpawner : MonoBehaviour
    {
        public TrafficDensity density = TrafficDensity.Medium;
        public int maxActiveVehicles => (density == TrafficDensity.Low ? 12 : (density == TrafficDensity.Medium ? 24 : 45));

        private readonly List<TrafficVehicleController> activeVehicles = new List<TrafficVehicleController>();
        private readonly Queue<TrafficVehicleController> vehiclePool = new Queue<TrafficVehicleController>();

        public IReadOnlyList<TrafficVehicleController> ActiveVehicles => activeVehicles;

        public void InitializePool(int poolSize = 50)
        {
            GameObject poolRoot = new GameObject("Traffic_Object_Pool");
            poolRoot.transform.SetParent(transform, false);

            for (int i = 0; i < poolSize; i++)
            {
                GameObject vGo = new GameObject($"Pooled_TrafficVehicle_{i}");
                vGo.transform.SetParent(poolRoot.transform, false);
                var ctrl = vGo.AddComponent<TrafficVehicleController>();
                vGo.SetActive(false);
                vehiclePool.Enqueue(ctrl);
            }
        }

        public TrafficVehicleController SpawnVehicle(VehicleCategory category, float spawnZ, float initialSpeedKmh)
        {
            TrafficVehicleController ctrl = null;
            if (vehiclePool.Count > 0)
            {
                ctrl = vehiclePool.Dequeue();
            }
            else
            {
                GameObject vGo = new GameObject("Dynamic_TrafficVehicle");
                ctrl = vGo.AddComponent<TrafficVehicleController>();
            }

            var profile = TrafficVehicleProfile.CreateDefault(category);
            ctrl.gameObject.SetActive(true);
            ctrl.Initialize(profile, initialSpeedKmh, spawnZ);
            activeVehicles.Add(ctrl);
            return ctrl;
        }

        public void DespawnVehicle(TrafficVehicleController ctrl)
        {
            if (ctrl == null) return;
            activeVehicles.Remove(ctrl);
            ctrl.gameObject.SetActive(false);
            vehiclePool.Enqueue(ctrl);
        }

        public void UpdateStreamingCull(float playerZPosition, float forwardSpawnDist = 800f, float rearDespawnDist = 250f)
        {
            for (int i = activeVehicles.Count - 1; i >= 0; i--)
            {
                var v = activeVehicles[i];
                float distFromPlayer = v.longitudinalDistanceMeters - playerZPosition;

                // Despawn if far behind player or far ahead of view distance
                if (distFromPlayer < -rearDespawnDist || distFromPlayer > forwardSpawnDist + 400f)
                {
                    DespawnVehicle(v);
                }
            }
        }
    }
}
