#!/usr/bin/env python3
"""
BUSSIGO Full 70K+ Genuine Source Code Expansion Engine - Part 2: Routes, Navigation, Traffic, Passengers, Fleet & Customization
Generates production-grade C# code files across:
- Assets/Game/Routes/
- Assets/Game/Navigation/
- Assets/Game/Traffic/
- Assets/Game/Passengers/
- Assets/Game/Fleet/
- Assets/Game/Garage/
- Assets/Game/Customization/
"""

import os
from pathlib import Path

def ensure_dir(path_str):
    p = Path(path_str)
    p.mkdir(parents=True, exist_ok=True)
    return p

ROUTES_DIR = ensure_dir("Assets/Game/Routes")
NAV_DIR = ensure_dir("Assets/Game/Navigation")
TRAFFIC_DIR = ensure_dir("Assets/Game/Traffic")
PASS_DIR = ensure_dir("Assets/Game/Passengers")
FLEET_DIR = ensure_dir("Assets/Game/Fleet")
GARAGE_DIR = ensure_dir("Assets/Game/Garage")
CUSTOM_DIR = ensure_dir("Assets/Game/Customization")

FILES = {}

# -----------------------------------------------------------------------------
# SOUTH INDIA CORRIDORS ATLAS (Andhra Pradesh & Telangana Network)
# -----------------------------------------------------------------------------

FILES[ROUTES_DIR / "SouthIndiaCorridorAtlas.cs"] = """using System;
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
"""

FILES[TRAFFIC_DIR / "IndianTrafficVehicleBehaviors.cs"] = """using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Traffic
{
    public enum IndianVehicleType
    {
        MultiAxleHeavyLorry, // Tata Prima / Ashok Leyland 14-wheeler
        StateRTCBus,          // Express / Pallevelugu RTC coach
        AutoRickshaw3Wheeler, // Bajaj RE / Piaggio Ape
        Motorcycle2Wheeler,   // Hero Splendor / Honda Activa
        PassengerCarHatchback,// Maruti Swift / Hyundai i20
        HighwaySUV,           // Mahindra Scorpio / Toyota Innova
        EmergencyAmbulance108 // 108 Emergency GVK Ambulance
    }

    public class IndianTrafficVehicleProfile
    {
        public IndianVehicleType VehicleType { get; set; }
        public string ModelName { get; set; }
        public float LengthMeters { get; set; }
        public float WidthMeters { get; set; }
        public float MaxSpeedKmh { get; set; }
        public float AccelerationCapabilityMps2 { get; set; }
        public float HornLikelihood01 { get; set; }
        public float LaneDiscipline01 { get; set; } // 1.0 = strict lane following, 0.3 = aggressive weaving
        public bool SoundHornAtOvertake { get; set; } = true;

        public static IndianTrafficVehicleProfile CreateDefault(IndianVehicleType type)
        {
            switch (type)
            {
                case IndianVehicleType.MultiAxleHeavyLorry:
                    return new IndianTrafficVehicleProfile
                    {
                        VehicleType = type,
                        ModelName = "Tata 1618 Cargo Lorry",
                        LengthMeters = 9.8f,
                        WidthMeters = 2.5f,
                        MaxSpeedKmh = 65.0f,
                        AccelerationCapabilityMps2 = 0.65f,
                        HornLikelihood01 = 0.85f,
                        LaneDiscipline01 = 0.45f
                    };
                case IndianVehicleType.AutoRickshaw3Wheeler:
                    return new IndianTrafficVehicleProfile
                    {
                        VehicleType = type,
                        ModelName = "Bajaj Compact 3-Wheeler",
                        LengthMeters = 2.7f,
                        WidthMeters = 1.3f,
                        MaxSpeedKmh = 50.0f,
                        AccelerationCapabilityMps2 = 1.2f,
                        HornLikelihood01 = 0.90f,
                        LaneDiscipline01 = 0.25f // Often hugs road shoulder or cuts across
                    };
                case IndianVehicleType.Motorcycle2Wheeler:
                    return new IndianTrafficVehicleProfile
                    {
                        VehicleType = type,
                        ModelName = "125cc Commuter Bike",
                        LengthMeters = 2.0f,
                        WidthMeters = 0.8f,
                        MaxSpeedKmh = 75.0f,
                        AccelerationCapabilityMps2 = 2.2f,
                        HornLikelihood01 = 0.70f,
                        LaneDiscipline01 = 0.20f // Filters between traffic lanes
                    };
                case IndianVehicleType.StateRTCBus:
                    return new IndianTrafficVehicleProfile
                    {
                        VehicleType = type,
                        ModelName = "Ashok Leyland Viking RTC",
                        LengthMeters = 11.5f,
                        WidthMeters = 2.6f,
                        MaxSpeedKmh = 85.0f,
                        AccelerationCapabilityMps2 = 1.1f,
                        HornLikelihood01 = 0.95f,
                        LaneDiscipline01 = 0.60f
                    };
                case IndianVehicleType.EmergencyAmbulance108:
                    return new IndianTrafficVehicleProfile
                    {
                        VehicleType = type,
                        ModelName = "108 Force Emergency Ambulance",
                        LengthMeters = 5.4f,
                        WidthMeters = 2.0f,
                        MaxSpeedKmh = 110.0f,
                        AccelerationCapabilityMps2 = 2.5f,
                        HornLikelihood01 = 1.0f,
                        LaneDiscipline01 = 0.50f
                    };
                case IndianVehicleType.HighwaySUV:
                default:
                    return new IndianTrafficVehicleProfile
                    {
                        VehicleType = IndianVehicleType.HighwaySUV,
                        ModelName = "Highway Cruiser SUV",
                        LengthMeters = 4.8f,
                        WidthMeters = 1.9f,
                        MaxSpeedKmh = 120.0f,
                        AccelerationCapabilityMps2 = 2.4f,
                        HornLikelihood01 = 0.60f,
                        LaneDiscipline01 = 0.75f
                    };
            }
        }
    }
}
"""

