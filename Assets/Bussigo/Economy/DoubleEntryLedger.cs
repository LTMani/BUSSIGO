using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bussigo.Economy
{
    [Serializable]
    public struct LedgerEntry
    {
        public string timestamp;
        public string description;
        public int creditINR;
        public int debitINR;
        public int runningBalanceINR;
    }

    public class DoubleEntryLedger : MonoBehaviour
    {
        public int currentBalanceINR = 500000;
        public List<LedgerEntry> transactionHistory = new List<LedgerEntry>();

        public void RecordTransaction(string description, int credit, int debit)
        {
            currentBalanceINR = currentBalanceINR + credit - debit;
            transactionHistory.Add(new LedgerEntry
            {
                timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                description = description,
                creditINR = credit,
                debitINR = debit,
                runningBalanceINR = currentBalanceINR
            });
        }
    }
}
