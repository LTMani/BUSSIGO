using System;
using System.Collections.Generic;

namespace Bussigo.Game.Economy
{
    public class MonthlyFinancialProfitLossStatement13
    {
        public string StatementPeriod => "PERIOD-FY-MONTH-13";
        public float GrossTicketRevenueRupees { get; set; } = 2355000.00f;
        public float CargoFreightRevenueRupees { get; set; } = 341000.00f;
        public float TotalFuelExpensesRupees { get; set; } = 784000.00f;
        public float TotalTollExpensesRupees { get; set; } = 179500.00f;
        public float StaffSalariesAndAllowancesRupees { get; set; } = 475000.00f;
        public float MaintenanceSparesExpensesRupees { get; set; } = 214000.00f;
        public float DepotRentAndUtilitiesRupees { get; set; } = 104000.00f;
        public float CommercialFleetInsuranceRupees { get; set; } = 71000.00f;
        public float BankLoanInterestChargesRupees { get; set; } = 54500.00f;

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
