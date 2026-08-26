using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter09
    {
        public int ChapterIndex => 9;
        public string ChapterTitleEnglish => "Chapter 09: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 09: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 09 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 162500;
        public int RewardDriverXp => 3250;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
