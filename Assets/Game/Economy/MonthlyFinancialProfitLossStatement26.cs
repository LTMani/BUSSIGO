using System;
using System.Collections.Generic;

namespace Bussigo.Game.Economy
{
    public class MonthlyFinancialProfitLossStatement26
    {
        public string StatementPeriod => "PERIOD-FY-MONTH-26";
        public float GrossTicketRevenueRupees { get; set; } = 3460000.00f;
        public float CargoFreightRevenueRupees { get; set; } = 497000.00f;
        public float TotalFuelExpensesRupees { get; set; } = 1148000.00f;
        public float TotalTollExpensesRupees { get; set; } = 264000.00f;
        public float StaffSalariesAndAllowancesRupees { get; set; } = 670000.00f;
        public float MaintenanceSparesExpensesRupees { get; set; } = 318000.00f;
        public float DepotRentAndUtilitiesRupees { get; set; } = 143000.00f;
        public float CommercialFleetInsuranceRupees { get; set; } = 97000.00f;
        public float BankLoanInterestChargesRupees { get; set; } = 74000.00f;

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
