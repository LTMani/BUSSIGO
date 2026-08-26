using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter13
    {
        public int ChapterIndex => 13;
        public string ChapterTitleEnglish => "Chapter 13: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 13: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 13 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 212500;
        public int RewardDriverXp => 4250;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
