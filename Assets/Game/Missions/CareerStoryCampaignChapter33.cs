using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter33
    {
        public int ChapterIndex => 33;
        public string ChapterTitleEnglish => "Chapter 33: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 33: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 33 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 462500;
        public int RewardDriverXp => 9250;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
