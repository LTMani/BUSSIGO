using System;
using Bussigo.Game.Core;
using Bussigo.Game.Navigation;
using Bussigo.Game.Routes;

namespace Bussigo.Tests.EditMode
{
    public static class RouteGraphNavigationTests
    {
        public static void RunAllTests()
        {
            TestAStarShortestPath();
            TestCorridorWaypointProgression();
        }

        public static void TestAStarShortestPath()
        {
            var graph = new RoadGraph();
            var n1 = graph.AddNode(1, "Vijayawada", new Vector3D(0f, 0f, 0f));
            var n2 = graph.AddNode(2, "Suryapet", new Vector3D(0f, 0f, 140000f));
            var n3 = graph.AddNode(3, "Hyderabad", new Vector3D(0f, 0f, 275000f));

            graph.AddEdge(101, 1, 2, 140000f);
            graph.AddEdge(102, 2, 3, 135000f);

            var pathfinder = new AStarPathfinder();
            var path = pathfinder.FindShortestPath(graph, 1, 3);

            if (path.Count != 3) throw new Exception($"A* path length expected 3 nodes, got {path.Count}.");
            if (path[0].Id != 1 || path[2].Id != 3) throw new Exception("A* path endpoints incorrect.");
        }

        public static void TestCorridorWaypointProgression()
        {
            var corridor = CorridorRegistry.VijayawadaToHyderabad;
            if (corridor.Waypoints.Count < 5) throw new Exception("Vijayawada-Hyderabad corridor missing required waypoints.");
            if (corridor.TotalDistanceKm < 250f || corridor.TotalDistanceKm > 300f)
                throw new Exception("Vijayawada-Hyderabad distance outside expected 250-300km bounds.");
        }
    }
}
