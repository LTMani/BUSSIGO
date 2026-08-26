using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter32
    {
        public int ChapterIndex => 32;
        public string ChapterTitleEnglish => "Chapter 32: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 32: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 32 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 450000;
        public int RewardDriverXp => 9000;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
