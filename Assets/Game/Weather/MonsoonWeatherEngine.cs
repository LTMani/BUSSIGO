using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Weather
{
    public enum WeatherType
    {
        ClearSunny,
        SummerHeatWave,
        OvercastCloudy,
        LightDrizzle,
        HeavyTropicalMonsoon,
        GhatValleyFog
    }

    public class MonsoonWeatherEngine
    {
        public WeatherType CurrentWeather { get; private set; } = WeatherType.ClearSunny;
        public float RainIntensity01 { get; private set; } = 0.0f;
        public float FogDensity01 { get; private set; } = 0.0f;
        public float RoadSurfaceFrictionMultiplier { get; private set; } = 1.0f;
        public float WindSpeedKmh { get; private set; } = 12.0f;

        public void SetWeather(WeatherType type)
        {
            CurrentWeather = type;
            switch (type)
            {
                case WeatherType.ClearSunny:
                    RainIntensity01 = 0.0f;
                    FogDensity01 = 0.0f;
                    RoadSurfaceFrictionMultiplier = 1.0f;
                    WindSpeedKmh = 10.0f;
                    break;
                case WeatherType.SummerHeatWave:
                    RainIntensity01 = 0.0f;
                    FogDensity01 = 0.05f; // Heat haze
                    RoadSurfaceFrictionMultiplier = 0.98f;
                    WindSpeedKmh = 5.0f;
                    break;
                case WeatherType.LightDrizzle:
                    RainIntensity01 = 0.35f;
                    FogDensity01 = 0.15f;
                    RoadSurfaceFrictionMultiplier = 0.82f;
                    WindSpeedKmh = 25.0f;
                    break;
                case WeatherType.HeavyTropicalMonsoon:
                    RainIntensity01 = 1.0f;
                    FogDensity01 = 0.45f;
                    RoadSurfaceFrictionMultiplier = 0.60f; // Significant wet road grip reduction
                    WindSpeedKmh = 65.0f;
                    break;
                case WeatherType.GhatValleyFog:
                    RainIntensity01 = 0.1f;
                    FogDensity01 = 0.85f;
                    RoadSurfaceFrictionMultiplier = 0.78f;
                    WindSpeedKmh = 8.0f;
                    break;
            }
        }
    }
}
