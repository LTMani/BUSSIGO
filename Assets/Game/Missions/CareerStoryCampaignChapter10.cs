using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter10
    {
        public int ChapterIndex => 10;
        public string ChapterTitleEnglish => "Chapter 10: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 10: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 10 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 175000;
        public int RewardDriverXp => 3500;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
