using System;

namespace Bussigo.Game.Progression
{
    public class TycoonEmpireAchievementSpec10
    {
        public string AchievementKey => "ACH_SOUTH_EMPIRE_10";
        public string TitleEnglish => "South Transport Master Badge 10";
        public string TitleTelugu => "రవాణా చక్రవర్తి పురస్కారం 10";
        public string Description => "Transport 500000 passengers across Andhra Pradesh and Telangana.";
        public bool IsUnlocked { get; set; } = false;
        public float Progress01 { get; set; } = 0.0f;
    }
}
