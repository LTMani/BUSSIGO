using System;
using System.Collections.Generic;

namespace Bussigo.Game.Economy
{
    public class MonthlyFinancialProfitLossStatement29
    {
        public string StatementPeriod => "PERIOD-FY-MONTH-29";
        public float GrossTicketRevenueRupees { get; set; } = 3715000.00f;
        public float CargoFreightRevenueRupees { get; set; } = 533000.00f;
        public float TotalFuelExpensesRupees { get; set; } = 1232000.00f;
        public float TotalTollExpensesRupees { get; set; } = 283500.00f;
        public float StaffSalariesAndAllowancesRupees { get; set; } = 715000.00f;
        public float MaintenanceSparesExpensesRupees { get; set; } = 342000.00f;
        public float DepotRentAndUtilitiesRupees { get; set; } = 152000.00f;
        public float CommercialFleetInsuranceRupees { get; set; } = 103000.00f;
        public float BankLoanInterestChargesRupees { get; set; } = 78500.00f;

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
