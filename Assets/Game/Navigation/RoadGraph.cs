using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Navigation
{
    public class RoadNode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Vector3D Position { get; set; }
        public List<RoadEdge> OutgoingEdges { get; } = new List<RoadEdge>();

        public RoadNode(int id, string name, Vector3D pos)
        {
            Id = id;
            Name = name;
            Position = pos;
        }
    }

    public class RoadEdge
    {
        public int EdgeId { get; set; }
        public RoadNode FromNode { get; set; }
        public RoadNode ToNode { get; set; }
        public float LengthMeters { get; set; }
        public float SpeedLimitKmh { get; set; } = 80.0f;
        public int LaneCount { get; set; } = 2;
        public bool IsOneway { get; set; } = true;
        public float CurrentTrafficCongestionFactor { get; set; } = 1.0f; // 1.0 = Free flow, 2.5 = Jammed

        public float TravelCostSeconds => (LengthMeters / (SpeedLimitKmh * CoreMath.KmhToMps)) * CurrentTrafficCongestionFactor;

        public RoadEdge(int id, RoadNode from, RoadNode to, float lengthM, float speedLimit = 80f, int lanes = 2)
        {
            EdgeId = id;
            FromNode = from;
            ToNode = to;
            LengthMeters = lengthM;
            SpeedLimitKmh = speedLimit;
            LaneCount = lanes;
        }
    }

    public class RoadGraph
    {
        public Dictionary<int, RoadNode> Nodes { get; } = new Dictionary<int, RoadNode>();
        public List<RoadEdge> Edges { get; } = new List<RoadEdge>();

        public RoadNode AddNode(int id, string name, Vector3D pos)
        {
            var node = new RoadNode(id, name, pos);
            Nodes[id] = node;
            return node;
        }

        public RoadEdge AddEdge(int edgeId, int fromNodeId, int toNodeId, float lengthM, float speedLimit = 80f, int lanes = 2)
        {
            var from = Nodes[fromNodeId];
            var to = Nodes[toNodeId];
            var edge = new RoadEdge(edgeId, from, to, lengthM, speedLimit, lanes);
            from.OutgoingEdges.Add(edge);
            Edges.Add(edge);
            return edge;
        }
    }
}
