using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bussigo.Route
{
    public enum LaneDirection
    {
        Forward = 0,
        Return = 1
    }

    [Serializable]
    public class LaneData
    {
        public string laneID;
        public int laneIndex; // 0 = Innermost/Overtaking lane, 1 = Cruising, 2 = Shoulder/Slow
        public float widthMeters = 3.75f;
        public LaneDirection direction = LaneDirection.Forward;
        public float speedLimitKmh = 90.0f;
        public bool canChangeLaneLeft = true;
        public bool canChangeLaneRight = true;
        public List<Vector3> centerlineWaypoints = new List<Vector3>();

        public LaneData() { }

        public LaneData(string id, int index, float width, LaneDirection dir, float speedLimit)
        {
            laneID = id;
            laneIndex = index;
            widthMeters = width;
            direction = dir;
            speedLimitKmh = speedLimit;
        }
    }
}
