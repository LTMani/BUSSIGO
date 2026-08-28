using System;
using UnityEngine;
using Bussigo.Core;

namespace Bussigo.Economy
{
    /// <summary>
    /// Master travel tycoon economy manager orchestrating double-entry accounting, fare calculations, and trip settlements.
    /// </summary>
    public class EconomyManager : MonoBehaviour, IService
    {
        public CompanyLedger ledger = new CompanyLedger();

        [Header("Fuel & Toll Constants")]
        public double dieselPricePerLitre = 98.50; // ₹98.50 / Litre
        public float busFuelEconomyKmPerLitre = 3.8f; // 3.8 km/L for 14.5t coach
        public double kanchikacherlaTollFeeRupees = 185.0; // ₹185 FASTag Commercial Bus Toll

        [Header("Fare Pricing Structure")]
        public double baseBoardingFareRupees = 120.0;
        public double perKmRateRupees = 1.65; // ₹1.65 / km for Luxury AC Push-Back

        public double CurrentBalance => ledger.currentBalanceRupees;

        private bool isInitialized = false;

        public void Initialize()
        {
            if (isInitialized) return;
            isInitialized = true;

            ServiceLocator.Register<EconomyManager>(this);
            Debug.Log("[BUSSIGO] EconomyManager initialized with Double-Entry Ledger.");
        }

        /// <summary>
        /// Resets the economy to its default state (as if starting a new game).
        /// Does not affect ServiceLocator registration or the isInitialized flag.
        /// </summary>
        public void ResetToDefault()
        {
            // Reset ledger to a fresh state
            ledger = new CompanyLedger();
        }

        public void Shutdown()
        {
            // Clean shutdown
        }

        public double CalculatePassengerFare(float tripDistanceKm)
        {
            return baseBoardingFareRupees + (tripDistanceKm * perKmRateRupees);
        }

        public double CalculateFuelCost(float distanceTraveledKm)
        {
            double litres = distanceTraveledKm / busFuelEconomyKmPerLitre;
            return litres * dieselPricePerLitre;
        }

        public bool RecordTicketSale(string passengerID, string destName, float tripDistKm)
        {
            double fare = CalculatePassengerFare(tripDistKm);
            return ledger.RecordTransaction(TransactionCategory.TicketRevenue, EntryType.Credit, fare, $"Ticket fare collection: {passengerID} to {destName}");
        }

        public bool PayFASTagToll(string tollPlazaName)
        {
            return ledger.RecordTransaction(TransactionCategory.TollPlazaFee, EntryType.Debit, kanchikacherlaTollFeeRupees, $"FASTag Automatic Toll Deduction: {tollPlazaName}");
        }

        public TripFinancialReport SettleTrip(string tripID, string origin, string dest, int passengerCount, float distanceKm, float avgSatisfaction)
        {
            double grossFares = passengerCount * CalculatePassengerFare(distanceKm);
            double fuel = CalculateFuelCost(distanceKm);
            double toll = kanchikacherlaTollFeeRupees;
            double maintenance = distanceKm * 2.10; // ₹2.10/km tyre and brake pad wear
            double driverPay = distanceKm * 1.50 + 350.0; // Base ₹350 + ₹1.50/km

            // Record debit transactions
            ledger.RecordTransaction(TransactionCategory.FuelExpense, EntryType.Debit, fuel, $"Trip fuel replenishment ({distanceKm:F1} km)");
            ledger.RecordTransaction(TransactionCategory.MaintenanceDepreciation, EntryType.Debit, maintenance, $"Tyre & Brake maintenance wear");
            ledger.RecordTransaction(TransactionCategory.DriverSalary, EntryType.Debit, driverPay, $"Driver trip allowance and salary");

            var report = new TripFinancialReport
            {
                tripID = tripID,
                originName = origin,
                destinationName = dest,
                totalPassengersCarried = passengerCount,
                grossTicketRevenueRupees = grossFares,
                dieselFuelExpenseRupees = fuel,
                fastagTollExpenseRupees = toll,
                maintenanceTyreWearRupees = maintenance,
                driverTripAllowanceRupees = driverPay,
                passengerSatisfactionAvg = avgSatisfaction,
                tripDistanceKm = distanceKm
            };

            ledger.AddTripReport(report);
            Debug.Log($"[BUSSIGO Economy] Trip {tripID} Settled: Gross ₹{grossFares:F2}, Net Profit ₹{report.netTripProfitRupees:F2}");
            return report;
        }
    }
}