using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter31
    {
        public int ChapterIndex => 31;
        public string ChapterTitleEnglish => "Chapter 31: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 31: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 31 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 437500;
        public int RewardDriverXp => 8750;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
