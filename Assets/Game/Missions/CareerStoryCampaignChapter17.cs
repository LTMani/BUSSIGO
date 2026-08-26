using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter17
    {
        public int ChapterIndex => 17;
        public string ChapterTitleEnglish => "Chapter 17: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 17: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 17 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 262500;
        public int RewardDriverXp => 5250;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
