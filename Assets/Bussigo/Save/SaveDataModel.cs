using System;
using System.Collections.Generic;
using Bussigo.Company;
using Bussigo.Economy;

namespace Bussigo.Save
{
    [Serializable]
    public class GameSaveData
    {
        public string schemaVersion = "2.0.0";
        public string saveTimestampIso;
        public string companyName;
        public int companyLevel;
        public int currentExperienceXP;
        public float companyReputationPercent;
        public double companyBalanceRupees;

        public List<FleetBusData> ownedFleet = new List<FleetBusData>();
        public List<DriverProfile> hiredDrivers = new List<DriverProfile>();
        public List<string> unlockedRouteIDs = new List<string>();
        public List<TripFinancialReport> completedTripHistory = new List<TripFinancialReport>();

        public string checksumSha256;

        public GameSaveData()
        {
            saveTimestampIso = DateTime.UtcNow.ToString("o");
        }
    }
}
