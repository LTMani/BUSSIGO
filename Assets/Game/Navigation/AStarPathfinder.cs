using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Navigation
{
    public class AStarPathfinder
    {
        public List<RoadNode> FindShortestPath(RoadGraph graph, int startNodeId, int targetNodeId)
        {
            if (!graph.Nodes.ContainsKey(startNodeId) || !graph.Nodes.ContainsKey(targetNodeId))
                return new List<RoadNode>();

            var startNode = graph.Nodes[startNodeId];
            var targetNode = graph.Nodes[targetNodeId];

            var openSet = new HashSet<RoadNode> { startNode };
            var cameFrom = new Dictionary<RoadNode, RoadNode>();

            var gScore = new Dictionary<RoadNode, float>();
            var fScore = new Dictionary<RoadNode, float>();

            foreach (var node in graph.Nodes.Values)
            {
                gScore[node] = float.MaxValue;
                fScore[node] = float.MaxValue;
            }

            gScore[startNode] = 0.0f;
            fScore[startNode] = Vector3D.Distance(startNode.Position, targetNode.Position);

            while (openSet.Count > 0)
            {
                // Find node with lowest fScore
                RoadNode current = null;
                float minF = float.MaxValue;
                foreach (var node in openSet)
                {
                    if (fScore[node] < minF)
                    {
                        minF = fScore[node];
                        current = node;
                    }
                }

                if (current == targetNode)
                {
                    return ReconstructPath(cameFrom, current);
                }

                openSet.Remove(current);

                foreach (var edge in current.OutgoingEdges)
                {
                    var neighbor = edge.ToNode;
                    float tentativeG = gScore[current] + edge.TravelCostSeconds;

                    if (tentativeG < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeG;
                        fScore[neighbor] = tentativeG + Vector3D.Distance(neighbor.Position, targetNode.Position) * 0.05f;

                        openSet.Add(neighbor);
                    }
                }
            }

            return new List<RoadNode>(); // No path found
        }

        private List<RoadNode> ReconstructPath(Dictionary<RoadNode, RoadNode> cameFrom, RoadNode current)
        {
            var path = new List<RoadNode> { current };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Insert(0, current);
            }
            return path;
        }
    }
}
