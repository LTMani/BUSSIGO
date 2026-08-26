using System;
using System.Collections.Generic;

namespace Bussigo.Game.Economy
{
    public class MonthlyFinancialProfitLossStatement05
    {
        public string StatementPeriod => "PERIOD-FY-MONTH-05";
        public float GrossTicketRevenueRupees { get; set; } = 1675000.00f;
        public float CargoFreightRevenueRupees { get; set; } = 245000.00f;
        public float TotalFuelExpensesRupees { get; set; } = 560000.00f;
        public float TotalTollExpensesRupees { get; set; } = 127500.00f;
        public float StaffSalariesAndAllowancesRupees { get; set; } = 355000.00f;
        public float MaintenanceSparesExpensesRupees { get; set; } = 150000.00f;
        public float DepotRentAndUtilitiesRupees { get; set; } = 80000.00f;
        public float CommercialFleetInsuranceRupees { get; set; } = 55000.00f;
        public float BankLoanInterestChargesRupees { get; set; } = 42500.00f;

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
