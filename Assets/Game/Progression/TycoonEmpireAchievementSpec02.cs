using System;

namespace Bussigo.Game.Progression
{
    public class TycoonEmpireAchievementSpec02
    {
        public string AchievementKey => "ACH_SOUTH_EMPIRE_02";
        public string TitleEnglish => "South Transport Master Badge 02";
        public string TitleTelugu => "రవాణా చక్రవర్తి పురస్కారం 02";
        public string Description => "Transport 100000 passengers across Andhra Pradesh and Telangana.";
        public bool IsUnlocked { get; set; } = false;
        public float Progress01 { get; set; } = 0.0f;
    }
}
