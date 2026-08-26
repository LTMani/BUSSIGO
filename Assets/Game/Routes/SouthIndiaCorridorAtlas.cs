using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Routes
{
    public class HighwayCityNode
    {
        public string CityCode { get; set; }
        public string CityNameEnglish { get; set; }
        public string CityNameTelugu { get; set; }
        public GeoCoordinate Coordinate { get; set; }
        public bool HasMajorBusTerminal { get; set; }
        public int RegionalPopulation { get; set; }

        public HighwayCityNode(string code, string nameEn, string nameTe, double lat, double lon, double elev, bool hasTerminal, int pop)
        {
            CityCode = code;
            CityNameEnglish = nameEn;
            CityNameTelugu = nameTe;
            Coordinate = new GeoCoordinate(lat, lon, elev);
            HasMajorBusTerminal = hasTerminal;
            RegionalPopulation = pop;
        }
    }

    public static class SouthIndiaCorridorAtlas
    {
        public static Dictionary<string, HighwayCityNode> Cities { get; } = new Dictionary<string, HighwayCityNode>();
        public static List<HighwayCorridor> RegionalCorridors { get; } = new List<HighwayCorridor>();

        static SouthIndiaCorridorAtlas()
        {
            RegisterCities();
            RegisterCorridors();
        }

        private static void RegisterCities()
        {
            // Andhra Pradesh Hubs
            Cities["VJA"] = new HighwayCityNode("VJA", "Vijayawada", "విజయవాడ", 16.5062, 80.6480, 22.0, true, 1800000);
            Cities["GNT"] = new HighwayCityNode("GNT", "Guntur", "గుంటూరు", 16.3067, 80.4365, 33.0, true, 950000);
            Cities["VSKP"] = new HighwayCityNode("VSKP", "Visakhapatnam", "విశాఖపట్నం", 17.6868, 83.2185, 15.0, true, 2400000);
            Cities["RJY"] = new HighwayCityNode("RJY", "Rajahmundry", "రాజమండ్రి", 17.0005, 81.8040, 25.0, true, 600000);
            Cities["KKD"] = new HighwayCityNode("KKD", "Kakinada", "కాకినాడ", 16.9891, 82.2475, 10.0, true, 480000);
            Cities["ELR"] = new HighwayCityNode("ELR", "Eluru", "ఏలూరు", 16.7107, 81.0952, 22.0, true, 320000);
            Cities["ONG"] = new HighwayCityNode("ONG", "Ongole", "ఒంగోలు", 15.5057, 80.0499, 24.0, true, 310000);
            Cities["NLR"] = new HighwayCityNode("NLR", "Nellore", "నెల్లూరు", 14.4426, 79.9865, 19.0, true, 750000);
            Cities["TPT"] = new HighwayCityNode("TPT", "Tirupati", "తిరుపతి", 13.6288, 79.4192, 160.0, true, 650000);
            Cities["KNL"] = new HighwayCityNode("KNL", "Kurnool", "కర్నూలు", 15.8281, 78.0373, 274.0, true, 580000);
            Cities["KDP"] = new HighwayCityNode("KDP", "Kadapa", "కడప", 14.4673, 78.8242, 138.0, true, 450000);
            Cities["ATP"] = new HighwayCityNode("ATP", "Anantapur", "అనంతపురం", 14.6819, 77.6006, 335.0, true, 420000);

            // Telangana Hubs
            Cities["HYD"] = new HighwayCityNode("HYD", "Hyderabad", "హైదరాబాద్", 17.3850, 78.4867, 505.0, true, 10500000);
            Cities["WGL"] = new HighwayCityNode("WGL", "Warangal", "వరంగల్", 17.9689, 79.5941, 302.0, true, 920000);
            Cities["KHM"] = new HighwayCityNode("KHM", "Khammam", "ఖమ్మం", 17.2473, 80.1514, 112.0, true, 390000);
            Cities["NLG"] = new HighwayCityNode("NLG", "Nalgonda", "నల్గొండ", 17.0500, 79.2667, 215.0, true, 210000);
            Cities["KRM"] = new HighwayCityNode("KRM", "Karimnagar", "కరీంనగర్", 18.4386, 79.1288, 265.0, true, 410000);
            Cities["NZB"] = new HighwayCityNode("NZB", "Nizamabad", "నిజామాబాద్", 18.6725, 78.0941, 395.0, true, 380000);
            Cities["MBNR"] = new HighwayCityNode("MBNR", "Mahbubnagar", "మహబూబ్‌నగర్", 16.7488, 77.9944, 498.0, true, 270000);
        }

        private static void RegisterCorridors()
        {
            // NH16 Coastal Corridor: Vijayawada -> Visakhapatnam (350 km)
            var vjaVskp = new HighwayCorridor("COR-VJA-VSKP-05", "Vijayawada", "Visakhapatnam", 348.5f, 6.25f, 490.0f);
            vjaVskp.AddWaypoint(new RouteWaypoint("WP-VJA-PNBS", "Vijayawada PNBS", 16.5186, 80.6198, 22.0, 30f, true));
            vjaVskp.AddWaypoint(new RouteWaypoint("WP-NH16-ELR", "Eluru Bypass Hub", 16.7107, 81.0952, 22.0, 70f, true));
            vjaVskp.AddWaypoint(new RouteWaypoint("WP-NH16-TPG", "Tadepalligudem Junction", 16.8120, 81.5230, 24.0, 80f, true));
            vjaVskp.AddWaypoint(new RouteWaypoint("WP-NH16-RJY", "Rajahmundry Godavari Bridge", 17.0005, 81.8040, 25.0, 60f, true));
            vjaVskp.AddWaypoint(new RouteWaypoint("WP-NH16-ANNA", "Annavaram Highway Rest Area", 17.2810, 82.4050, 35.0, 80f, true));
            vjaVskp.AddWaypoint(new RouteWaypoint("WP-NH16-TUNI", "Tuni Toll Plaza", 17.3520, 82.5510, 28.0, 50f));
            vjaVskp.AddWaypoint(new RouteWaypoint("WP-NH16-ANA", "Anakapalle Steel City Hub", 17.6910, 83.0020, 30.0, 60f, true));
            vjaVskp.AddWaypoint(new RouteWaypoint("WP-VSKP-DWAR", "Visakhapatnam Dwaraka RTC Complex", 17.7280, 83.3050, 18.0, 30f, true));
            RegionalCorridors.Add(vjaVskp);

            // NH44 Rayalaseema Corridor: Hyderabad -> Kurnool -> Anantapur (360 km)
            var hydAtp = new HighwayCorridor("COR-HYD-ATP-06", "Hyderabad", "Anantapur", 362.0f, 5.50f, 440.0f);
            hydAtp.AddWaypoint(new RouteWaypoint("WP-HYD-MGBS", "Hyderabad MGBS", 17.3780, 78.4820, 505.0, 30f, true));
            hydAtp.AddWaypoint(new RouteWaypoint("WP-NH44-SHAD", "Shadnagar Toll Plaza", 17.0650, 78.2050, 545.0, 60f));
            hydAtp.AddWaypoint(new RouteWaypoint("WP-NH44-JAD", "Jadcherla Food Stop", 16.7620, 78.1420, 510.0, 80f, true));
            hydAtp.AddWaypoint(new RouteWaypoint("WP-NH44-PEBB", "Pebbair Krishna River Crossing", 16.2050, 77.9950, 310.0, 80f));
            hydAtp.AddWaypoint(new RouteWaypoint("WP-NH44-KNL", "Kurnool Central Bus Stand", 15.8281, 78.0373, 274.0, 50f, true));
            hydAtp.AddWaypoint(new RouteWaypoint("WP-NH44-DHON", "Dhone Toll Plaza", 15.4120, 77.8720, 380.0, 60f));
            hydAtp.AddWaypoint(new RouteWaypoint("WP-NH44-GOOTY", "Gooty Fort Junction", 15.1150, 77.6350, 345.0, 80f, true));
            hydAtp.AddWaypoint(new RouteWaypoint("WP-ATP-MAIN", "Anantapur RTC Complex", 14.6819, 77.6006, 335.0, 30f, true));
            RegionalCorridors.Add(hydAtp);

            // NH16 South Coastal: Guntur -> Ongole -> Nellore -> Chennai Boundary (280 km)
            var gntNlr = new HighwayCorridor("COR-GNT-NLR-07", "Guntur", "Nellore", 278.0f, 4.25f, 320.0f);
            gntNlr.AddWaypoint(new RouteWaypoint("WP-GNT-NTR", "Guntur NTR Bus Station", 16.2980, 80.4420, 35.0, 30f, true));
            gntNlr.AddWaypoint(new RouteWaypoint("WP-NH16-CHIL", "Chilakaluripet Highway Stop", 16.0890, 80.1650, 38.0, 70f, true));
            gntNlr.AddWaypoint(new RouteWaypoint("WP-NH16-ONG", "Ongole Bypass Terminal", 15.5057, 80.0499, 24.0, 60f, true));
            gntNlr.AddWaypoint(new RouteWaypoint("WP-NH16-KAV", "Kavali Highway Plaza", 14.9120, 79.9920, 22.0, 80f, true));
            gntNlr.AddWaypoint(new RouteWaypoint("WP-NLR-MAIN", "Nellore RTC Bus Stand", 14.4426, 79.9865, 19.0, 30f, true));
            RegionalCorridors.Add(gntNlr);
        }
    }
}