FILES[PASS_DIR / "PassengerCrowdManager.cs"] = """using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Passengers
{
    public enum PassengerType
    {
        DailyCityCommuter,
        IntercityFamily,
        SeniorPilgrimToTirupati,
        CollegeStudent,
        BusinessExecutive,
        RuralFarmerFeeder
    }

    public class PassengerEntity
    {
        public string PassengerId { get; set; }
        public string Name { get; set; }
        public PassengerType Type { get; set; }
        public string OriginTerminal { get; set; }
        public string DestinationTerminal { get; set; }
        public int AssignedSeatNumber { get; set; }
        public float LuggageWeightKg { get; set; }
        public float TicketFarePaidRupees { get; set; }
        public float EmotionalComfortScore { get; set; } = 100.0f;
    }

    public class PassengerCrowdManager
    {
        public List<PassengerEntity> CurrentBusPassengers { get; } = new List<PassengerEntity>();
        public List<PassengerEntity> TerminalWaitingQueue { get; } = new List<PassengerEntity>();

        public int TotalPassengersTransportedLifetime { get; private set; } = 0;
        public float TotalFareCollectedLifetimeRupees { get; private set; } = 0.0f;

        public void GenerateTerminalCrowd(string terminalCode, int crowdSize)
        {
            TerminalWaitingQueue.Clear();
            var rnd = new Random(101);

            for (int i = 1; i <= crowdSize; i++)
            {
                var passenger = new PassengerEntity
                {
                    PassengerId = $"PAX-{terminalCode}-{i:D3}",
                    Name = $"Passenger {i}",
                    Type = (PassengerType)(i % 6),
                    OriginTerminal = terminalCode,
                    DestinationTerminal = "HYD",
                    AssignedSeatNumber = i,
                    LuggageWeightKg = 8.0f + (float)(rnd.NextDouble() * 22.0),
                    TicketFarePaidRupees = 450.0f
                };
                TerminalWaitingQueue.Add(passenger);
            }
        }

        public int BoardAllEligiblePassengers(int busMaxCapacity)
        {
            int boardedCount = 0;
            while (TerminalWaitingQueue.Count > 0 && CurrentBusPassengers.Count < busMaxCapacity)
            {
                var pax = TerminalWaitingQueue[0];
                TerminalWaitingQueue.RemoveAt(0);
                CurrentBusPassengers.Add(pax);
                boardedCount++;
                TotalPassengersTransportedLifetime++;
                TotalFareCollectedLifetimeRupees += pax.TicketFarePaidRupees;
            }
            return boardedCount;
        }

        public int AlightPassengersAtDestination(string currentStopCode)
        {
            int alightedCount = 0;
            for (int i = CurrentBusPassengers.Count - 1; i >= 0; i--)
            {
                if (CurrentBusPassengers[i].DestinationTerminal == currentStopCode)
                {
                    CurrentBusPassengers.RemoveAt(i);
                    alightedCount++;
                }
            }
            return alightedCount;
        }
    }
}
"""

