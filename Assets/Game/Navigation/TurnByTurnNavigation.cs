using System;
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
