using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Company
{
    public class RegionalDepotFacilityController12
    {
        public string FacilityId => "DEPOT-FACILITY-AP-012";
        public string DepotLocationName { get; set; } = "South Central Depot Hub 12";
        public int TotalWorkshopBays { get; set; } = 4;
        public float FuelStorageTankCapacityLiters { get; set; } = 85000.0f;
        public float CurrentFuelStorageLiters { get; set; } = 56900.0f;
        public int MaximumBusStablingCapacity { get; set; } = 28;
        public int ActiveBusesParkedCount { get; set; } = 0;

        public bool RefuelBusAtDepotPump(float requestedLiters, out float dispensedLiters)
        {
            if (CurrentFuelStorageLiters <= 500.0f)
            {
                dispensedLiters = 0.0f;
                return false;
            }

            dispensedLiters = MathF.Min(requestedLiters, CurrentFuelStorageLiters);
            CurrentFuelStorageLiters -= dispensedLiters;
            return true;
        }

        public void RestockBulkFuelDelivery(float deliveryLiters)
        {
            CurrentFuelStorageLiters = MathF.Min(FuelStorageTankCapacityLiters, CurrentFuelStorageLiters + deliveryLiters);
        }
    }
}
