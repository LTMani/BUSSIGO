using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter02
    {
        public int ChapterIndex => 2;
        public string ChapterTitleEnglish => "Chapter 02: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 02: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 02 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 75000;
        public int RewardDriverXp => 1500;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
