using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter03
    {
        public int ChapterIndex => 3;
        public string ChapterTitleEnglish => "Chapter 03: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 03: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 03 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 87500;
        public int RewardDriverXp => 1750;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
