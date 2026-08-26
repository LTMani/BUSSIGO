using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bussigo.Route
{
    [Serializable]
    public struct RouteMilestone
    {
        public string milestoneName;
        public float distanceFromOriginKm;
        public Vector3 worldPosition;
        public bool isTollPlaza;
        public bool isTerminal;
    }

    public class CorridorGraph : MonoBehaviour
    {
        public string corridorTitle = "NH65: Vijayawada to Hyderabad";
        public List<RouteMilestone> milestones = new List<RouteMilestone>();

        private void Awake()
        {
            InitializeNH65Milestones();
        }

        public void InitializeNH65Milestones()
        {
            milestones.Clear();
            milestones.Add(new RouteMilestone { milestoneName = "Vijayawada PNBS Platform 4", distanceFromOriginKm = 0f, worldPosition = new Vector3(8.5f, 0f, 0f), isTerminal = true });
            milestones.Add(new RouteMilestone { milestoneName = "Vijayawada City Bypass", distanceFromOriginKm = 8.5f, worldPosition = new Vector3(0f, 0f, 150f) });
            milestones.Add(new RouteMilestone { milestoneName = "Kanchikacherla FASTag Toll Plaza", distanceFromOriginKm = 32.0f, worldPosition = new Vector3(0f, 0f, 520f), isTollPlaza = true });
            milestones.Add(new RouteMilestone { milestoneName = "Nandigama Highway Junction", distanceFromOriginKm = 52.0f, worldPosition = new Vector3(0f, 0f, 850f) });
            milestones.Add(new RouteMilestone { milestoneName = "Kodad Bypass Waypoint", distanceFromOriginKm = 88.0f, worldPosition = new Vector3(0f, 0f, 1100f) });
            milestones.Add(new RouteMilestone { milestoneName = "Suryapet 7-Hotel Food Hub", distanceFromOriginKm = 135.0f, worldPosition = new Vector3(18f, 0f, 1300f) });
            milestones.Add(new RouteMilestone { milestoneName = "Hyderabad City Limits Outer Ring Road", distanceFromOriginKm = 245.0f, worldPosition = new Vector3(0f, 0f, 2400f) });
            milestones.Add(new RouteMilestone { milestoneName = "Hyderabad MGBS Platform 12", distanceFromOriginKm = 275.0f, worldPosition = new Vector3(0f, 0f, 2700f), isTerminal = true });
        }
    }
}
