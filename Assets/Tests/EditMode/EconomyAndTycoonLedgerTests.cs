using System;
using Bussigo.Game.Economy;

namespace Bussigo.Tests.EditMode
{
    public static class EconomyAndTycoonLedgerTests
    {
        public static void RunAllTests()
        {
            TestFinancialLedgerTransactions();
            TestInsufficientFundsRejection();
        }

        public static void TestFinancialLedgerTransactions()
        {
            var ledger = new FinancialLedger();
            float initialBalance = ledger.CurrentBalanceInRupees;

            bool success = ledger.RecordTransaction(TransactionType.TicketRevenue, 12500f, "Vijayawada-Hyderabad morning run");
            if (!success) throw new Exception("Revenue transaction failed.");
            if (ledger.CurrentBalanceInRupees != initialBalance + 12500f)
                throw new Exception("Balance did not reflect ticket revenue.");
        }

        public static void TestInsufficientFundsRejection()
        {
            var ledger = new FinancialLedger();
            bool success = ledger.RecordTransaction(TransactionType.VehiclePurchaseExpense, 99999999f, "Ultra luxury fleet purchase");
            if (success) throw new Exception("Should not allow expense exceeding current cash balance.");
        }
    }
}
