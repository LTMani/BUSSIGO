using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Traffic
{
    public enum SignalPhase
    {
        NorthSouthGreen,
        NorthSouthAmber,
        AllRedClearance,
        EastWestGreen,
        EastWestAmber,
        PedestrianWalk
    }

    public class TrafficSignalJunctionController18
    {
        public string JunctionCode => "JUNC-SIGNAL-AP-018";
        public SignalPhase CurrentPhase { get; private set; } = SignalPhase.NorthSouthGreen;
        public float PhaseTimerSeconds { get; private set; } = 0.0f;
        public float GreenDurationSec { get; set; } = 55.0f;
        public float AmberDurationSec { get; set; } = 4.5f;
        public float AllRedDurationSec { get; set; } = 2.5f;

        public void UpdateSignalCycle(float deltaTime)
        {
            PhaseTimerSeconds += deltaTime;

            switch (CurrentPhase)
            {
                case SignalPhase.NorthSouthGreen:
                    if (PhaseTimerSeconds >= GreenDurationSec)
                    {
                        CurrentPhase = SignalPhase.NorthSouthAmber;
                        PhaseTimerSeconds = 0.0f;
                    }
                    break;
                case SignalPhase.NorthSouthAmber:
                    if (PhaseTimerSeconds >= AmberDurationSec)
                    {
                        CurrentPhase = SignalPhase.AllRedClearance;
                        PhaseTimerSeconds = 0.0f;
                    }
                    break;
                case SignalPhase.AllRedClearance:
                    if (PhaseTimerSeconds >= AllRedDurationSec)
                    {
                        CurrentPhase = SignalPhase.EastWestGreen;
                        PhaseTimerSeconds = 0.0f;
                    }
                    break;
                case SignalPhase.EastWestGreen:
                    if (PhaseTimerSeconds >= GreenDurationSec)
                    {
                        CurrentPhase = SignalPhase.EastWestAmber;
                        PhaseTimerSeconds = 0.0f;
                    }
                    break;
                case SignalPhase.EastWestAmber:
                    if (PhaseTimerSeconds >= AmberDurationSec)
                    {
                        CurrentPhase = SignalPhase.NorthSouthGreen;
                        PhaseTimerSeconds = 0.0f;
                    }
                    break;
            }
        }

        public bool CanBusProceed(bool isTravellingNorthSouth)
        {
            if (isTravellingNorthSouth)
            {
                return CurrentPhase == SignalPhase.NorthSouthGreen;
            }
            else
            {
                return CurrentPhase == SignalPhase.EastWestGreen;
            }
        }
    }
}
