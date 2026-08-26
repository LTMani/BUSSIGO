using System;
using System.Collections.Generic;

namespace Bussigo.Game.Routes
{
    public enum ServiceTier
    {
        Pallevelugu,   // Rural ordinary service, frequent village stops
        Express,       // Intercity fast passenger
        UltraDeluxe,   // Non-stop pushback
        SuperLuxury,   // Air suspension express
        GarudaAC,      // Multi-axle Volvo/Scania AC
        VennelaSleeper // Overnight luxury sleeper
    }

    public class ScheduledTrip
    {
        public string TripCode { get; set; }
        public string CorridorId { get; set; }
        public ServiceTier Tier { get; set; }
        public float DepartureHour { get; set; } // 0.0 to 24.0
        public float ArrivalHour { get; set; }
        public float BaseFarePerSeatRupees { get; set; }
        public int TotalSeatsBooked { get; set; }

        public ScheduledTrip(string code, string corridorId, ServiceTier tier, float depHour, float arrHour, float baseFare)
        {
            TripCode = code;
            CorridorId = corridorId;
            Tier = tier;
            DepartureHour = depHour;
            ArrivalHour = arrHour;
            BaseFarePerSeatRupees = baseFare;
        }
    }

    public class TimetableSchedule
    {
        public List<ScheduledTrip> DailyTrips { get; } = new List<ScheduledTrip>();

        public void PopulateDefaultSchedules()
        {
            DailyTrips.Clear();
            // Morning Express VJA -> HYD
            DailyTrips.Add(new ScheduledTrip("TRIP-101", "COR-VJA-HYD-01", ServiceTier.SuperLuxury, 6.0f, 10.75f, 480.0f));
            DailyTrips.Add(new ScheduledTrip("TRIP-102", "COR-VJA-HYD-01", ServiceTier.GarudaAC, 7.30f, 12.0f, 650.0f));
            DailyTrips.Add(new ScheduledTrip("TRIP-103", "COR-VJA-HYD-01", ServiceTier.Express, 9.0f, 14.0f, 320.0f));

            // Afternoon & Evening
            DailyTrips.Add(new ScheduledTrip("TRIP-104", "COR-VJA-HYD-01", ServiceTier.UltraDeluxe, 13.0f, 17.75f, 420.0f));
            DailyTrips.Add(new ScheduledTrip("TRIP-105", "COR-VJA-HYD-01", ServiceTier.GarudaAC, 17.0f, 21.5f, 650.0f));
            
            // Night Sleeper Flagship
            DailyTrips.Add(new ScheduledTrip("TRIP-106", "COR-VJA-HYD-01", ServiceTier.VennelaSleeper, 22.30f, 4.0f, 850.0f));
            DailyTrips.Add(new ScheduledTrip("TRIP-107", "COR-VJA-HYD-01", ServiceTier.VennelaSleeper, 23.15f, 4.45f, 850.0f));

            // Frequent Shuttle: Vijayawada <-> Guntur (Every 30 mins)
            for (int h = 6; h <= 21; h++)
            {
                DailyTrips.Add(new ScheduledTrip($"TRIP-GNT-{h}00", "COR-VJA-GNT-02", ServiceTier.Express, h, h + 0.85f, 45.0f));
                DailyTrips.Add(new ScheduledTrip($"TRIP-GNT-{h}30", "COR-VJA-GNT-02", ServiceTier.Pallevelugu, h + 0.5f, h + 1.45f, 30.0f));
            }
        }
    }
}
