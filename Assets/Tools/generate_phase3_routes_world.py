#!/usr/bin/env python3
"""
BUSSIGO Engine Codebase Generator - Phase 3: Routes, Navigation & World Subsystems
Generates production-grade C# code files for:
- Assets/Game/Routes/
- Assets/Game/Navigation/
- Assets/Game/World/
"""

import os
from pathlib import Path

ROUTES_DIR = Path("Assets/Game/Routes")
NAV_DIR = Path("Assets/Game/Navigation")
WORLD_DIR = Path("Assets/Game/World")

ROUTES_DIR.mkdir(parents=True, exist_ok=True)
NAV_DIR.mkdir(parents=True, exist_ok=True)
WORLD_DIR.mkdir(parents=True, exist_ok=True)

FILES = {}

# -----------------------------------------------------------------------------
# ROUTES SUBSYSTEM
# -----------------------------------------------------------------------------

FILES[ROUTES_DIR / "CorridorRegistry.cs"] = """using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Routes
{
    public enum RoadClass
    {
        Expressway6Lane,     // NH65 modern expressway bypasses
        NationalHighway4Lane, // NH16 Vijayawada-Guntur, NH65 dual carriageway
        StateHighway2Lane,    // AP/Telangana State Highway
        RuralSingleLane,      // Village feeder connecting roads
        GhatRoadMountainPass  // Steep hairpin ghat sections
    }

    public class RouteWaypoint
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public GeoCoordinate Coordinate { get; set; }
        public float SpeedLimitKmh { get; set; } = 80.0f;
        public RoadClass HighwayClass { get; set; } = RoadClass.NationalHighway4Lane;
        public bool IsStopPoint { get; set; } = false;
        public float DwellTimeMinutes { get; set; } = 0.0f;

        public RouteWaypoint(string id, string name, double lat, double lon, double elev, float speedLimit = 80f, bool isStop = false)
        {
            Id = id;
            Name = name;
            Coordinate = new GeoCoordinate(lat, lon, elev);
            SpeedLimitKmh = speedLimit;
            IsStopPoint = isStop;
        }
    }

    public class HighwayCorridor
    {
        public string CorridorId { get; set; }
        public string OriginCity { get; set; }
        public string DestinationCity { get; set; }
        public float TotalDistanceKm { get; set; }
        public float EstimatedDurationHours { get; set; }
        public float TollFeesTotalInRupees { get; set; }
        public List<RouteWaypoint> Waypoints { get; } = new List<RouteWaypoint>();

        public HighwayCorridor(string id, string origin, string dest, float distKm, float durHours, float tollRs)
        {
            CorridorId = id;
            OriginCity = origin;
            DestinationCity = dest;
            TotalDistanceKm = distKm;
            EstimatedDurationHours = durHours;
            TollFeesTotalInRupees = tollRs;
        }

        public void AddWaypoint(RouteWaypoint wp) => Waypoints.Add(wp);
    }

    public static class CorridorRegistry
    {
        public static HighwayCorridor VijayawadaToHyderabad { get; private set; }
        public static HighwayCorridor VijayawadaToGuntur { get; private set; }
        public static HighwayCorridor HyderabadToWarangal { get; private set; }
        public static HighwayCorridor SrisailamGhatCorridor { get; private set; }

        static CorridorRegistry()
        {
            InitializeCorridors();
        }

        private static void InitializeCorridors()
        {
            // 1. Flagship Route: Vijayawada (PNBS) to Hyderabad (MGBS) via NH65 (275 km)
            VijayawadaToHyderabad = new HighwayCorridor("COR-VJA-HYD-01", "Vijayawada", "Hyderabad", 274.5f, 4.75f, 385.0f);
            VijayawadaToHyderabad.AddWaypoint(new RouteWaypoint("WP-VJA-PNBS", "Vijayawada PNBS Terminal", 16.5186, 80.6198, 22.0, 30f, true));
            VijayawadaToHyderabad.AddWaypoint(new RouteWaypoint("WP-VJA-IBRA", "Ibrahimpatnam Ring Road", 16.5880, 80.5210, 28.0, 60f));
            VijayawadaToHyderabad.AddWaypoint(new RouteWaypoint("WP-NH65-KANCH", "Kanchikacherla Toll Plaza", 16.6850, 80.3800, 35.0, 50f));
            VijayawadaToHyderabad.AddWaypoint(new RouteWaypoint("WP-NH65-NAND", "Nandigama Highway Stop", 16.7820, 80.2910, 42.0, 70f, true));
            VijayawadaToHyderabad.AddWaypoint(new RouteWaypoint("WP-NH65-KODAD", "Kodad Border Rest Area", 16.9980, 79.9650, 78.0, 80f));
            VijayawadaToHyderabad.AddWaypoint(new RouteWaypoint("WP-NH65-SURY", "Suryapet 7-Hotel Food Plaza", 17.1420, 79.6230, 165.0, 60f, true));
            VijayawadaToHyderabad.AddWaypoint(new RouteWaypoint("WP-NH65-NAKRE", "Nakrekal Junction", 17.1700, 79.4300, 182.0, 80f));
            VijayawadaToHyderabad.AddWaypoint(new RouteWaypoint("WP-NH65-CHOUT", "Choutuppal Toll Plaza", 17.2450, 78.9020, 310.0, 50f));
            VijayawadaToHyderabad.AddWaypoint(new RouteWaypoint("WP-HYD-LB", "LB Nagar Ring Road Hub", 17.3450, 78.5520, 490.0, 50f, true));
            VijayawadaToHyderabad.AddWaypoint(new RouteWaypoint("WP-HYD-MGBS", "Hyderabad MGBS Imlibun Terminal", 17.3780, 78.4820, 505.0, 30f, true));

            // 2. Twin City Corridor: Vijayawada to Guntur via NH16 (36 km)
            VijayawadaToGuntur = new HighwayCorridor("COR-VJA-GNT-02", "Vijayawada", "Guntur", 36.2f, 0.85f, 85.0f);
            VijayawadaToGuntur.AddWaypoint(new RouteWaypoint("WP-VJA-BENZ", "Vijayawada Benz Circle", 16.5010, 80.6520, 20.0, 40f, true));
            VijayawadaToGuntur.AddWaypoint(new RouteWaypoint("WP-NH16-PRAK", "Prakasam Barrage Krishna Bridge", 16.5090, 80.6050, 25.0, 45f));
            VijayawadaToGuntur.AddWaypoint(new RouteWaypoint("WP-NH16-MANG", "Mangalagiri Bypass Stop", 16.4320, 80.5610, 29.0, 80f, true));
            VijayawadaToGuntur.AddWaypoint(new RouteWaypoint("WP-NH16-KAZA", "Kaza Toll Plaza", 16.3850, 80.5200, 31.0, 50f));
            VijayawadaToGuntur.AddWaypoint(new RouteWaypoint("WP-GNT-AUTO", "Guntur Auto Nagar", 16.3210, 80.4650, 33.0, 50f));
            VijayawadaToGuntur.AddWaypoint(new RouteWaypoint("WP-GNT-NTR", "Guntur NTR Bus Station", 16.2980, 80.4420, 35.0, 30f, true));

            // 3. Telangana Heritage Corridor: Hyderabad to Warangal via NH163 (148 km)
            HyderabadToWarangal = new HighwayCorridor("COR-HYD-WGL-03", "Hyderabad", "Warangal", 148.0f, 2.75f, 160.0f);
            HyderabadToWarangal.AddWaypoint(new RouteWaypoint("WP-HYD-JBS", "Secunderabad Jubilee Bus Station (JBS)", 17.4520, 78.4980, 530.0, 30f, true));
            HyderabadToWarangal.AddWaypoint(new RouteWaypoint("WP-NH163-GHAT", "Ghatkesar Outer Ring Road", 17.4510, 78.6820, 480.0, 80f));
            HyderabadToWarangal.AddWaypoint(new RouteWaypoint("WP-NH163-BHONG", "Bhongir Fort Viewpoint", 17.5120, 78.8890, 430.0, 80f, true));
            HyderabadToWarangal.AddWaypoint(new RouteWaypoint("WP-NH163-ALER", "Aler Toll Plaza", 17.6520, 79.0510, 390.0, 50f));
            HyderabadToWarangal.AddWaypoint(new RouteWaypoint("WP-NH163-JAN", "Jangaon Highway Stop", 17.7210, 79.1820, 375.0, 60f, true));
            HyderabadToWarangal.AddWaypoint(new RouteWaypoint("WP-WGL-KAZI", "Kazipet Junction", 17.9780, 79.5200, 320.0, 50f));
            HyderabadToWarangal.AddWaypoint(new RouteWaypoint("WP-WGL-MAIN", "Warangal Central Bus Stand", 17.9950, 79.5850, 302.0, 30f, true));

            // 4. Eastern Ghats Mountain Pass: Srisailam Ghat Corridor (85 km)
            SrisailamGhatCorridor = new HighwayCorridor("COR-GHAT-SRI-04", "Dornala", "Srisailam", 85.0f, 2.50f, 60.0f);
            SrisailamGhatCorridor.AddWaypoint(new RouteWaypoint("WP-GHAT-DOR", "Dornala Forest Checkpost", 15.9010, 79.1020, 240.0, 50f, true));
            SrisailamGhatCorridor.AddWaypoint(new RouteWaypoint("WP-GHAT-HP01", "Hairpin Bend 1 (Tiger Valley)", 15.9450, 79.0520, 380.0, 30f));
            SrisailamGhatCorridor.AddWaypoint(new RouteWaypoint("WP-GHAT-HP06", "Hairpin Bend 6 (Sikharam View)", 16.0120, 78.9320, 590.0, 25f));
            SrisailamGhatCorridor.AddWaypoint(new RouteWaypoint("WP-GHAT-HP12", "Hairpin Bend 12 (Krishna Gorge)", 16.0520, 78.8920, 480.0, 25f));
            SrisailamGhatCorridor.AddWaypoint(new RouteWaypoint("WP-GHAT-SRI", "Srisailam Temple Terminal", 16.0750, 78.8680, 475.0, 30f, true));
        }
    }
}
"""

