using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter11
    {
        public int ChapterIndex => 11;
        public string ChapterTitleEnglish => "Chapter 11: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 11: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 11 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 187500;
        public int RewardDriverXp => 3750;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
