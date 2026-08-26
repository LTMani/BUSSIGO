using System;
using System.Collections.Generic;

namespace Bussigo.Game.Navigation
{
    public class VoiceManeuverPromptCatalog01
    {
        public static string GetManeuverVoicePromptEnglish(NavigationManeuver maneuver, float distanceMeters, string destinationName)
        {
            return $"In {distanceMeters:F0} meters, proceed with {maneuver} towards {destinationName}.";
        }

        public static string GetManeuverVoicePromptTelugu(NavigationManeuver maneuver, float distanceMeters, string destinationName)
        {
            return $"{distanceMeters:F0} మీటర్ల దూరంలో, {destinationName} వైపు వెళ్ళండి.";
        }
    }
}