FILES[ROUTES_DIR / "TimetableSchedule.cs"] = """using System;
using System.Collections.Generic;

namespace Bussigo.Game.Routes
{
    public enum ServiceTier
    {
        Pallevelugu,   // Rural ordinary service, frequent village stops
        Express,       // Intercity fast passenger
        UltraDeluxe,   // Non-stop pushback
        SuperLuxury,   // Air suspension express
        GarudaAC,      // Multi-axle Volvo/Scania AC
        VennelaSleeper // Overnight luxury sleeper
    }

    public class ScheduledTrip
    {
        public string TripCode { get; set; }
        public string CorridorId { get; set; }
        public ServiceTier Tier { get; set; }
        public float DepartureHour { get; set; } // 0.0 to 24.0
        public float ArrivalHour { get; set; }
        public float BaseFarePerSeatRupees { get; set; }
        public int TotalSeatsBooked { get; set; }

        public ScheduledTrip(string code, string corridorId, ServiceTier tier, float depHour, float arrHour, float baseFare)
        {
            TripCode = code;
            CorridorId = corridorId;
            Tier = tier;
            DepartureHour = depHour;
            ArrivalHour = arrHour;
            BaseFarePerSeatRupees = baseFare;
        }
    }

    public class TimetableSchedule
    {
        public List<ScheduledTrip> DailyTrips { get; } = new List<ScheduledTrip>();

        public void PopulateDefaultSchedules()
        {
            DailyTrips.Clear();
            // Morning Express VJA -> HYD
            DailyTrips.Add(new ScheduledTrip("TRIP-101", "COR-VJA-HYD-01", ServiceTier.SuperLuxury, 6.0f, 10.75f, 480.0f));
            DailyTrips.Add(new ScheduledTrip("TRIP-102", "COR-VJA-HYD-01", ServiceTier.GarudaAC, 7.30f, 12.0f, 650.0f));
            DailyTrips.Add(new ScheduledTrip("TRIP-103", "COR-VJA-HYD-01", ServiceTier.Express, 9.0f, 14.0f, 320.0f));

            // Afternoon & Evening
            DailyTrips.Add(new ScheduledTrip("TRIP-104", "COR-VJA-HYD-01", ServiceTier.UltraDeluxe, 13.0f, 17.75f, 420.0f));
            DailyTrips.Add(new ScheduledTrip("TRIP-105", "COR-VJA-HYD-01", ServiceTier.GarudaAC, 17.0f, 21.5f, 650.0f));
            
            // Night Sleeper Flagship
            DailyTrips.Add(new ScheduledTrip("TRIP-106", "COR-VJA-HYD-01", ServiceTier.VennelaSleeper, 22.30f, 4.0f, 850.0f));
            DailyTrips.Add(new ScheduledTrip("TRIP-107", "COR-VJA-HYD-01", ServiceTier.VennelaSleeper, 23.15f, 4.45f, 850.0f));

            // Frequent Shuttle: Vijayawada <-> Guntur (Every 30 mins)
            for (int h = 6; h <= 21; h++)
            {
                DailyTrips.Add(new ScheduledTrip($"TRIP-GNT-{h}00", "COR-VJA-GNT-02", ServiceTier.Express, h, h + 0.85f, 45.0f));
                DailyTrips.Add(new ScheduledTrip($"TRIP-GNT-{h}30", "COR-VJA-GNT-02", ServiceTier.Pallevelugu, h + 0.5f, h + 1.45f, 30.0f));
            }
        }
    }
}
"""

