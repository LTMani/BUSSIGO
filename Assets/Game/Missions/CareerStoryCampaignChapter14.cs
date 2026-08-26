using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter14
    {
        public int ChapterIndex => 14;
        public string ChapterTitleEnglish => "Chapter 14: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 14: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 14 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 225000;
        public int RewardDriverXp => 4500;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
