using System;
using UnityEngine;
using Bussigo.Core;
using Bussigo.Route;

namespace Bussigo.Navigation
{
    [Serializable]
    public struct GPSNavigationTelemetry
    {
        public string currentLocationName;
        public string destinationName;
        public string activeRoadName;
        public float speedLimitKmh;
        public float physicalTraveledKm;
        public float physicalRemainingKm;
        public float estimatedTimeToArrivalMinutes;
    }

    /// <summary>
    /// GPS route guidance service computing turn-by-turn waypoints and telemetry directly from true physical distances.
    /// </summary>
    public class GPSRouteService : MonoBehaviour, IService
    {
        [NonSerialized]
        public RouteDistanceService distanceService;

        [NonSerialized]
        public RouteGraph activeGraph;

        public GPSNavigationTelemetry CurrentTelemetry { get; private set; }

        public void Initialize()
        {
            if (distanceService == null)
            {
                distanceService = new RouteDistanceService();
                distanceService.Initialize();
            }
            if (activeGraph == null)
            {
                activeGraph = NH65HighwayNetworkBuilder.BuildCorridorGraph();
                distanceService.SetActiveGraph(activeGraph);
            }
            ServiceLocator.Register<GPSRouteService>(this);
            Debug.Log("[BUSSIGO] GPSRouteService initialized with true-distance NH65 network.");
        }

        public void Shutdown()
        {
            activeGraph = null;
        }

        public void UpdateTelemetry(Vector3 playerPosition, float currentSpeedKmh)
        {
            if (activeGraph == null || distanceService == null) return;

            float playerZ = playerPosition.z;
            float traveledKm = distanceService.GetTraveledDistanceKm(playerZ);
            float remainingKm = distanceService.GetRemainingDistanceKm(playerZ);

            // Locate current node
            string currentNodeName = "NH65 Main Highway Corridor";
            float speedLimit = 90.0f;

            foreach (var kvp in activeGraph.nodes)
            {
                var node = kvp.Value;
                if (Mathf.Abs(playerZ - node.distanceFromOriginMeters) < 500.0f)
                {
                    currentNodeName = node.nodeName;
                    break;
                }
            }

            float safeSpeed = Mathf.Max(30.0f, currentSpeedKmh);
            float etaMinutes = (remainingKm / safeSpeed) * 60.0f;

            CurrentTelemetry = new GPSNavigationTelemetry
            {
                currentLocationName = currentNodeName,
                destinationName = "Hyderabad MGBS Platform 12",
                activeRoadName = "National Highway 65 (NH65)",
                speedLimitKmh = speedLimit,
                physicalTraveledKm = traveledKm,
                physicalRemainingKm = remainingKm,
                estimatedTimeToArrivalMinutes = etaMinutes
            };
        }
    }
}
