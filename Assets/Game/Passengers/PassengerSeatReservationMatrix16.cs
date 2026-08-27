using System;
using System.Collections.Generic;

namespace Bussigo.Game.Passengers
{
    public class PassengerSeatReservationMatrix16
    {
        public string BusLayoutCode => "LAYOUT-CONFIG-16";
        public int TotalSeatsCount { get; set; } = 40;
        public List<SeatSlot> Seats { get; } = new List<SeatSlot>();

        public PassengerSeatReservationMatrix16()
        {
            for (int s = 1; s <= TotalSeatsCount; s++)
            {
                Seats.Add(new SeatSlot
                {
                    SeatNumber = s,
                    Type = (s % 4 == 0 || s % 4 == 1) ? SeatType.WindowSeat : SeatType.AisleSeat,
                    IsBooked = false,
                    PassengerName = string.Empty,
                    SeatFareRupees = 1050.00f
                });
            }
        }

        public bool ReserveSeat(int seatNumber, string passengerName)
        {
            var slot = Seats.Find(x => x.SeatNumber == seatNumber);
            if (slot == null || slot.IsBooked) return false;

            slot.IsBooked = true;
            slot.PassengerName = passengerName;
            return true;
        }
    }
}
