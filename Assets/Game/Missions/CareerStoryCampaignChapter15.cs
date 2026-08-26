using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter15
    {
        public int ChapterIndex => 15;
        public string ChapterTitleEnglish => "Chapter 15: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 15: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 15 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 237500;
        public int RewardDriverXp => 4750;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
