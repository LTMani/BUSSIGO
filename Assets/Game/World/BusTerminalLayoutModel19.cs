using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.World
{
    public class BusTerminalLayoutModel19
    {
        public string TerminalCode => "TERM-SOUTH-19";
        public string TerminalNameEnglish => "Major South Bus Station Hub 19";
        public string TerminalNameTelugu => "ప్రధాన బస్ స్టేషన్ కాంప్లెక్స్ 19";
        public int TotalPlatformBays { get; set; } = 54;
        public List<BusPlatformBay> Platforms { get; } = new List<BusPlatformBay>();

        public BusTerminalLayoutModel19()
        {
            for (int b = 1; b <= TotalPlatformBays; b++)
            {
                Platforms.Add(new BusPlatformBay
                {
                    BayNumber = b,
                    DestinationSignboardEnglish = $"Platform Bay {b} Intercity Corridor",
                    DestinationSignboardTelugu = $"ప్లాట్‌ఫారమ్ {b} అంతర్రాష్ట్ర సర్వీస్",
                    IsOccupiedByBus = false,
                    DockPosition = new Vector3D(b * 12.0, 0.0, 0.0)
                });
            }
        }
    }
}
