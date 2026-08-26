using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter27
    {
        public int ChapterIndex => 27;
        public string ChapterTitleEnglish => "Chapter 27: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 27: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 27 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 387500;
        public int RewardDriverXp => 7750;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
