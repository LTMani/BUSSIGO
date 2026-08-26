using System;
using System.Collections.Generic;

namespace Bussigo.Game.Economy
{
    public class MonthlyFinancialProfitLossStatement04
    {
        public string StatementPeriod => "PERIOD-FY-MONTH-04";
        public float GrossTicketRevenueRupees { get; set; } = 1590000.00f;
        public float CargoFreightRevenueRupees { get; set; } = 233000.00f;
        public float TotalFuelExpensesRupees { get; set; } = 532000.00f;
        public float TotalTollExpensesRupees { get; set; } = 121000.00f;
        public float StaffSalariesAndAllowancesRupees { get; set; } = 340000.00f;
        public float MaintenanceSparesExpensesRupees { get; set; } = 142000.00f;
        public float DepotRentAndUtilitiesRupees { get; set; } = 77000.00f;
        public float CommercialFleetInsuranceRupees { get; set; } = 53000.00f;
        public float BankLoanInterestChargesRupees { get; set; } = 41000.00f;

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
