using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter12
    {
        public int ChapterIndex => 12;
        public string ChapterTitleEnglish => "Chapter 12: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 12: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 12 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 200000;
        public int RewardDriverXp => 4000;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
