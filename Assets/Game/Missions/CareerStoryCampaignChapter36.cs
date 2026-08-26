using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter36
    {
        public int ChapterIndex => 36;
        public string ChapterTitleEnglish => "Chapter 36: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 36: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 36 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 500000;
        public int RewardDriverXp => 10000;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
