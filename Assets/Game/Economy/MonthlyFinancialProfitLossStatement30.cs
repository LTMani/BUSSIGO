using System;
using System.Collections.Generic;

namespace Bussigo.Game.Economy
{
    public class MonthlyFinancialProfitLossStatement30
    {
        public string StatementPeriod => "PERIOD-FY-MONTH-30";
        public float GrossTicketRevenueRupees { get; set; } = 3800000.00f;
        public float CargoFreightRevenueRupees { get; set; } = 545000.00f;
        public float TotalFuelExpensesRupees { get; set; } = 1260000.00f;
        public float TotalTollExpensesRupees { get; set; } = 290000.00f;
        public float StaffSalariesAndAllowancesRupees { get; set; } = 730000.00f;
        public float MaintenanceSparesExpensesRupees { get; set; } = 350000.00f;
        public float DepotRentAndUtilitiesRupees { get; set; } = 155000.00f;
        public float CommercialFleetInsuranceRupees { get; set; } = 105000.00f;
        public float BankLoanInterestChargesRupees { get; set; } = 80000.00f;

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
