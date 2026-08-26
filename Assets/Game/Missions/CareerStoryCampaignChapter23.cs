using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter23
    {
        public int ChapterIndex => 23;
        public string ChapterTitleEnglish => "Chapter 23: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 23: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 23 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 337500;
        public int RewardDriverXp => 6750;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
