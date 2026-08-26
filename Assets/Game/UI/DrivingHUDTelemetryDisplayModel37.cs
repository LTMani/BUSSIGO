using System;
using Bussigo.Game.Core;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.UI
{
    public class DrivingHUDTelemetryDisplayModel37
    {
        public string HUDProfileId => "HUD-PROFILE-037";
        public float FilteredSpeedKmh { get; private set; } = 0.0f;
        public float FilteredRpm { get; private set; } = 600.0f;
        public float FilteredAirPressureBar { get; private set; } = 8.5f;
        public float PassengerSmileComfortPercent { get; private set; } = 100.0f;
        public string ActiveGearDisplayName { get; private set; } = "N";

        public void UpdateSmoothTelemetry(float rawSpeedKmh, float rawRpm, float rawAirPressure, float comfortPercent, int gear, float deltaTime)
        {
            FilteredSpeedKmh = CoreMath.MoveTowards(FilteredSpeedKmh, rawSpeedKmh, deltaTime * 85.0f);
            FilteredRpm = CoreMath.MoveTowards(FilteredRpm, rawRpm, deltaTime * 2200.0f);
            FilteredAirPressureBar = CoreMath.MoveTowards(FilteredAirPressureBar, rawAirPressure, deltaTime * 2.0f);
            PassengerSmileComfortPercent = CoreMath.MoveTowards(PassengerSmileComfortPercent, comfortPercent, deltaTime * 15.0f);

            if (gear == 0) ActiveGearDisplayName = "N";
            else if (gear == -1) ActiveGearDisplayName = "R";
            else ActiveGearDisplayName = $"G{gear}";
        }
    }
}
