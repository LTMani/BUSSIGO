using System;

namespace Bussigo.Core
{
    public struct TripStartedEvent : IGameEvent
    {
        public string RouteName;
        public float DistanceKm;
        public DateTime StartTime;

        public TripStartedEvent(string routeName, float distanceKm)
        {
            RouteName = routeName;
            DistanceKm = distanceKm;
            StartTime = DateTime.UtcNow;
        }
    }

    public struct PassengerBoardingCompletedEvent : IGameEvent
    {
        public int TotalPassengers;
        public int TicketRevenueINR;

        public PassengerBoardingCompletedEvent(int totalPassengers, int ticketRevenueINR)
        {
            TotalPassengers = totalPassengers;
            TicketRevenueINR = ticketRevenueINR;
        }
    }

    public struct TollPlazaCrossedEvent : IGameEvent
    {
        public string TollPlazaName;
        public int FeeDeductedINR;
        public int RemainingBalanceINR;

        public TollPlazaCrossedEvent(string tollPlazaName, int feeDeductedINR, int remainingBalanceINR)
        {
            TollPlazaName = tollPlazaName;
            FeeDeductedINR = feeDeductedINR;
            RemainingBalanceINR = remainingBalanceINR;
        }
    }

    public struct TripCompletedEvent : IGameEvent
    {
        public string DestinationTerminal;
        public int GrossRevenueINR;
        public int NetProfitINR;
        public int EarnedDriverXP;

        public TripCompletedEvent(string destinationTerminal, int grossRevenueINR, int netProfitINR, int earnedDriverXP)
        {
            DestinationTerminal = destinationTerminal;
            GrossRevenueINR = grossRevenueINR;
            NetProfitINR = netProfitINR;
            EarnedDriverXP = earnedDriverXP;
        }
    }
}
