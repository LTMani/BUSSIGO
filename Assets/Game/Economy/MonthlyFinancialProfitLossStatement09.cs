using System;
using System.Collections.Generic;

namespace Bussigo.Game.Economy
{
    public class MonthlyFinancialProfitLossStatement09
    {
        public string StatementPeriod => "PERIOD-FY-MONTH-09";
        public float GrossTicketRevenueRupees { get; set; } = 2015000.00f;
        public float CargoFreightRevenueRupees { get; set; } = 293000.00f;
        public float TotalFuelExpensesRupees { get; set; } = 672000.00f;
        public float TotalTollExpensesRupees { get; set; } = 153500.00f;
        public float StaffSalariesAndAllowancesRupees { get; set; } = 415000.00f;
        public float MaintenanceSparesExpensesRupees { get; set; } = 182000.00f;
        public float DepotRentAndUtilitiesRupees { get; set; } = 92000.00f;
        public float CommercialFleetInsuranceRupees { get; set; } = 63000.00f;
        public float BankLoanInterestChargesRupees { get; set; } = 48500.00f;

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
