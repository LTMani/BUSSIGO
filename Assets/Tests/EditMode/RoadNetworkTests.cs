using System;
using NUnit.Framework;
using UnityEngine;
using Bussigo.Route;
using Bussigo.Navigation;

namespace Bussigo.Tests.EditMode
{
    [TestFixture]
    public class RoadNetworkTests
    {
        [Test]
        public void NH65HighwayNetwork_BuildsTrueDistanceGraph()
        {
            RouteGraph graph = NH65HighwayNetworkBuilder.BuildCorridorGraph();

            Assert.IsNotNull(graph);
            Assert.AreEqual(10, graph.nodes.Count);
            Assert.AreEqual(9, graph.segments.Count);

            // 1. Verify Endpoints and Intermediate Waypoints
            Assert.IsTrue(graph.nodes.ContainsKey("NODE_VJA_PNBS"));
            Assert.IsTrue(graph.nodes.ContainsKey("NODE_SYP_HUB"));
            Assert.IsTrue(graph.nodes.ContainsKey("NODE_HYD_MGBS"));

            var originNode = graph.nodes["NODE_VJA_PNBS"];
            var destNode = graph.nodes["NODE_HYD_MGBS"];
            var sypNode = graph.nodes["NODE_SYP_HUB"];

            Assert.AreEqual(RouteNodeType.TerminalOrigin, originNode.nodeType);
            Assert.AreEqual(RouteNodeType.TerminalDestination, destNode.nodeType);
            Assert.AreEqual(RouteNodeType.RestAreaFoodHub, sypNode.nodeType);

            // 2. Verify Physical Length Sum
            float totalMeters = graph.CalculateTotalPhysicalLengthMeters();
            float totalKm = graph.CalculateTotalPhysicalLengthKm();

            Assert.AreEqual(274850f, totalMeters, 1.0f);
            Assert.AreEqual(274.85f, totalKm, 0.01f);

            // 3. Verify Every Segment has Positive Length and 4 Lanes
            foreach (var kvp in graph.segments)
            {
                var seg = kvp.Value;
                Assert.Greater(seg.lengthMeters, 0f, $"Segment {seg.segmentID} has invalid length");
                Assert.AreEqual(4, seg.lanes.Count, $"Segment {seg.segmentID} missing 4-lane configuration");
            }
        }

        [Test]
        public void RouteDistanceService_ComputesRealPhysicalDistance()
        {
            var service = new RouteDistanceService();
            service.Initialize();

            var graph = NH65HighwayNetworkBuilder.BuildCorridorGraph();
            service.SetActiveGraph(graph);

            Assert.AreEqual(274.85f, service.GetTotalRouteDistanceKm(), 0.01f);

            // At km 0 (Start)
            Assert.AreEqual(0f, service.GetTraveledDistanceKm(0f));
            Assert.AreEqual(274.85f, service.GetRemainingDistanceKm(0f), 0.01f);

            // At Suryapet (136,400m)
            float sypZ = 136400f;
            Assert.AreEqual(136.4f, service.GetTraveledDistanceKm(sypZ), 0.01f);
            Assert.AreEqual(138.45f, service.GetRemainingDistanceKm(sypZ), 0.01f);

            // At Destination (274,850m)
            float destZ = 274850f;
            Assert.AreEqual(274.85f, service.GetTraveledDistanceKm(destZ), 0.01f);
            Assert.AreEqual(0f, service.GetRemainingDistanceKm(destZ), 0.01f);
        }
    }
}
