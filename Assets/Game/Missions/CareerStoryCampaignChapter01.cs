using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter01
    {
        public int ChapterIndex => 1;
        public string ChapterTitleEnglish => "Chapter 01: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 01: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 01 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 62500;
        public int RewardDriverXp => 1250;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
