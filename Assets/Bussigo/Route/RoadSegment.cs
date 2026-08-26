using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bussigo.Route
{
    public enum RoadSurfaceType
    {
        AsphaltSmooth = 0,
        AsphaltWeathered = 1,
        ConcretePavement = 2,
        TollPlazaPavers = 3
    }

    public enum RoadClassification
    {
        NationalHighway4Lane = 0,
        Expressway6Lane = 1,
        UrbanTerminalApproach = 2,
        TollPlazaZone = 3,
        ServiceRoadRamp = 4
    }

    [Serializable]
    public class RoadSegment
    {
        [Header("Segment Identifiers & Topology")]
        public string segmentID;
        public string startNodeID;
        public string endNodeID;
        public string roadName = "NH65";

        [Header("Physical Geometry & Dimensions")]
        public float lengthMeters;
        public int laneCount = 4; // 2 forward + 2 return
        public float laneWidthMeters = 3.75f;
        public float shoulderWidthMeters = 2.5f;
        public float medianWidthMeters = 1.2f;
        public float speedLimitKmh = 90.0f;

        [Header("Curvature & Elevation")]
        public float curvatureDegrees = 0f;
        public float elevationChangeMeters = 0f;
        public float superelevationAngleDeg = 0f;
        public RoadClassification classification = RoadClassification.NationalHighway4Lane;
        public RoadSurfaceType surfaceType = RoadSurfaceType.AsphaltSmooth;

        [Header("Logical Lanes")]
        public List<LaneData> lanes = new List<LaneData>();

        public RoadSegment() { }

        public RoadSegment(string id, string startNode, string endNode, float lengthM, float speedLimit, RoadClassification roadClass)
        {
            segmentID = id;
            startNodeID = startNode;
            endNodeID = endNode;
            lengthMeters = lengthM;
            speedLimitKmh = speedLimit;
            classification = roadClass;
        }
    }
}
