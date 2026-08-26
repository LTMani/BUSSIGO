using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bussigo.Passengers
{
    [Serializable]
    public class PassengerTicket
    {
        public string ticketID;
        public string passengerID;
        public string originNodeID;
        public string destinationNodeID;
        public int seatNumber;
        public float fareRupees;
        public bool isValidated;

        public PassengerTicket(string tId, string pId, string origin, string dest, int seat, float fare)
        {
            ticketID = tId;
            passengerID = pId;
            originNodeID = origin;
            destinationNodeID = dest;
            seatNumber = seat;
            fareRupees = fare;
            isValidated = false;
        }
    }

    [Serializable]
    public class BusSeat
    {
        public int seatNumber; // 1 to 44
        public int rowNumber; // 1 to 11
        public string seatPosition; // WindowLeft, AisleLeft, AisleRight, WindowRight
        public bool isOccupied;
        public string currentPassengerID;

        public BusSeat(int num, int row, string pos)
        {
            seatNumber = num;
            rowNumber = row;
            seatPosition = pos;
            isOccupied = false;
            currentPassengerID = null;
        }
    }

    public class SeatManager
    {
        public const int MAX_PASSENGER_SEATS = 44;
        private readonly List<BusSeat> seats = new List<BusSeat>();

        public IReadOnlyList<BusSeat> Seats => seats;

        public SeatManager()
        {
            // Initialize 11 rows of 2+2 seats
            string[] posNames = { "WindowLeft", "AisleLeft", "AisleRight", "WindowRight" };
            int seatIdx = 1;

            for (int r = 1; r <= 11; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    seats.Add(new BusSeat(seatIdx++, r, posNames[c]));
                }
            }
        }

        public int OccupiedSeatCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < seats.Count; i++)
                {
                    if (seats[i].isOccupied) count++;
                }
                return count;
            }
        }

        public int AssignNextAvailableSeat(string passengerID)
        {
            for (int i = 0; i < seats.Count; i++)
            {
                if (!seats[i].isOccupied)
                {
                    seats[i].isOccupied = true;
                    seats[i].currentPassengerID = passengerID;
                    return seats[i].seatNumber;
                }
            }
            return -1; // Bus Full
        }

        public void ReleaseSeat(int seatNumber)
        {
            if (seatNumber >= 1 && seatNumber <= 44)
            {
                seats[seatNumber - 1].isOccupied = false;
                seats[seatNumber - 1].currentPassengerID = null;
            }
        }
    }
}
