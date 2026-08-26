using System;
using System.Collections.Generic;

namespace Bussigo.Game.Economy
{
    public struct JournalEntryLine
    {
        public string AccountCode;
        public string AccountTitle;
        public float DebitAmount;
        public float CreditAmount;
    }

    public class FinancialAccountingJournal24
    {
        public string JournalVoucherNumber => "JV-BUSSIGO-0024";
        public DateTime VoucherDate { get; set; } = DateTime.UtcNow;
        public string Narration { get; set; } = "Operating route fare revenue and highway toll settlement 24";
        public List<JournalEntryLine> Lines { get; } = new List<JournalEntryLine>();

        public void AddDebit(string accountCode, string title, float amount)
        {
            Lines.Add(new JournalEntryLine { AccountCode = accountCode, AccountTitle = title, DebitAmount = amount, CreditAmount = 0.0f });
        }

        public void AddCredit(string accountCode, string title, float amount)
        {
            Lines.Add(new JournalEntryLine { AccountCode = accountCode, AccountTitle = title, DebitAmount = 0.0f, CreditAmount = amount });
        }

        public bool ValidateDoubleEntryBalance()
        {
            float totalDebits = 0.0f;
            float totalCredits = 0.0f;
            foreach (var l in Lines)
            {
                totalDebits += l.DebitAmount;
                totalCredits += l.CreditAmount;
            }
            return MathF.Abs(totalDebits - totalCredits) < 0.01f;
        }
    }
}
