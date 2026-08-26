using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter16
    {
        public int ChapterIndex => 16;
        public string ChapterTitleEnglish => "Chapter 16: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 16: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 16 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 250000;
        public int RewardDriverXp => 5000;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
