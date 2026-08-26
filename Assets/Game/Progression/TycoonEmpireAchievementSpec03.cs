using System;

namespace Bussigo.Game.Progression
{
    public class TycoonEmpireAchievementSpec03
    {
        public string AchievementKey => "ACH_SOUTH_EMPIRE_03";
        public string TitleEnglish => "South Transport Master Badge 03";
        public string TitleTelugu => "రవాణా చక్రవర్తి పురస్కారం 03";
        public string Description => "Transport 150000 passengers across Andhra Pradesh and Telangana.";
        public bool IsUnlocked { get; set; } = false;
        public float Progress01 { get; set; } = 0.0f;
    }
}
