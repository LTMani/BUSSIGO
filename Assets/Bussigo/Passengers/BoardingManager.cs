using System;
using System.Collections.Generic;
using UnityEngine;
using Bussigo.Route;

namespace Bussigo.Passengers
{
    /// <summary>
    /// Coordinates terminal passenger queues, ticket validation, luggage stowing, and deboarding logistics.
    /// </summary>
    public class BoardingManager
    {
        public SeatManager seatManager = new SeatManager();
        public readonly List<PassengerProfile> onboardPassengers = new List<PassengerProfile>();
        public readonly Queue<PassengerProfile> terminalWaitingQueue = new Queue<PassengerProfile>();

        public void PopulateTerminalQueue(string originNodeID, int passengerCount)
        {
            terminalWaitingQueue.Clear();
            int count = Mathf.Min(passengerCount, SeatManager.MAX_PASSENGER_SEATS);

            string[] names = { "Ramesh K.", "Lakshmi P.", "Srinivas Rao", "Ananya Reddy", "Venkatesh M.", "Pooja Sharma", "Karthik V.", "Divya N." };
            PassengerCategory[] cats = { PassengerCategory.SoloTraveller, PassengerCategory.Family, PassengerCategory.Student, PassengerCategory.BusinessTraveller };
            LuggageSize[] lugs = { LuggageSize.SmallHandbag, LuggageSize.MediumSuitcase, LuggageSize.LargeTrunk };

            for (int i = 0; i < count; i++)
            {
                string id = $"PAX_{originNodeID}_{i+1:D3}";
                string name = names[i % names.Length];
                var cat = cats[i % cats.Length];
                var lug = lugs[i % lugs.Length];
                string dest = (originNodeID == "NODE_VJA_PNBS" && i % 3 == 0) ? "NODE_SYP_HUB" : "NODE_HYD_MGBS";

                var p = new PassengerProfile(id, name, cat, originNodeID, dest, lug);
                terminalWaitingQueue.Enqueue(p);
            }
        }

        public bool ProcessNextBoardingPassenger(out PassengerProfile boardedPassenger)
        {
            boardedPassenger = null;
            if (terminalWaitingQueue.Count == 0) return false;

            var p = terminalWaitingQueue.Dequeue();
            p.boardingState = BoardingState.TicketCheck;

            // Assign Seat
            int seatNum = seatManager.AssignNextAvailableSeat(p.passengerID);
            if (seatNum == -1)
            {
                p.boardingState = BoardingState.BoardingDenied;
                return false;
            }

            p.assignedSeatNumber = seatNum;
            p.boardingState = BoardingState.BoardingBus;

            // Complete Seating
            p.boardingState = BoardingState.Seated;
            onboardPassengers.Add(p);
            boardedPassenger = p;
            return true;
        }

        public List<PassengerProfile> ProcessDeboardingAtNode(string currentNodeID)
        {
            var deboarded = new List<PassengerProfile>();

            for (int i = onboardPassengers.Count - 1; i >= 0; i--)
            {
                var p = onboardPassengers[i];
                if (p.destinationNodeID == currentNodeID)
                {
                    p.boardingState = BoardingState.Deboarding;
                    seatManager.ReleaseSeat(p.assignedSeatNumber);
                    p.luggageState = LuggageState.Unloaded;
                    p.boardingState = BoardingState.TripCompleted;
                    deboarded.Add(p);
                    onboardPassengers.RemoveAt(i);
                }
            }

            return deboarded;
        }
    }
}
