using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bussigo.World
{
    public enum InfrastructureItemType
    {
        KilometreStone = 0,
        OverheadHighwayGantry = 1,
        MetalBeamCrashBarrier = 2,
        StreetLightPole = 3,
        FASTagTollCanopy = 4
    }

    [Serializable]
    public struct RoadsidePlacement
    {
        public InfrastructureItemType itemType;
        public float distanceFromOriginMeters;
        public float lateralOffsetMeters;
        public string labelText;
    }

    /// <summary>
    /// Manages data-driven placement and distance-based activation of Indian highway roadside infrastructure.
    /// </summary>
    public class RoadsideInfrastructureManager : MonoBehaviour
    {
        public readonly List<RoadsidePlacement> placements = new List<RoadsidePlacement>();

        public void InitializeCorridorInfrastructure()
        {
            placements.Clear();

            // 1. Kilometre Stones every 5 km along NH65 (Total 275 km)
            for (int km = 0; km <= 275; km += 5)
            {
                float distM = km * 1000f;
                int remKm = 275 - km;
                placements.Add(new RoadsidePlacement
                {
                    itemType = InfrastructureItemType.KilometreStone,
                    distanceFromOriginMeters = distM,
                    lateralOffsetMeters = 8.5f, // Left shoulder verge
                    labelText = $"NH 65\nHYD {remKm}\nVJA {km}"
                });
            }

            // 2. Major Destination Overhead Gantries
            placements.Add(new RoadsidePlacement { itemType = InfrastructureItemType.OverheadHighwayGantry, distanceFromOriginMeters: 4000f, lateralOffsetMeters: 0f, labelText: "WELCOME TO NH65 - HYDERABAD 271 KM" });
            placements.Add(new RoadsidePlacement { itemType = InfrastructureItemType.OverheadHighwayGantry, distanceFromOriginMeters: 32000f, lateralOffsetMeters: 0f, labelText: "TOLL PLAZA 800M - FASTAG LANES" });
            placements.Add(new RoadsidePlacement { itemType = InfrastructureItemType.OverheadHighwayGantry, distanceFromOriginMeters: 135000f, lateralOffsetMeters: 0f, labelText: "SURYAPET REST AREA & FOOD HUB - 1.4 KM" });
            placements.Add(new RoadsidePlacement { itemType = InfrastructureItemType.OverheadHighwayGantry, distanceFromOriginMeters: 255000f, lateralOffsetMeters: 0f, labelText: "HYDERABAD OUTER RING ROAD (ORR) EXIT" });
        }
    }
}
