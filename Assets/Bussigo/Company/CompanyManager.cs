using System;
using System.Collections.Generic;
using UnityEngine;
using Bussigo.Core;
using Bussigo.Economy;

namespace Bussigo.Company
{
    /// <summary>
    /// Master travel empire company manager coordinating fleet inventory, hired drivers, and company career progression.
    /// </summary>
    public class CompanyManager : MonoBehaviour, IService
    {
        [Header("Company Identity & Reputation")]
        public string companyName = "BUSSIGO Royal Travels";
        public int companyLevel = 1;
        public int currentExperienceXP = 0;
        [Range(0f, 100f)] public float companyReputationPercent = 88.0f;

        [Header("Fleet & Staff Registry")]
        public readonly List<FleetBusData> ownedFleet = new List<FleetBusData>();
        public readonly List<DriverProfile> hiredDrivers = new List<DriverProfile>();
        public readonly List<string> unlockedRouteIDs = new List<string>();

        public FleetBusData ActiveHeroBus => ownedFleet.Count > 0 ? ownedFleet[0] : null;

        public void Initialize()
        {
            ServiceLocator.Register<CompanyManager>(this);

            // 1. Initialize Default Flagship Hero Bus (12.5m Indian Intercity Coach)
            if (ownedFleet.Count == 0)
            {
                var heroBus = new FleetBusData(
                    "BUS_HERO_01",
                    "BUSSIGO Royal 12.5M Intercity Coach",
                    "AP 16 TX 4499",
                    BusChassisType.Standard12MCoach,
                    3800000.0,
                    44
                );
                ownedFleet.Add(heroBus);
            }

            // 2. Initialize Default Senior Driver
            if (hiredDrivers.Count == 0)
            {
                var seniorDriver = new DriverProfile(
                    "DRV_01",
                    "Srinivasa Rao (Senior Captain)",
                    4,
                    32000.0,
                    4.8f
                );
                seniorDriver.assignedBusID = "BUS_HERO_01";
                hiredDrivers.Add(seniorDriver);
            }

            // 3. Initialize Flagship Route
            if (unlockedRouteIDs.Count == 0)
            {
                unlockedRouteIDs.Add("ROUTE_NH65_VJA_HYD");
            }

            Debug.Log("[BUSSIGO] CompanyManager initialized with Fleet and Staff.");
        }

        public void Shutdown()
        {
            // Clean shutdown
        }

        public void AddExperience(int xpAmount)
        {
            currentExperienceXP += xpAmount;
            int nextLevelRequirement = companyLevel * 1000;
            if (currentExperienceXP >= nextLevelRequirement)
            {
                companyLevel++;
                currentExperienceXP -= nextLevelRequirement;
                Debug.Log($"[BUSSIGO Empire] LEVEL UP! Company reached Level {companyLevel}");
            }
        }

        public void UpdateReputation(float tripSatisfaction)
        {
            // Exponential moving average update
            companyReputationPercent = Mathf.Clamp(Mathf.Lerp(companyReputationPercent, tripSatisfaction, 0.25f), 0f, 100f);
        }

        public bool PurchaseNewCoach(EconomyManager economy, string modelName, string regNum, double priceRupees)
        {
            if (economy == null) return false;

            if (economy.ledger.RecordTransaction(TransactionCategory.FleetPurchase, EntryType.Debit, priceRupees, $"Acquired New Coach: {modelName} ({regNum})"))
            {
                string id = $"BUS_{ownedFleet.Count + 1:D2}";
                var newBus = new FleetBusData(id, modelName, regNum, BusChassisType.Standard12MCoach, priceRupees, 44);
                ownedFleet.Add(newBus);
                AddExperience(500);
                return true;
            }
            return false;
        }
    }
}
