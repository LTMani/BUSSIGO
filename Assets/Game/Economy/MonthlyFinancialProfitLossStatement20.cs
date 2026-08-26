using System;
using System.Collections.Generic;

namespace Bussigo.Game.Economy
{
    public class MonthlyFinancialProfitLossStatement20
    {
        public string StatementPeriod => "PERIOD-FY-MONTH-20";
        public float GrossTicketRevenueRupees { get; set; } = 2950000.00f;
        public float CargoFreightRevenueRupees { get; set; } = 425000.00f;
        public float TotalFuelExpensesRupees { get; set; } = 980000.00f;
        public float TotalTollExpensesRupees { get; set; } = 225000.00f;
        public float StaffSalariesAndAllowancesRupees { get; set; } = 580000.00f;
        public float MaintenanceSparesExpensesRupees { get; set; } = 270000.00f;
        public float DepotRentAndUtilitiesRupees { get; set; } = 125000.00f;
        public float CommercialFleetInsuranceRupees { get; set; } = 85000.00f;
        public float BankLoanInterestChargesRupees { get; set; } = 65000.00f;

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
