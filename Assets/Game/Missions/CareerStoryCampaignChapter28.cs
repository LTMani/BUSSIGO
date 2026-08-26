using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter28
    {
        public int ChapterIndex => 28;
        public string ChapterTitleEnglish => "Chapter 28: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 28: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 28 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 400000;
        public int RewardDriverXp => 8000;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
