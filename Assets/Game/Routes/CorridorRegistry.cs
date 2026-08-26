using System;
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
