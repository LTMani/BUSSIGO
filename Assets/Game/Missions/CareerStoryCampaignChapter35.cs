using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter35
    {
        public int ChapterIndex => 35;
        public string ChapterTitleEnglish => "Chapter 35: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 35: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 35 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 487500;
        public int RewardDriverXp => 9750;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
