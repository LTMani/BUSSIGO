using System;
using System.Collections.Generic;

namespace Bussigo.Game.Missions
{
    public class CareerMissionStoryDefinition01
    {
        public int MissionStoryId => 201;
        public string ChapterTitle => "Deccan Journey Story Mission 01";
        public string NarrativeStoryBrief => "Deliver passenger express service on Corridor Sector 01 with perfect punctuality.";
        public float TargetComfortScore => 86.5f;
        public float MaxAllowedSpeedLimitKmh => 80.0f;
        public long CompletionBonusCoins => 43500;
        public int CompletionBonusXp => 800;
    }
}
