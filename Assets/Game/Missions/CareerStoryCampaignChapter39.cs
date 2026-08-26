using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter39
    {
        public int ChapterIndex => 39;
        public string ChapterTitleEnglish => "Chapter 39: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 39: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 39 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 537500;
        public int RewardDriverXp => 10750;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
