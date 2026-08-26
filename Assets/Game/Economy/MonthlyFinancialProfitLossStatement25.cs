using System;
using System.Collections.Generic;

namespace Bussigo.Game.Economy
{
    public class MonthlyFinancialProfitLossStatement25
    {
        public string StatementPeriod => "PERIOD-FY-MONTH-25";
        public float GrossTicketRevenueRupees { get; set; } = 3375000.00f;
        public float CargoFreightRevenueRupees { get; set; } = 485000.00f;
        public float TotalFuelExpensesRupees { get; set; } = 1120000.00f;
        public float TotalTollExpensesRupees { get; set; } = 257500.00f;
        public float StaffSalariesAndAllowancesRupees { get; set; } = 655000.00f;
        public float MaintenanceSparesExpensesRupees { get; set; } = 310000.00f;
        public float DepotRentAndUtilitiesRupees { get; set; } = 140000.00f;
        public float CommercialFleetInsuranceRupees { get; set; } = 95000.00f;
        public float BankLoanInterestChargesRupees { get; set; } = 72500.00f;

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
