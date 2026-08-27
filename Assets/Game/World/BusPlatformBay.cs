using System;
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
}
