using System;
using System.Collections.Generic;

namespace Bussigo.Game.Economy
{
    public class MonthlyFinancialProfitLossStatement17
    {
        public string StatementPeriod => "PERIOD-FY-MONTH-17";
        public float GrossTicketRevenueRupees { get; set; } = 2695000.00f;
        public float CargoFreightRevenueRupees { get; set; } = 389000.00f;
        public float TotalFuelExpensesRupees { get; set; } = 896000.00f;
        public float TotalTollExpensesRupees { get; set; } = 205500.00f;
        public float StaffSalariesAndAllowancesRupees { get; set; } = 535000.00f;
        public float MaintenanceSparesExpensesRupees { get; set; } = 246000.00f;
        public float DepotRentAndUtilitiesRupees { get; set; } = 116000.00f;
        public float CommercialFleetInsuranceRupees { get; set; } = 79000.00f;
        public float BankLoanInterestChargesRupees { get; set; } = 60500.00f;

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
