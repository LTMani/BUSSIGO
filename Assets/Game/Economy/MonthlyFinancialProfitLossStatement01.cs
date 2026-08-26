using System;
using System.Collections.Generic;

namespace Bussigo.Game.Economy
{
    public class MonthlyFinancialProfitLossStatement01
    {
        public string StatementPeriod => "PERIOD-FY-MONTH-01";
        public float GrossTicketRevenueRupees { get; set; } = 1335000.00f;
        public float CargoFreightRevenueRupees { get; set; } = 197000.00f;
        public float TotalFuelExpensesRupees { get; set; } = 448000.00f;
        public float TotalTollExpensesRupees { get; set; } = 101500.00f;
        public float StaffSalariesAndAllowancesRupees { get; set; } = 295000.00f;
        public float MaintenanceSparesExpensesRupees { get; set; } = 118000.00f;
        public float DepotRentAndUtilitiesRupees { get; set; } = 68000.00f;
        public float CommercialFleetInsuranceRupees { get; set; } = 47000.00f;
        public float BankLoanInterestChargesRupees { get; set; } = 36500.00f;

        public float CalculateNetOperatingProfit()
        {
            float totalRevenue = GrossTicketRevenueRupees + CargoFreightRevenueRupees;
            float totalExpenses = TotalFuelExpensesRupees + TotalTollExpensesRupees + StaffSalariesAndAllowancesRupees +
                                  MaintenanceSparesExpensesRupees + DepotRentAndUtilitiesRupees +
                                  CommercialFleetInsuranceRupees + BankLoanInterestChargesRupees;
            return totalRevenue - totalExpenses;
        }

        public float CalculateOperatingProfitMarginPercent()
        {
            float totalRevenue = GrossTicketRevenueRupees + CargoFreightRevenueRupees;
            if (totalRevenue <= 1.0f) return 0.0f;
            return (CalculateNetOperatingProfit() / totalRevenue) * 100.0f;
        }
    }
}
