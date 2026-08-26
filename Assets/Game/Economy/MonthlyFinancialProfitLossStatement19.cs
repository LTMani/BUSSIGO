using System;
using System.Collections.Generic;

namespace Bussigo.Game.Economy
{
    public class MonthlyFinancialProfitLossStatement19
    {
        public string StatementPeriod => "PERIOD-FY-MONTH-19";
        public float GrossTicketRevenueRupees { get; set; } = 2865000.00f;
        public float CargoFreightRevenueRupees { get; set; } = 413000.00f;
        public float TotalFuelExpensesRupees { get; set; } = 952000.00f;
        public float TotalTollExpensesRupees { get; set; } = 218500.00f;
        public float StaffSalariesAndAllowancesRupees { get; set; } = 565000.00f;
        public float MaintenanceSparesExpensesRupees { get; set; } = 262000.00f;
        public float DepotRentAndUtilitiesRupees { get; set; } = 122000.00f;
        public float CommercialFleetInsuranceRupees { get; set; } = 83000.00f;
        public float BankLoanInterestChargesRupees { get; set; } = 63500.00f;

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
