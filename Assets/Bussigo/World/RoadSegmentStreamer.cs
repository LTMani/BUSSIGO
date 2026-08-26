using System;
using System.Collections.Generic;
using UnityEngine;
using Bussigo.Route;

namespace Bussigo.World
{
    /// <summary>
    /// Distance-based streaming and LOD manager for highway road segments.
    /// </summary>
    public class RoadSegmentStreamer : MonoBehaviour
    {
        public Transform playerTransform;
        public float activeStreamingRadiusMeters = 1200f;
        public float culledDistanceMeters = 2000f;

        private RouteGraph routeGraph;
        private readonly Dictionary<string, GameObject> activeSegmentGameObjects = new Dictionary<string, GameObject>();

        public void Initialize(RouteGraph graph)
        {
            routeGraph = graph;
        }

        private void Update()
        {
            if (playerTransform == null || routeGraph == null) return;

            float playerZ = playerTransform.position.z;

            // Stream / Cull segments based on player Z displacement
            foreach (var segKvp in routeGraph.segments)
            {
                string segId = segKvp.Key;
                RoadSegment seg = segKvp.Value;

                if (routeGraph.nodes.TryGetValue(seg.startNodeID, out RouteNode startNode))
                {
                    float segStartDist = startNode.distanceFromOriginMeters;
                    float distToPlayer = Mathf.Abs(playerZ - segStartDist);

                    if (distToPlayer <= activeStreamingRadiusMeters)
                    {
                        // In active view radius -> Ensure GameObject is active
                        if (activeSegmentGameObjects.TryGetValue(segId, out GameObject segGo) && segGo != null)
                        {
                            if (!segGo.activeSelf) segGo.SetActive(true);
                        }
                    }
                    else if (distToPlayer > culledDistanceMeters)
                    {
                        // Beyond culling radius -> Deactivate to free draw calls & physics overhead
                        if (activeSegmentGameObjects.TryGetValue(segId, out GameObject segGo) && segGo != null)
                        {
                            if (segGo.activeSelf) segGo.SetActive(false);
                        }
                    }
                }
            }
        }

        public void RegisterSegmentObject(string segId, GameObject segObj)
        {
            activeSegmentGameObjects[segId] = segObj;
        }
    }
}
