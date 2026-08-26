using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Passengers
{
    public class LuggageCompartmentLoadDistribution02
    {
        public string CompartmentId => "LUGGAGE-BAY-SECTION-002";
        public float MaxLuggageVolumeCapacityM3 { get; set; } = 11.5f;
        public float MaxLuggageWeightCapacityKg { get; set; } = 1700.0f;
        public float CurrentLuggageWeightKg { get; private set; } = 0.0f;
        public float CurrentLuggageVolumeM3 { get; private set; } = 0.0f;

        public bool TryLoadLuggage(float weightKg, float volumeM3)
        {
            if (CurrentLuggageWeightKg + weightKg > MaxLuggageWeightCapacityKg ||
                CurrentLuggageVolumeM3 + volumeM3 > MaxLuggageVolumeCapacityM3)
            {
                return false; // Luggage bay full
            }

            CurrentLuggageWeightKg += weightKg;
            CurrentLuggageVolumeM3 += volumeM3;
            return true;
        }

        public void UnloadAllLuggage()
        {
            CurrentLuggageWeightKg = 0.0f;
            CurrentLuggageVolumeM3 = 0.0f;
        }
    }
}
