using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.World
{
    public class HighwayHighMastLightingController36
    {
        public string GantryPoleId => "LIGHT-POLE-NH-036";
        public float PoleHeightMeters { get; set; } = 24.0f;
        public float IlluminanceLuxRating { get; set; } = 85.0f;
        public bool IsPhotocellNightActive { get; private set; } = false;
        public float PowerRatingKilowatts => 1.8f;

        public void EvaluatePhotocellSensor(float sunElevationDegrees)
        {
            IsPhotocellNightActive = sunElevationDegrees < 5.0f;
        }
    }
}
