using System;

namespace Bussigo.Game.Passengers
{
    public enum SeatType
    {
        WindowSeat,
        AisleSeat,
        MiddleSeat,
        UpperSleeperBerth,
        LowerSleeperBerth
    }

    public class SeatSlot
    {
        public int SeatNumber { get; set; }
        public SeatType Type { get; set; }
        public bool IsBooked { get; set; } = false;
        public string PassengerName { get; set; }
        public float SeatFareRupees { get; set; }
    }
}
