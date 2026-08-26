using System;
using System.Collections.Generic;

namespace Bussigo.Game.Economy
{
    public class MonthlyFinancialProfitLossStatement12
    {
        public string StatementPeriod => "PERIOD-FY-MONTH-12";
        public float GrossTicketRevenueRupees { get; set; } = 2270000.00f;
        public float CargoFreightRevenueRupees { get; set; } = 329000.00f;
        public float TotalFuelExpensesRupees { get; set; } = 756000.00f;
        public float TotalTollExpensesRupees { get; set; } = 173000.00f;
        public float StaffSalariesAndAllowancesRupees { get; set; } = 460000.00f;
        public float MaintenanceSparesExpensesRupees { get; set; } = 206000.00f;
        public float DepotRentAndUtilitiesRupees { get; set; } = 101000.00f;
        public float CommercialFleetInsuranceRupees { get; set; } = 69000.00f;
        public float BankLoanInterestChargesRupees { get; set; } = 53000.00f;

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