# -----------------------------------------------------------------------------
# NAVIGATION SUBSYSTEM
# -----------------------------------------------------------------------------

FILES[NAV_DIR / "RoadGraph.cs"] = """using System;
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
"""

FILES[NAV_DIR / "AStarPathfinder.cs"] = """using System;
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
"""

FILES[NAV_DIR / "TurnByTurnNavigation.cs"] = """using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Navigation
{
    public enum NavigationManeuver
    {
        Straight,
        TurnSlightLeft,
        TurnLeft,
        TurnSharpLeft,
        TurnSlightRight,
        TurnRight,
        TurnSharpRight,
        UTurn,
        EnterRoundabout,
        TollPlazaAhead,
        ArriveAtDestination
    }

    public class NavigationInstruction
    {
        public NavigationManeuver Maneuver { get; set; }
        public string TextDescriptionEnglish { get; set; }
        public string TextDescriptionTelugu { get; set; }
        public float DistanceToManeuverMeters { get; set; }
        public float SpeedLimitKmh { get; set; }

        public string FormattedVoicePrompt => $"In {DistanceToManeuverMeters:F0} meters, {TextDescriptionEnglish}";
    }

    public class TurnByTurnNavigation
    {
        public List<RoadNode> ActiveRoutePath { get; private set; } = new List<RoadNode>();
        public int CurrentTargetWaypointIndex { get; private set; } = 0;
        public NavigationInstruction CurrentInstruction { get; private set; } = new NavigationInstruction();

        public float TotalDistanceRemainingKm { get; private set; }
        public float EstimatedTimeToArrivalMinutes { get; private set; }

        public void SetRoute(List<RoadNode> path)
        {
            ActiveRoutePath = path ?? new List<RoadNode>();
            CurrentTargetWaypointIndex = 0;
            RecalculateRemainingDistance(Vector3D.Zero);
        }

        public void UpdateGPS(Vector3D busPosition, float busSpeedKmh)
        {
            if (ActiveRoutePath.Count == 0 || CurrentTargetWaypointIndex >= ActiveRoutePath.Count)
            {
                CurrentInstruction = new NavigationInstruction
                {
                    Maneuver = NavigationManeuver.ArriveAtDestination,
                    TextDescriptionEnglish = "You have arrived at your destination terminal.",
                    TextDescriptionTelugu = "మీరు గమ్యస్థాన బస్ స్టేషన్ చేరుకున్నారు."
                };
                return;
            }

            RoadNode targetNode = ActiveRoutePath[CurrentTargetWaypointIndex];
            float distToNode = Vector3D.Distance(busPosition, targetNode.Position);

            if (distToNode < 25.0f && CurrentTargetWaypointIndex < ActiveRoutePath.Count - 1)
            {
                CurrentTargetWaypointIndex++;
                targetNode = ActiveRoutePath[CurrentTargetWaypointIndex];
                distToNode = Vector3D.Distance(busPosition, targetNode.Position);
            }

            RecalculateRemainingDistance(busPosition);

            float speedMps = MathF.Max(busSpeedKmh * CoreMath.KmhToMps, 1.0f);
            EstimatedTimeToArrivalMinutes = (TotalDistanceRemainingKm * 1000.0f / speedMps) / 60.0f;

            CurrentInstruction = new NavigationInstruction
            {
                Maneuver = NavigationManeuver.Straight,
                TextDescriptionEnglish = $"Continue towards {targetNode.Name}",
                TextDescriptionTelugu = $"{targetNode.Name} వైపు కొనసాగండి",
                DistanceToManeuverMeters = distToNode,
                SpeedLimitKmh = 80.0f
            };
        }

        private void RecalculateRemainingDistance(Vector3D busPos)
        {
            float dist = 0.0f;
            if (CurrentTargetWaypointIndex < ActiveRoutePath.Count)
            {
                dist += Vector3D.Distance(busPos, ActiveRoutePath[CurrentTargetWaypointIndex].Position);
                for (int i = CurrentTargetWaypointIndex; i < ActiveRoutePath.Count - 1; i++)
                {
                    dist += Vector3D.Distance(ActiveRoutePath[i].Position, ActiveRoutePath[i + 1].Position);
                }
            }
            TotalDistanceRemainingKm = dist / 1000.0f;
        }
    }
}
"""

