using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Passengers
{
    public enum PassengerType
    {
        DailyCityCommuter,
        IntercityFamily,
        SeniorPilgrimToTirupati,
        CollegeStudent,
        BusinessExecutive,
        RuralFarmerFeeder
    }

    public class PassengerEntity
    {
        public string PassengerId { get; set; }
        public string Name { get; set; }
        public PassengerType Type { get; set; }
        public string OriginTerminal { get; set; }
        public string DestinationTerminal { get; set; }
        public int AssignedSeatNumber { get; set; }
        public float LuggageWeightKg { get; set; }
        public float TicketFarePaidRupees { get; set; }
        public float EmotionalComfortScore { get; set; } = 100.0f;
    }

    public class PassengerCrowdManager
    {
        public List<PassengerEntity> CurrentBusPassengers { get; } = new List<PassengerEntity>();
        public List<PassengerEntity> TerminalWaitingQueue { get; } = new List<PassengerEntity>();

        public int TotalPassengersTransportedLifetime { get; private set; } = 0;
        public float TotalFareCollectedLifetimeRupees { get; private set; } = 0.0f;

        public void GenerateTerminalCrowd(string terminalCode, int crowdSize)
        {
            TerminalWaitingQueue.Clear();
            var rnd = new Random(101);

            for (int i = 1; i <= crowdSize; i++)
            {
                var passenger = new PassengerEntity
                {
                    PassengerId = $"PAX-{terminalCode}-{i:D3}",
                    Name = $"Passenger {i}",
                    Type = (PassengerType)(i % 6),
                    OriginTerminal = terminalCode,
                    DestinationTerminal = "HYD",
                    AssignedSeatNumber = i,
                    LuggageWeightKg = 8.0f + (float)(rnd.NextDouble() * 22.0),
                    TicketFarePaidRupees = 450.0f
                };
                TerminalWaitingQueue.Add(passenger);
            }
        }

        public int BoardAllEligiblePassengers(int busMaxCapacity)
        {
            int boardedCount = 0;
            while (TerminalWaitingQueue.Count > 0 && CurrentBusPassengers.Count < busMaxCapacity)
            {
                var pax = TerminalWaitingQueue[0];
                TerminalWaitingQueue.RemoveAt(0);
                CurrentBusPassengers.Add(pax);
                boardedCount++;
                TotalPassengersTransportedLifetime++;
                TotalFareCollectedLifetimeRupees += pax.TicketFarePaidRupees;
            }
            return boardedCount;
        }

        public int AlightPassengersAtDestination(string currentStopCode)
        {
            int alightedCount = 0;
            for (int i = CurrentBusPassengers.Count - 1; i >= 0; i--)
            {
                if (CurrentBusPassengers[i].DestinationTerminal == currentStopCode)
                {
                    CurrentBusPassengers.RemoveAt(i);
                    alightedCount++;
                }
            }
            return alightedCount;
        }
    }
}
