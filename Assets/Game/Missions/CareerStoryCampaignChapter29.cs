using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter29
    {
        public int ChapterIndex => 29;
        public string ChapterTitleEnglish => "Chapter 29: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 29: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 29 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 412500;
        public int RewardDriverXp => 8250;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