# -----------------------------------------------------------------------------
# WORLD SUBSYSTEM
# -----------------------------------------------------------------------------

FILES[WORLD_DIR / "TollPlazaController.cs"] = """using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.World
{
    public enum TollPaymentType
    {
        FASTagRFID,
        CashBoothManual,
        VIPExempt
    }

    public class TollLane
    {
        public int LaneNumber { get; set; }
        public TollPaymentType PaymentType { get; set; } = TollPaymentType.FASTagRFID;
        public bool BarrierArmRaised { get; set; } = false;
        public bool VehicleDetected { get; set; } = false;
        public float TollFeeInRupees { get; set; } = 110.0f;
    }

    public class TollPlazaController
    {
        public string PlazaName { get; set; } = "Kaza Toll Plaza (NH16)";
        public List<TollLane> Lanes { get; } = new List<TollLane>();
        public float StandardBusTollFee { get; set; } = 110.0f;

        public event Action<string, float> OnTollPaid;

        public TollPlazaController(string name, int numLanes = 6)
        {
            PlazaName = name;
            for (int i = 1; i <= numLanes; i++)
            {
                Lanes.Add(new TollLane
                {
                    LaneNumber = i,
                    PaymentType = (i <= 4) ? TollPaymentType.FASTagRFID : TollPaymentType.CashBoothManual,
                    TollFeeInRupees = StandardBusTollFee
                });
            }
        }

        public bool ProcessFASTagDeduction(int laneIndex, float fastagBalance)
        {
            if (laneIndex < 0 || laneIndex >= Lanes.Count) return false;
            var lane = Lanes[laneIndex];

            if (fastagBalance >= lane.TollFeeInRupees)
            {
                lane.BarrierArmRaised = true;
                OnTollPaid?.Invoke(PlazaName, lane.TollFeeInRupees);
                return true;
            }

            lane.BarrierArmRaised = false;
            return false;
        }
    }
}
"""

for fpath, content in FILES.items():
    with open(fpath, "w", encoding="utf-8") as f:
        f.write(content.strip() + "\n")
    print(f"Generated: {fpath}")

print("Phase 3 generation complete.")
