using System;
using System.Collections.Generic;
using Bussigo.Game.Economy;

namespace Bussigo.Game.Store
{
    public class SyntheticBillingTransactionRecord01
    {
        public string TransactionToken => "SYNTH-TX-STORE-0001";
        public string PackageSku { get; set; } = "SKU_COINS_BUNDLE_01";
        public long CoinsGrantedAmount { get; set; } = 125000;
        public bool IsTransactionValidated { get; private set; } = false;

        public bool ProcessSyntheticPurchase(FinancialLedger ledger)
        {
            IsTransactionValidated = true;
            return ledger.RecordTransaction(TransactionType.TicketRevenue, CoinsGrantedAmount, "In-App Mock Store Coin Package Grant " + PackageSku);
        }
    }
}
