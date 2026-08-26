using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter05
    {
        public int ChapterIndex => 5;
        public string ChapterTitleEnglish => "Chapter 05: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 05: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 05 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 112500;
        public int RewardDriverXp => 2250;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
