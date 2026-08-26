using System;
using System.Collections.Generic;

namespace Bussigo.Game.Economy
{
    public class MonthlyFinancialProfitLossStatement23
    {
        public string StatementPeriod => "PERIOD-FY-MONTH-23";
        public float GrossTicketRevenueRupees { get; set; } = 3205000.00f;
        public float CargoFreightRevenueRupees { get; set; } = 461000.00f;
        public float TotalFuelExpensesRupees { get; set; } = 1064000.00f;
        public float TotalTollExpensesRupees { get; set; } = 244500.00f;
        public float StaffSalariesAndAllowancesRupees { get; set; } = 625000.00f;
        public float MaintenanceSparesExpensesRupees { get; set; } = 294000.00f;
        public float DepotRentAndUtilitiesRupees { get; set; } = 134000.00f;
        public float CommercialFleetInsuranceRupees { get; set; } = 91000.00f;
        public float BankLoanInterestChargesRupees { get; set; } = 69500.00f;

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
