using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Economy
{
    public struct MonthlyInstallmentRow
    {
        public int MonthIndex;
        public float MonthlyPaymentEmi;
        public float PrincipalPortion;
        public float InterestPortion;
        public float RemainingPrincipalBalance;
    }

    public class BankLoanAmortizationSchedule07
    {
        public string LoanAgreementNumber => "LOAN-AGR-SBI-007";
        public float PrincipalAmountRupees { get; set; } = 5000000.00f;
        public float AnnualInterestRatePercent { get; set; } = 10.25f;
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
