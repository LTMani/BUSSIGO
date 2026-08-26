using System;
using System.Collections.Generic;

namespace Bussigo.Game.Core
{
    public enum GamePlatformMode
    {
        PC,
        Mobile,
        Console
    }

    public enum GraphicsQualityTier
    {
        Low,
        Medium,
        High,
        Ultra
    }

    public class GameConfiguration
    {
        public static GameConfiguration Active { get; set; } = new GameConfiguration();

        public GamePlatformMode PlatformMode { get; set; } = GamePlatformMode.PC;
        public GraphicsQualityTier QualityTier { get; set; } = GraphicsQualityTier.High;
        public string ActiveLanguage { get; set; } = "en";

        public bool EnableTrafficAI { get; set; } = true;
        public int MaxTrafficDensity { get; set; } = 64;
        public bool EnableDynamicWeather { get; set; } = true;
        public bool EnableForceFeedback { get; set; } = false;
        public float MasterAudioVolume { get; set; } = 1.0f;
        public float EngineAudioVolume { get; set; } = 0.9f;
        public float AmbienceAudioVolume { get; set; } = 0.7f;
        public float HornAudioVolume { get; set; } = 1.0f;
        public float VoiceAudioVolume { get; set; } = 0.85f;

        public bool IsMetricUnits { get; set; } = true;
        public float SteeringSensitivity { get; set; } = 1.0f;
        public float SteeringSmoothing { get; set; } = 0.15f;
        public bool AutomaticTransmission { get; set; } = false;
        public bool ABSBrakingAssist { get; set; } = true;
        public bool CruiseControlEnabled { get; set; } = true;
    }
}
