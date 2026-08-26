using System;
using System.Collections.Generic;

namespace Bussigo.Game.Localization
{
    public static class LocalizationCatalog
    {
        public static Dictionary<string, Dictionary<string, string>> Translations { get; } = new Dictionary<string, Dictionary<string, string>>();

        static LocalizationCatalog()
        {
            var en = new Dictionary<string, string>
            {
                { "ui.game_title", "South India Bus & Travel Empire Simulator" },
                { "ui.start_trip", "Start Journey" },
                { "ui.depart", "Depart" },
                { "ui.arrive", "Arrive" },
                { "ui.speed_kmh", "km/h" },
                { "ui.air_pressure", "Air Pressure" },
                { "ui.fare_collected", "Fare Collected" },
                { "ui.garage", "Fleet Garage" },
                { "ui.company_hq", "Company Headquarters" }
            };

            var te = new Dictionary<string, string>
            {
                { "ui.game_title", "దక్షిణ భారత బస్సు & ట్రావెల్ ఎంపైర్ సిమ్యులేటర్" },
                { "ui.start_trip", "ప్రయాణం ప్రారంభించండి" },
                { "ui.depart", "బయలుదేరు" },
                { "ui.arrive", "గమ్యం చేరు" },
                { "ui.speed_kmh", "కిమీ/గం" },
                { "ui.air_pressure", "ఎయిర్ ప్రెజర్" },
                { "ui.fare_collected", "టికెట్ ఆదాయం" },
                { "ui.garage", "బస్సు గ్యారేజ్" },
                { "ui.company_hq", "ట్రావెల్స్ ప్రధాన కార్యాలయం" }
            };

            Translations["en"] = en;
            Translations["te"] = te;
        }

        public static string GetString(string key, string lang = "en")
        {
            if (Translations.TryGetValue(lang, out var dict) && dict.TryGetValue(key, out var val))
            {
                return val;
            }
            if (Translations["en"].TryGetValue(key, out var fallbackVal))
            {
                return fallbackVal;
            }
            return key;
        }
    }
}
