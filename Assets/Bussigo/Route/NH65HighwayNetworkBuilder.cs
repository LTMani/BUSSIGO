using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bussigo.Route
{
    /// <summary>
    /// Builds the data-driven NH65 Flagship Corridor connecting Vijayawada to Hyderabad via Suryapet.
    /// Uses true physical segment lengths without arbitrary scaling multipliers.
    /// </summary>
    public static class NH65HighwayNetworkBuilder
    {
        public static RouteGraph BuildCorridorGraph()
        {
            var graph = new RouteGraph();

            // 1. Create Route Nodes
            var node0 = new RouteNode("NODE_VJA_PNBS", "Vijayawada PNBS Platform 4", RouteNodeType.TerminalOrigin, new Vector3(8.5f, 0f, 0f), 0f);
            var node1 = new RouteNode("NODE_VJA_EXIT", "Vijayawada City Exit Merge", RouteNodeType.JunctionMerge, new Vector3(0f, 0f, 4250f), 4250f);
            var node2 = new RouteNode("NODE_KCK_TOLL", "Kanchikacherla FASTag Toll Plaza", RouteNodeType.TollPlaza, new Vector3(0f, 0f, 32800f), 32800f);
            var node3 = new RouteNode("NODE_NDG_BYPASS", "Nandigama Highway Bypass", RouteNodeType.HighwayWaypoint, new Vector3(0f, 0f, 54200f), 54200f);
            var node4 = new RouteNode("NODE_KOD_INTER", "Kodad Cross Interchange", RouteNodeType.InterchangeExit, new Vector3(0f, 0f, 89600f), 89600f);
            var node5 = new RouteNode("NODE_SYP_HUB", "Suryapet 7-Hotel Food Hub", RouteNodeType.RestAreaFoodHub, new Vector3(18f, 0f, 136400f), 136400f);
            var node6 = new RouteNode("NODE_NKR_BYPASS", "Nakrekal Bypass Waypoint", RouteNodeType.HighwayWaypoint, new Vector3(0f, 0f, 178100f), 178100f);
            var node7 = new RouteNode("NODE_CHT_JUNCT", "Choutuppal Outer Junction", RouteNodeType.JunctionMerge, new Vector3(0f, 0f, 224500f), 224500f);
            var node8 = new RouteNode("NODE_HYD_ORR", "Hyderabad Outer Ring Road Interchange", RouteNodeType.InterchangeExit, new Vector3(0f, 0f, 256300f), 256300f);
            var node9 = new RouteNode("NODE_HYD_MGBS", "Hyderabad MGBS Platform 12", RouteNodeType.TerminalDestination, new Vector3(0f, 0f, 274850f), 274850f);

            graph.AddNode(node0);
            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.AddNode(node3);
            graph.AddNode(node4);
            graph.AddNode(node5);
            graph.AddNode(node6);
            graph.AddNode(node7);
            graph.AddNode(node8);
            graph.AddNode(node9);

            // 2. Create True-Distance Road Segments
            AddSegmentWithLanes(graph, "SEG_01", "NODE_VJA_PNBS", "NODE_VJA_EXIT", 4250f, 50f, RoadClassification.UrbanTerminalApproach);
            AddSegmentWithLanes(graph, "SEG_02", "NODE_VJA_EXIT", "NODE_KCK_TOLL", 28550f, 90f, RoadClassification.NationalHighway4Lane);
            AddSegmentWithLanes(graph, "SEG_03", "NODE_KCK_TOLL", "NODE_NDG_BYPASS", 21400f, 90f, RoadClassification.NationalHighway4Lane);
            AddSegmentWithLanes(graph, "SEG_04", "NODE_NDG_BYPASS", "NODE_KOD_INTER", 35400f, 90f, RoadClassification.NationalHighway4Lane);
            AddSegmentWithLanes(graph, "SEG_05", "NODE_KOD_INTER", "NODE_SYP_HUB", 46800f, 90f, RoadClassification.NationalHighway4Lane);
            AddSegmentWithLanes(graph, "SEG_06", "NODE_SYP_HUB", "NODE_NKR_BYPASS", 41700f, 90f, RoadClassification.NationalHighway4Lane);
            AddSegmentWithLanes(graph, "SEG_07", "NODE_NKR_BYPASS", "NODE_CHT_JUNCT", 46400f, 90f, RoadClassification.NationalHighway4Lane);
            AddSegmentWithLanes(graph, "SEG_08", "NODE_CHT_JUNCT", "NODE_HYD_ORR", 31800f, 90f, RoadClassification.NationalHighway4Lane);
            AddSegmentWithLanes(graph, "SEG_09", "NODE_HYD_ORR", "NODE_HYD_MGBS", 18550f, 60f, RoadClassification.UrbanTerminalApproach);

            return graph;
        }

        private static void AddSegmentWithLanes(RouteGraph graph, string id, string startNode, string endNode, float lengthM, float speedLimit, RoadClassification roadClass)
        {
            var segment = new RoadSegment(id, startNode, endNode, lengthM, speedLimit, roadClass);
            // Add 4 physical lanes (2 forward + 2 return)
            segment.lanes.Add(new LaneData($"{id}_L0_FWD", 0, 3.75f, LaneDirection.Forward, speedLimit));
            segment.lanes.Add(new LaneData($"{id}_L1_FWD", 1, 3.75f, LaneDirection.Forward, speedLimit));
            segment.lanes.Add(new LaneData($"{id}_L0_RET", 0, 3.75f, LaneDirection.Return, speedLimit));
            segment.lanes.Add(new LaneData($"{id}_L1_RET", 1, 3.75f, LaneDirection.Return, speedLimit));
            graph.AddSegment(segment);
        }
    }
}
