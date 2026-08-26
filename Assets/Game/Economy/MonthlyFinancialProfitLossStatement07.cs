using System;
using System.Collections.Generic;

namespace Bussigo.Game.Economy
{
    public class MonthlyFinancialProfitLossStatement07
    {
        public string StatementPeriod => "PERIOD-FY-MONTH-07";
        public float GrossTicketRevenueRupees { get; set; } = 1845000.00f;
        public float CargoFreightRevenueRupees { get; set; } = 269000.00f;
        public float TotalFuelExpensesRupees { get; set; } = 616000.00f;
        public float TotalTollExpensesRupees { get; set; } = 140500.00f;
        public float StaffSalariesAndAllowancesRupees { get; set; } = 385000.00f;
        public float MaintenanceSparesExpensesRupees { get; set; } = 166000.00f;
        public float DepotRentAndUtilitiesRupees { get; set; } = 86000.00f;
        public float CommercialFleetInsuranceRupees { get; set; } = 59000.00f;
        public float BankLoanInterestChargesRupees { get; set; } = 45500.00f;

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
