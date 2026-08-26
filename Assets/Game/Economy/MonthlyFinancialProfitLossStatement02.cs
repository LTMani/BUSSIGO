using System;
using System.Collections.Generic;

namespace Bussigo.Game.Economy
{
    public class MonthlyFinancialProfitLossStatement02
    {
        public string StatementPeriod => "PERIOD-FY-MONTH-02";
        public float GrossTicketRevenueRupees { get; set; } = 1420000.00f;
        public float CargoFreightRevenueRupees { get; set; } = 209000.00f;
        public float TotalFuelExpensesRupees { get; set; } = 476000.00f;
        public float TotalTollExpensesRupees { get; set; } = 108000.00f;
        public float StaffSalariesAndAllowancesRupees { get; set; } = 310000.00f;
        public float MaintenanceSparesExpensesRupees { get; set; } = 126000.00f;
        public float DepotRentAndUtilitiesRupees { get; set; } = 71000.00f;
        public float CommercialFleetInsuranceRupees { get; set; } = 49000.00f;
        public float BankLoanInterestChargesRupees { get; set; } = 38000.00f;

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
