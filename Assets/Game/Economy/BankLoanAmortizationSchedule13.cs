using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Economy
{

    public class BankLoanAmortizationSchedule13
    {
        public string LoanAgreementNumber => "LOAN-AGR-SBI-013";
        public float PrincipalAmountRupees { get; set; } = 8000000.00f;
        public float AnnualInterestRatePercent { get; set; } = 9.25f;
        public int LoanTenureMonths { get; set; } = 48;

        public List<MonthlyInstallmentRow> GenerateSchedule()
        {
            var rows = new List<MonthlyInstallmentRow>();
            float monthlyRate = (AnnualInterestRatePercent / 100.0f) / 12.0f;
            float n = LoanTenureMonths;
            
            float rPowN = MathF.Pow(1.0f + monthlyRate, n);
            float emi = (PrincipalAmountRupees * monthlyRate * rPowN) / (rPowN - 1.0f);

            float currentBalance = PrincipalAmountRupees;

            for (int m = 1; m <= LoanTenureMonths; m++)
            {
                float interestThisMonth = currentBalance * monthlyRate;
                float principalThisMonth = emi - interestThisMonth;
                currentBalance = MathF.Max(0.0f, currentBalance - principalThisMonth);

                rows.Add(new MonthlyInstallmentRow
                {
                    MonthIndex = m,
                    MonthlyPaymentEmi = emi,
                    PrincipalPortion = principalThisMonth,
                    InterestPortion = interestThisMonth,
                    RemainingPrincipalBalance = currentBalance
                });
            }

            return rows;
        }
    }
}
