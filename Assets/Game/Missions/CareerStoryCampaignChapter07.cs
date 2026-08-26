using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter07
    {
        public int ChapterIndex => 7;
        public string ChapterTitleEnglish => "Chapter 07: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 07: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 07 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 137500;
        public int RewardDriverXp => 2750;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
