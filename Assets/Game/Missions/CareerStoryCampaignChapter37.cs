using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter37
    {
        public int ChapterIndex => 37;
        public string ChapterTitleEnglish => "Chapter 37: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 37: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 37 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 512500;
        public int RewardDriverXp => 10250;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
