using System;
using System.Collections.Generic;

namespace Bussigo.Game.Economy
{
    public class MonthlyFinancialProfitLossStatement18
    {
        public string StatementPeriod => "PERIOD-FY-MONTH-18";
        public float GrossTicketRevenueRupees { get; set; } = 2780000.00f;
        public float CargoFreightRevenueRupees { get; set; } = 401000.00f;
        public float TotalFuelExpensesRupees { get; set; } = 924000.00f;
        public float TotalTollExpensesRupees { get; set; } = 212000.00f;
        public float StaffSalariesAndAllowancesRupees { get; set; } = 550000.00f;
        public float MaintenanceSparesExpensesRupees { get; set; } = 254000.00f;
        public float DepotRentAndUtilitiesRupees { get; set; } = 119000.00f;
        public float CommercialFleetInsuranceRupees { get; set; } = 81000.00f;
        public float BankLoanInterestChargesRupees { get; set; } = 62000.00f;

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
