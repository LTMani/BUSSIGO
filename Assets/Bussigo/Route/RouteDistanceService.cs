using System;
using UnityEngine;
using Bussigo.Core;

namespace Bussigo.Route
{
    /// <summary>
    /// Computes true physical highway distance in metres and kilometres without arbitrary scaling multipliers.
    /// </summary>
    public class RouteDistanceService : IService
    {
        public RouteGraph ActiveGraph { get; private set; }

        public void Initialize()
        {
            ServiceLocator.Register<RouteDistanceService>(this);
            Debug.Log("[BUSSIGO] RouteDistanceService initialized: true physical distance enabled.");
        }

        public void Shutdown()
        {
            ActiveGraph = null;
        }

        public void SetActiveGraph(RouteGraph graph)
        {
            ActiveGraph = graph;
        }

        public float GetTotalRouteDistanceMeters()
        {
            if (ActiveGraph == null) return 0f;
            return ActiveGraph.CalculateTotalPhysicalLengthMeters();
        }

        public float GetTotalRouteDistanceKm()
        {
            return GetTotalRouteDistanceMeters() / 1000.0f;
        }

        public float GetTraveledDistanceMeters(float playerZPosition)
        {
            if (ActiveGraph == null) return 0f;
            // Physical displacement in meters along the corridor
            return Mathf.Max(0f, playerZPosition);
        }

        public float GetTraveledDistanceKm(float playerZPosition)
        {
            return GetTraveledDistanceMeters(playerZPosition) / 1000.0f;
        }

        public float GetRemainingDistanceMeters(float playerZPosition)
        {
            float total = GetTotalRouteDistanceMeters();
            float traveled = GetTraveledDistanceMeters(playerZPosition);
            return Mathf.Max(0f, total - traveled);
        }

        public float GetRemainingDistanceKm(float playerZPosition)
        {
            return GetRemainingDistanceMeters(playerZPosition) / 1000.0f;
        }
    }
}
