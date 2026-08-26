using System;
using System.Collections.Generic;
using Bussigo.Game.Economy;

namespace Bussigo.Game.Store
{
    public class SyntheticBillingTransactionRecord02
    {
        public string TransactionToken => "SYNTH-TX-STORE-0002";
        public string PackageSku { get; set; } = "SKU_COINS_BUNDLE_02";
        public long CoinsGrantedAmount { get; set; } = 150000;
        public bool IsTransactionValidated { get; private set; } = false;

        public bool ProcessSyntheticPurchase(FinancialLedger ledger)
        {
            IsTransactionValidated = true;
            return ledger.RecordTransaction(TransactionType.TicketRevenue, CoinsGrantedAmount, "In-App Mock Store Coin Package Grant " + PackageSku);
        }
    }
}
