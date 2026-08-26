using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter34
    {
        public int ChapterIndex => 34;
        public string ChapterTitleEnglish => "Chapter 34: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 34: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 34 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 475000;
        public int RewardDriverXp => 9500;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
