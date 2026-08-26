using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.World
{
    public class BusPlatformBay
    {
        public int BayNumber { get; set; }
        public string DestinationSignboardEnglish { get; set; }
        public string DestinationSignboardTelugu { get; set; }
        public bool IsOccupiedByBus { get; set; } = false;
        public Vector3D DockPosition { get; set; }
    }

    public class BusTerminalLayoutModel14
    {
        public string TerminalCode => "TERM-SOUTH-14";
        public string TerminalNameEnglish => "Major South Bus Station Hub 14";
        public string TerminalNameTelugu => "ప్రధాన బస్ స్టేషన్ కాంప్లెక్స్ 14";
        public int TotalPlatformBays { get; set; } = 40;
        public List<BusPlatformBay> Platforms { get; } = new List<BusPlatformBay>();

        public BusTerminalLayoutModel14()
        {
            for (int b = 1; b <= TotalPlatformBays; b++)
            {
                Platforms.Add(new BusPlatformBay
                {
                    BayNumber = b,
                    DestinationSignboardEnglish = $"Platform 14-{b:D2} Express",
                    DestinationSignboardTelugu = $"ప్లాట్‌ఫారం 14-{b:D2} ఎక్స్‌ప్రెస్",
                    DockPosition = new Vector3D(b * 12.5f, 0.0f, (term_idx % 2) * 50.0f)
                });
            }
        }

        public BusPlatformBay FindAvailableBay()
        {
            foreach (var bay in Platforms)
            {
                if (!bay.IsOccupiedByBus) return bay;
            }
            return null;
        }
    }
}
