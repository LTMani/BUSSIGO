using System;
using System.Collections.Generic;

namespace Bussigo.Game.Economy
{
    public enum TransactionType
    {
        TicketRevenue,
        ParcelFreightRevenue,
        FuelExpense,
        TollFeeExpense,
        DriverWageExpense,
        VehicleMaintenanceExpense,
        DepotUpkeepExpense,
        LoanPaymentExpense,
        VehiclePurchaseExpense,
        VehicleSaleRevenue
    }

    public class LedgerEntry
    {
        public string TransactionId { get; set; }
        public DateTime Timestamp { get; set; }
        public TransactionType Type { get; set; }
        public float AmountInRupees { get; set; }
        public string Description { get; set; }
        public float ResultingBalanceInRupees { get; set; }
    }

    public class FinancialLedger
    {
        public float CurrentBalanceInRupees { get; private set; } = 500000.0f; // Starting capital ₹5,00,000
        public List<LedgerEntry> Transactions { get; } = new List<LedgerEntry>();

        public event Action<float> OnBalanceChanged;

        public bool RecordTransaction(TransactionType type, float amount, string description)
        {
            if (amount < 0.0f) return false;

            bool isIncome = type == TransactionType.TicketRevenue || 
                            type == TransactionType.ParcelFreightRevenue || 
                            type == TransactionType.VehicleSaleRevenue;

            if (!isIncome && CurrentBalanceInRupees < amount)
            {
                return false; // Insufficient funds
            }

            CurrentBalanceInRupees += isIncome ? amount : -amount;

            var entry = new LedgerEntry
            {
                TransactionId = Guid.NewGuid().ToString("N").Substring(0, 8),
                Timestamp = DateTime.UtcNow,
                Type = type,
                AmountInRupees = amount,
                Description = description,
                ResultingBalanceInRupees = CurrentBalanceInRupees
            };

            Transactions.Add(entry);
            OnBalanceChanged?.Invoke(CurrentBalanceInRupees);
            return true;
        }
    }
}
