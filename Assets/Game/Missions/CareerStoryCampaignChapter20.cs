using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter20
    {
        public int ChapterIndex => 20;
        public string ChapterTitleEnglish => "Chapter 20: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 20: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 20 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 300000;
        public int RewardDriverXp => 6000;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
