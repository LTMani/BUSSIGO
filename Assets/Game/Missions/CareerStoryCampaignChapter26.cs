using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter26
    {
        public int ChapterIndex => 26;
        public string ChapterTitleEnglish => "Chapter 26: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 26: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 26 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 375000;
        public int RewardDriverXp => 7500;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
