using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter38
    {
        public int ChapterIndex => 38;
        public string ChapterTitleEnglish => "Chapter 38: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 38: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 38 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 525000;
        public int RewardDriverXp => 10500;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
