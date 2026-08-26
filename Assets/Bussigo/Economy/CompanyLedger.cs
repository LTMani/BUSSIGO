using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bussigo.Economy
{
    [Serializable]
    public class TripFinancialReport
    {
        public string tripID;
        public string originName;
        public string destinationName;
        public int totalPassengersCarried;
        public double grossTicketRevenueRupees;
        public double dieselFuelExpenseRupees;
        public double fastagTollExpenseRupees;
        public double maintenanceTyreWearRupees;
        public double driverTripAllowanceRupees;
        public double netTripProfitRupees => grossTicketRevenueRupees - (dieselFuelExpenseRupees + fastagTollExpenseRupees + maintenanceTyreWearRupees + driverTripAllowanceRupees);
        public float passengerSatisfactionAvg;
        public float tripDistanceKm;
    }

    [Serializable]
    public class CompanyLedger
    {
        public double currentBalanceRupees = 250000.0; // Initial seed capital (₹2,50,000)
        public double lifetimeRevenueRupees = 0.0;
        public double lifetimeExpensesRupees = 0.0;
        public readonly List<LedgerEntry> transactionHistory = new List<LedgerEntry>();
        public readonly List<TripFinancialReport> completedTripReports = new List<TripFinancialReport>();

        public bool RecordTransaction(TransactionCategory category, EntryType entryType, double amount, string description, string segId = "")
        {
            if (amount <= 0.0) return false;

            if (entryType == EntryType.Debit && currentBalanceRupees < amount)
            {
                Debug.LogWarning($"[BUSSIGO Ledger] Insufficient funds for transaction: {description} (Req: ₹{amount:F2}, Bal: ₹{currentBalanceRupees:F2})");
                return false;
            }

            string txId = $"TX_{DateTime.UtcNow.Ticks}_{transactionHistory.Count + 1}";
            var entry = new LedgerEntry(txId, category, entryType, amount, description, segId);
            transactionHistory.Add(entry);

            if (entryType == EntryType.Credit)
            {
                currentBalanceRupees += amount;
                lifetimeRevenueRupees += amount;
            }
            else
            {
                currentBalanceRupees -= amount;
                lifetimeExpensesRupees += amount;
            }

            return true;
        }

        public void AddTripReport(TripFinancialReport report)
        {
            completedTripReports.Add(report);
        }
    }
}