FILES[CUSTOM_DIR / "AirHornSynthesizer.cs"] = """using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Customization
{
    public class AirHornMelodyChord
    {
        public float[] FrequenciesHz { get; set; }
        public float DurationSeconds { get; set; }

        public AirHornMelodyChord(float[] freqs, float duration)
        {
            FrequenciesHz = freqs;
            DurationSeconds = duration;
        }
    }

    public class AirHornMelodicPattern
    {
        public string MelodyName { get; set; }
        public List<AirHornMelodyChord> Chords { get; } = new List<AirHornMelodyChord>();

        public AirHornMelodicPattern(string name)
        {
            MelodyName = name;
        }

        public void AddChord(float[] freqs, float duration)
        {
            Chords.Add(new AirHornMelodyChord(freqs, duration));
        }
    }

    public static class AirHornCatalog
    {
        public static List<AirHornMelodicPattern> Melodies { get; } = new List<AirHornMelodicPattern>();

        static AirHornCatalog()
        {
            // 1. Classic Telugu Dual Tone Pressure Horn
            var dualTone = new AirHornMelodicPattern("Classic Deccan Dual Tone");
            dualTone.AddChord(new float[] { 349.23f, 440.0f }, 0.4f); // F4 + A4
            dualTone.AddChord(new float[] { 392.00f, 523.25f }, 0.6f); // G4 + C5
            Melodies.Add(dualTone);

            // 2. High-Deck Triple Trombone Highway Horn
            var tripleHorn = new AirHornMelodicPattern("Highway King Triple Trombone");
            tripleHorn.AddChord(new float[] { 311.13f, 370.0f, 466.16f }, 0.35f);
            tripleHorn.AddChord(new float[] { 349.23f, 415.3f, 523.25f }, 0.35f);
            tripleHorn.AddChord(new float[] { 392.00f, 466.16f, 587.33f }, 0.8f);
            Melodies.Add(tripleHorn);

            // 3. Iconic South Indian 5-Tone Musical Chime
            var chime = new AirHornMelodicPattern("South Indian Highway Symphony");
            chime.AddChord(new float[] { 261.63f }, 0.18f); // C4
            chime.AddChord(new float[] { 293.66f }, 0.18f); // D4
            chime.AddChord(new float[] { 329.63f }, 0.18f); // E4
            chime.AddChord(new float[] { 392.00f }, 0.18f); // G4
            chime.AddChord(new float[] { 523.25f }, 0.60f); // C5
            Melodies.Add(chime);
        }
    }
}
"""

FILES[GARAGE_DIR / "VehicleDynoTuningBench.cs"] = """using System;
using System.Collections.Generic;
using Bussigo.Game.Core;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.Garage
{
    public struct DynoDataPoint
    {
        public float EngineRpm;
        public float BrakeTorqueNm;
        public float Horsepower;
        public float FuelFlowGramsPerKwh;
    }

    public class VehicleDynoTuningBench
    {
        public List<DynoDataPoint> RunDynoSweep(VehicleChassisSpec spec)
        {
            var results = new List<DynoDataPoint>();
            float rpmStep = 100.0f;

            for (float rpm = spec.IdleRpm; rpm <= spec.MaxEngineRpm; rpm += rpmStep)
            {
                float baseTorque;
                if (rpm < spec.MaxTorqueRpmMin)
                {
                    baseTorque = CoreMath.Lerp(spec.MaxTorqueNm * 0.55f, spec.MaxTorqueNm, (rpm - spec.IdleRpm) / (spec.MaxTorqueRpmMin - spec.IdleRpm));
                }
                else if (rpm <= spec.MaxTorqueRpmMax)
                {
                    baseTorque = spec.MaxTorqueNm;
                }
                else
                {
                    baseTorque = CoreMath.Lerp(spec.MaxTorqueNm, spec.MaxTorqueNm * 0.60f, (rpm - spec.MaxTorqueRpmMax) / (spec.MaxEngineRpm - spec.MaxTorqueRpmMax));
                }

                // Power (kW) = (Torque (Nm) * RPM * 2*pi) / 60000
                float powerKw = (baseTorque * rpm * 2.0f * MathF.PI) / 60000.0f;
                float hp = powerKw * 1.34102f;

                // BSFC estimate (g/kWh)
                float bsfc = 195.0f + MathF.Abs(rpm - 1400.0f) * 0.045f;

                results.Add(new DynoDataPoint
                {
                    EngineRpm = rpm,
                    BrakeTorqueNm = baseTorque,
                    Horsepower = hp,
                    FuelFlowGramsPerKwh = bsfc
                });
            }

            return results;
        }
    }
}
"""

for fpath, content in FILES.items():
    with open(fpath, "w", encoding="utf-8") as f:
        f.write(content.strip() + "\n")
    print(f"Generated: {fpath}")

print("Expansion Part 2 complete.")
