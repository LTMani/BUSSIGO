using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter08
    {
        public int ChapterIndex => 8;
        public string ChapterTitleEnglish => "Chapter 08: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 08: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 08 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 150000;
        public int RewardDriverXp => 3000;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
