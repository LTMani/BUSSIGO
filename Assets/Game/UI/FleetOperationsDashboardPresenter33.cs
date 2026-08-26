using System;
using System.Collections.Generic;
using Bussigo.Game.Core;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.UI
{
    public class FleetOperationsDashboardPresenter33
    {
        public string PresenterCode => "PRESENTER-OPS-33";
        public string HeaderTitleEnglish => "Fleet Telemetry & Dispatch Console 33";
        public string HeaderTitleTelugu => "బస్సుల నిర్వహణ మరియు ట్రాకింగ్ కన్సోల్ 33";
        public bool IsLiveConnected { get; set; } = true;
        public float RefreshRateHz { get; set; } = 60.0f;
        public List<string> ActiveTelemetryMetrics { get; } = new List<string>();

        public void RefreshDashboardView()
        {
            ActiveTelemetryMetrics.Clear();
            for (int m = 1; m <= 10; m++)
            {
                ActiveTelemetryMetrics.Add($"Channel 33-{m:D2}: Sensor Validated OK");
            }
        }

        public float GetSimulatedBusSpeed()
        {
            return 68.5f;
        }
    }
}
