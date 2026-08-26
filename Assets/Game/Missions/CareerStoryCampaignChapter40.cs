using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter40
    {
        public int ChapterIndex => 40;
        public string ChapterTitleEnglish => "Chapter 40: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 40: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 40 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 550000;
        public int RewardDriverXp => 11000;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
