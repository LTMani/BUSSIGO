using System;
using UnityEngine;

namespace Bussigo.Economy
{
    public enum TransactionCategory
    {
        TicketRevenue = 0,
        FuelExpense = 1,
        TollPlazaFee = 2,
        MaintenanceDepreciation = 3,
        DriverSalary = 4,
        TerminalParkingFee = 5,
        FleetPurchase = 6
    }

    public enum EntryType
    {
        Credit = 0, // Inflow / Revenue (+)
        Debit = 1   // Outflow / Expense (-)
    }

    [Serializable]
    public class LedgerEntry
    {
        public string transactionID;
        public TransactionCategory category;
        public EntryType entryType;
        public double amountRupees;
        public string timestampIso;
        public string description;
        public string routeSegmentID;

        public LedgerEntry() { }

        public LedgerEntry(string txId, TransactionCategory cat, EntryType type, double amount, string desc, string segId = "")
        {
            transactionID = txId;
            category = cat;
            entryType = type;
            amountRupees = amount;
            timestampIso = DateTime.UtcNow.ToString("o");
            description = desc;
            routeSegmentID = segId;
        }
    }
}
