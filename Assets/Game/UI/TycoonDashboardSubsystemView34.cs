using System;
using System.Collections.Generic;
using Bussigo.Game.Core;
using Bussigo.Game.Economy;

namespace Bussigo.Game.UI
{
    public class TycoonDashboardSubsystemView34
    {
        public string SubsystemName => "Dashboard Module 34";
        public bool IsActiveTab { get; set; } = false;
        public float ScrollOffsetPixels { get; set; } = 0.0f;
        public List<string> TelemetryCardTitles { get; } = new List<string>();

        public void BindDataSources()
        {
            TelemetryCardTitles.Clear();
            for (int c = 1; c <= 8; c++)
            {
                TelemetryCardTitles.Add($"Data Card 34-{c:D2} Status Active");
            }
        }

        public void HandleScroll(float deltaY)
        {
            ScrollOffsetPixels = MathF.Max(0.0f, ScrollOffsetPixels + deltaY);
        }
    }
}
