using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bussigo.Route
{
    public enum RouteNodeType
    {
        TerminalOrigin = 0,
        TerminalDestination = 1,
        HighwayWaypoint = 2,
        JunctionMerge = 3,
        InterchangeExit = 4,
        TollPlaza = 5,
        RestAreaFoodHub = 6
    }

    [Serializable]
    public class RouteNode
    {
        public string nodeID;
        public string nodeName;
        public RouteNodeType nodeType;
        public Vector3 worldPosition;
        public float distanceFromOriginMeters;
        public List<string> outgoingSegmentIDs = new List<string>();

        public RouteNode() { }

        public RouteNode(string id, string name, RouteNodeType type, Vector3 position, float distMeters)
        {
            nodeID = id;
            nodeName = name;
            nodeType = type;
            worldPosition = position;
            distanceFromOriginMeters = distMeters;
        }
    }

    [Serializable]
    public class RouteGraph
    {
        public Dictionary<string, RouteNode> nodes = new Dictionary<string, RouteNode>();
        public Dictionary<string, RoadSegment> segments = new Dictionary<string, RoadSegment>();
        public List<string> primaryRouteSegmentOrder = new List<string>();

        public void AddNode(RouteNode node)
        {
            nodes[node.nodeID] = node;
        }

        public void AddSegment(RoadSegment segment, bool isPrimaryCorridor = true)
        {
            segments[segment.segmentID] = segment;
            if (nodes.TryGetValue(segment.startNodeID, out RouteNode startNode))
            {
                if (!startNode.outgoingSegmentIDs.Contains(segment.segmentID))
                {
                    startNode.outgoingSegmentIDs.Add(segment.segmentID);
                }
            }
            if (isPrimaryCorridor && !primaryRouteSegmentOrder.Contains(segment.segmentID))
            {
                primaryRouteSegmentOrder.Add(segment.segmentID);
            }
        }

        public float CalculateTotalPhysicalLengthMeters()
        {
            float total = 0f;
            foreach (var segID in primaryRouteSegmentOrder)
            {
                if (segments.TryGetValue(segID, out RoadSegment seg))
                {
                    total += seg.lengthMeters;
                }
            }
            return total;
        }

        public float CalculateTotalPhysicalLengthKm()
        {
            return CalculateTotalPhysicalLengthMeters() / 1000.0f;
        }
    }
}
