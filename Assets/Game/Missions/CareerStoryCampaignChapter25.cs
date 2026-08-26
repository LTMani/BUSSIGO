using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter25
    {
        public int ChapterIndex => 25;
        public string ChapterTitleEnglish => "Chapter 25: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 25: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 25 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 362500;
        public int RewardDriverXp => 7250;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
