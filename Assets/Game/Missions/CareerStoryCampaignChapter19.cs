using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter19
    {
        public int ChapterIndex => 19;
        public string ChapterTitleEnglish => "Chapter 19: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 19: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 19 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 287500;
        public int RewardDriverXp => 5750;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
