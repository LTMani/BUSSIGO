using System;

namespace Bussigo.Game.Missions
{
    public class CareerStoryCampaignChapter21
    {
        public int ChapterIndex => 21;
        public string ChapterTitleEnglish => "Chapter 21: Dominating the Southern Corridors";
        public string ChapterTitleTelugu => "అధ్యాయం 21: దక్షిణ భారత రవాణా విజయం";
        public string MissionBriefing => "Operate scheduled express service on Sector 21 ensuring passenger satisfaction >= 90%.";
        public long RewardCoinsAmount => 312500;
        public int RewardDriverXp => 6250;
        public bool IsChapterCompleted { get; set; } = false;

        public bool EvaluateCompletionCriteria(float tripComfortScore, float delayMinutes, int passengersCarried)
        {
            return tripComfortScore >= 88.0f && delayMinutes <= 15.0f && passengersCarried >= 30;
        }
    }
}
