using System;
using System.Collections.Generic;

namespace Bussigo.Game.Store
{
    public class InAppPackage
    {
        public string PackageId { get; set; }
        public string Title { get; set; }
        public long InGameCoinsAmount { get; set; }
        public string PriceDisplay { get; set; }
    }

    public class MockStoreManager
    {
        public List<InAppPackage> AvailablePackages { get; } = new List<InAppPackage>();

        public MockStoreManager()
        {
            AvailablePackages.Add(new InAppPackage { PackageId = "COIN_STARTER", Title = "Starter Bus Pass (100,000 Coins)", InGameCoinsAmount = 100000, PriceDisplay = "MOCK ₹99" });
            AvailablePackages.Add(new InAppPackage { PackageId = "COIN_FLEET", Title = "Fleet Tycoon Pack (1,000,000 Coins)", InGameCoinsAmount = 1000000, PriceDisplay = "MOCK ₹499" });
            AvailablePackages.Add(new InAppPackage { PackageId = "DLC_EXPANSION", Title = "Karnataka & Tamil Nadu Highway DLC", InGameCoinsAmount = 0, PriceDisplay = "MOCK ₹799" });
        }
    }
}
