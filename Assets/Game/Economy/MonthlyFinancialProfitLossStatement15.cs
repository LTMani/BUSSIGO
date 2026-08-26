using System;
using System.Collections.Generic;

namespace Bussigo.Game.Economy
{
    public class MonthlyFinancialProfitLossStatement15
    {
        public string StatementPeriod => "PERIOD-FY-MONTH-15";
        public float GrossTicketRevenueRupees { get; set; } = 2525000.00f;
        public float CargoFreightRevenueRupees { get; set; } = 365000.00f;
        public float TotalFuelExpensesRupees { get; set; } = 840000.00f;
        public float TotalTollExpensesRupees { get; set; } = 192500.00f;
        public float StaffSalariesAndAllowancesRupees { get; set; } = 505000.00f;
        public float MaintenanceSparesExpensesRupees { get; set; } = 230000.00f;
        public float DepotRentAndUtilitiesRupees { get; set; } = 110000.00f;
        public float CommercialFleetInsuranceRupees { get; set; } = 75000.00f;
        public float BankLoanInterestChargesRupees { get; set; } = 57500.00f;

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
