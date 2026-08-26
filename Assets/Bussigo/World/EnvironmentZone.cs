using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bussigo.World
{
    public enum EnvironmentZoneType
    {
        UrbanTerminalApproach = 0,
        AgriculturalBasin = 1,
        HighwayVillages = 2,
        RestAreaFoodHub = 3,
        OpenHighwayArid = 4,
        IndustrialCorridor = 5
    }

    [Serializable]
    public class EnvironmentZoneDefinition
    {
        public string zoneID;
        public string zoneName;
        public EnvironmentZoneType zoneType;
        public float startDistanceMeters;
        public float endDistanceMeters;
        public float treeDensityPerKm;
        public float buildingDensityPerKm;
        public float streetLightingDensityPerKm;
        public string ambientAudioTag;

        public EnvironmentZoneDefinition(string id, string name, EnvironmentZoneType type, float startM, float endM, float trees, float bldgs, float lights, string audio)
        {
            zoneID = id;
            zoneName = name;
            zoneType = type;
            startDistanceMeters = startM;
            endDistanceMeters = endM;
            treeDensityPerKm = trees;
            buildingDensityPerKm = bldgs;
            streetLightingDensityPerKm = lights;
            ambientAudioTag = audio;
        }
    }

    /// <summary>
    /// Data-driven registry of the 9 distinct environmental and geographical zones along the 274.85 km NH65 corridor.
    /// </summary>
    public static class EnvironmentZoneRegistry
    {
        public static List<EnvironmentZoneDefinition> GetNH65CorridorZones()
        {
            return new List<EnvironmentZoneDefinition>
            {
                new EnvironmentZoneDefinition("ZONE_01", "Vijayawada PNBS & Urban Exit", EnvironmentZoneType.UrbanTerminalApproach, 0f, 8000f, 15f, 45f, 30f, "Ambience_Urban_Traffic"),
                new EnvironmentZoneDefinition("ZONE_02", "Krishna River Agricultural Corridor", EnvironmentZoneType.AgriculturalBasin, 8000f, 45000f, 85f, 5f, 0f, "Ambience_Rural_Birds"),
                new EnvironmentZoneDefinition("ZONE_03", "Kanchikacherla & Border Villages", EnvironmentZoneType.HighwayVillages, 45000f, 95000f, 40f, 20f, 10f, "Ambience_Highway_Wind"),
                new EnvironmentZoneDefinition("ZONE_04", "Kodad Commercial Strip", EnvironmentZoneType.HighwayVillages, 95000f, 130000f, 30f, 25f, 15f, "Ambience_Highway_Wind"),
                new EnvironmentZoneDefinition("ZONE_05", "Suryapet 7-Hotel Food Hub", EnvironmentZoneType.RestAreaFoodHub, 130000f, 145000f, 25f, 35f, 25f, "Ambience_FoodHub_Crowd"),
                new EnvironmentZoneDefinition("ZONE_06", "Nakrekal Open Highway", EnvironmentZoneType.OpenHighwayArid, 145000f, 195000f, 20f, 4f, 0f, "Ambience_Highway_Wind"),
                new EnvironmentZoneDefinition("ZONE_07", "Choutuppal Industrial Corridor", EnvironmentZoneType.IndustrialCorridor, 195000f, 240000f, 25f, 22f, 12f, "Ambience_Industrial_Hum"),
                new EnvironmentZoneDefinition("ZONE_08", "Hyderabad ORR Outskirts", EnvironmentZoneType.HighwayVillages, 240000f, 265000f, 35f, 30f, 25f, "Ambience_Highway_Wind"),
                new EnvironmentZoneDefinition("ZONE_09", "Hyderabad MGBS Urban Approach", EnvironmentZoneType.UrbanTerminalApproach, 265000f, 274850f, 15f, 50f, 35f, "Ambience_City_Traffic")
            };
        }

        public static EnvironmentZoneDefinition FindZoneAtDistance(float distanceMeters)
        {
            var zones = GetNH65CorridorZones();
            foreach (var z in zones)
            {
                if (distanceMeters >= z.startDistanceMeters && distanceMeters <= z.endDistanceMeters)
                {
                    return z;
                }
            }
            return zones[0];
        }
    }
}
