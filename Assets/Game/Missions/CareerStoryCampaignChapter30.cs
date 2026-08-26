using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter30
    {
        public int ChapterIndex => 30;
        public string ChapterTitleEnglish => "Chapter 30: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 30: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 30 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 425000;
        public int RewardDriverXp => 8500;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
