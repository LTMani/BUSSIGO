using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.World
{
    public class BusTerminalLayoutModel08
    {
        public string TerminalCode => "TERM-SOUTH-08";
        public string TerminalNameEnglish => "Major South Bus Station Hub 08";
        public string TerminalNameTelugu => "ప్రధాన బస్ స్టేషన్ కాంప్లెక్స్ 08";
        public int TotalPlatformBays { get; set; } = 32;
        public List<BusPlatformBay> Platforms { get; } = new List<BusPlatformBay>();

        public BusTerminalLayoutModel08()
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
