using System;
using System.Collections.Generic;

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

    public class PassengerSeatReservationMatrix24
    {
        public string BusLayoutCode => "LAYOUT-CONFIG-024";
        public int TotalSeatsCount { get; set; } = 52;
        public List<SeatSlot> Seats { get; } = new List<SeatSlot>();

        public PassengerSeatReservationMatrix24()
        {
            for (int s = 1; s <= TotalSeatsCount; s++)
            {
                Seats.Add(new SeatSlot
                {
                    SeatNumber = s,
                    Type = (s % 4 == 1 || s % 4 == 0) ? SeatType.WindowSeat : SeatType.AisleSeat,
                    SeatFareRupees = 420.00f
                });
            }
        }

        public bool ReserveSpecificSeat(int seatNumber, string passengerName)
        {
            var slot = Seats.Find(s => s.SeatNumber == seatNumber);
            if (slot != null && !slot.IsBooked)
            {
                slot.IsBooked = true;
                slot.PassengerName = passengerName;
                return true;
            }
            return false;
        }

        public int GetOccupiedSeatCount()
        {
            return Seats.FindAll(s => s.IsBooked).Count;
        }
    }
}
