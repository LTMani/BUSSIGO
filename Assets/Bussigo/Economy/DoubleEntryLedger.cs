using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bussigo.Economy
{
    [Serializable]
    public struct DoubleEntryRecord
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
        public List<DoubleEntryRecord> transactionHistory = new List<DoubleEntryRecord>();

        public void RecordTransaction(string description, int credit, int debit)
        {
            currentBalanceINR = currentBalanceINR + credit - debit;
            transactionHistory.Add(new DoubleEntryRecord
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
