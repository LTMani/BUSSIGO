using System;
using System.Collections.Generic;

namespace Bussigo.Game.Economy
{
    public class MonthlyFinancialProfitLossStatement21
    {
        public string StatementPeriod => "PERIOD-FY-MONTH-21";
        public float GrossTicketRevenueRupees { get; set; } = 3035000.00f;
        public float CargoFreightRevenueRupees { get; set; } = 437000.00f;
        public float TotalFuelExpensesRupees { get; set; } = 1008000.00f;
        public float TotalTollExpensesRupees { get; set; } = 231500.00f;
        public float StaffSalariesAndAllowancesRupees { get; set; } = 595000.00f;
        public float MaintenanceSparesExpensesRupees { get; set; } = 278000.00f;
        public float DepotRentAndUtilitiesRupees { get; set; } = 128000.00f;
        public float CommercialFleetInsuranceRupees { get; set; } = 87000.00f;
        public float BankLoanInterestChargesRupees { get; set; } = 66500.00f;

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
