using System;
using UnityEngine;

namespace Bussigo.Weather
{
    public enum WeatherCondition
    {
        Clear = 0,
        PartlyCloudy = 1,
        Overcast = 2,
        LightRain = 3,
        ModerateRain = 4,
        HeavyMonsoon = 5,
        Storm = 6
    }

    [Serializable]
    public class WeatherProfile
    {
        public WeatherCondition condition;
        public string conditionName;
        public float rainRate; // 0.0 to 1.0
        public float cloudDensity; // 0.0 to 1.0
        public float visibilityMeters; // 300m to 3000m
        public float windStrengthMps;
        public float roadFrictionMultiplier; // 1.0 (dry) to 0.78 (monsoon)
        public float thunderProbability; // 0.0 to 1.0

        public WeatherProfile(WeatherCondition cond, string name, float rain, float clouds, float vis, float wind, float friction, float thunder)
        {
            condition = cond;
            conditionName = name;
            rainRate = rain;
            cloudDensity = clouds;
            visibilityMeters = vis;
            windStrengthMps = wind;
            roadFrictionMultiplier = friction;
            thunderProbability = thunder;
        }

        public static WeatherProfile Create(WeatherCondition cond)
        {
            switch (cond)
            {
                case WeatherCondition.Clear:
                    return new WeatherProfile(cond, "Clear Sky", 0.0f, 0.1f, 3000f, 2.5f, 1.0f, 0.0f);
                case WeatherCondition.PartlyCloudy:
                    return new WeatherProfile(cond, "Partly Cloudy", 0.0f, 0.4f, 2500f, 4.0f, 1.0f, 0.0f);
                case WeatherCondition.Overcast:
                    return new WeatherProfile(cond, "Overcast", 0.0f, 0.85f, 2000f, 6.0f, 0.98f, 0.0f);
                case WeatherCondition.LightRain:
                    return new WeatherProfile(cond, "Light Drizzle", 0.25f, 0.9f, 1400f, 8.0f, 0.92f, 0.0f);
                case WeatherCondition.ModerateRain:
                    return new WeatherProfile(cond, "Moderate Rain", 0.60f, 0.95f, 900f, 12.0f, 0.85f, 0.05f);
                case WeatherCondition.HeavyMonsoon:
                    return new WeatherProfile(cond, "Heavy Monsoon Downpour", 0.90f, 1.0f, 550f, 18.0f, 0.78f, 0.25f);
                case WeatherCondition.Storm:
                    return new WeatherProfile(cond, "Tropical Thunderstorm", 1.0f, 1.0f, 400f, 24.0f, 0.75f, 0.60f);
                default:
                    return new WeatherProfile(WeatherCondition.Clear, "Clear Sky", 0.0f, 0.1f, 3000f, 2.5f, 1.0f, 0.0f);
            }
        }
    }
}
