using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter18
    {
        public int ChapterIndex => 18;
        public string ChapterTitleEnglish => "Chapter 18: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 18: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 18 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 275000;
        public int RewardDriverXp => 5500;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
