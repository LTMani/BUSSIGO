using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter06
    {
        public int ChapterIndex => 6;
        public string ChapterTitleEnglish => "Chapter 06: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 06: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 06 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 125000;
        public int RewardDriverXp => 2500;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
