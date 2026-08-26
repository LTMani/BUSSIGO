using System;
using System.Collections.Generic;

namespace Bussigo.Game.Company
{
    public class RegionalDepot
    {
        public string DepotId { get; set; }
        public string CityName { get; set; }
        public int MaxBusCapacity { get; set; } = 8;
        public int CurrentBusCount { get; set; } = 0;
        public bool HasWorkshop { get; set; } = true;
        public bool HasAutomatedWashPlant { get; set; } = false;
        public bool HasFuelPump { get; set; } = true;
        public bool HasDriverDormitory { get; set; } = true;
        public float MonthlyUpkeepCost { get; set; } = 45000.0f;
    }

    public class TravelCompanyProfile
    {
        public string CompanyName { get; set; } = "Deccan Royal Express Travels";
        public string Slogan { get; set; } = "Connecting South India with Luxury & Safety";
        public int CompanyLevel { get; set; } = 1;
        public long TotalXpEarned { get; set; } = 0;
        public float ReputationRating { get; set; } = 4.5f; // 1.0 to 5.0 stars

        public List<RegionalDepot> Depots { get; } = new List<RegionalDepot>();

        public TravelCompanyProfile()
        {
            // Initial founding depot in Vijayawada
            Depots.Add(new RegionalDepot
            {
                DepotId = "DEP-VJA-01",
                CityName = "Vijayawada Auto Nagar Depot",
                MaxBusCapacity = 6,
                HasWorkshop = true,
                HasFuelPump = true
            });
        }
    }
}
