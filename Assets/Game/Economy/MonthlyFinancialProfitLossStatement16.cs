using System;
using System.Collections.Generic;

namespace Bussigo.Game.Economy
{
    public class MonthlyFinancialProfitLossStatement16
    {
        public string StatementPeriod => "PERIOD-FY-MONTH-16";
        public float GrossTicketRevenueRupees { get; set; } = 2610000.00f;
        public float CargoFreightRevenueRupees { get; set; } = 377000.00f;
        public float TotalFuelExpensesRupees { get; set; } = 868000.00f;
        public float TotalTollExpensesRupees { get; set; } = 199000.00f;
        public float StaffSalariesAndAllowancesRupees { get; set; } = 520000.00f;
        public float MaintenanceSparesExpensesRupees { get; set; } = 238000.00f;
        public float DepotRentAndUtilitiesRupees { get; set; } = 113000.00f;
        public float CommercialFleetInsuranceRupees { get; set; } = 77000.00f;
        public float BankLoanInterestChargesRupees { get; set; } = 59000.00f;

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
