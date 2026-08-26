using System;
using System.Collections.Generic;

namespace Bussigo.Game.Economy
{
    public enum AccountCategory
    {
        AssetCurrent,
        AssetFixedEquipment,
        LiabilityCurrent,
        LiabilityLongTermDebt,
        EquityShareCapital,
        RevenueOperating,
        RevenueNonOperating,
        ExpenseDirectOperating,
        ExpenseAdministrativeOverhead,
        ExpenseTaxesAndDuties
    }

    public class LedgerAccount
    {
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public AccountCategory Category { get; set; }
        public float DebitBalance { get; set; }
        public float CreditBalance { get; set; }

        public float NetBalance => (Category == AccountCategory.AssetCurrent || 
                                    Category == AccountCategory.AssetFixedEquipment || 
                                    Category == AccountCategory.ExpenseDirectOperating || 
                                    Category == AccountCategory.ExpenseAdministrativeOverhead || 
                                    Category == AccountCategory.ExpenseTaxesAndDuties) 
                                    ? (DebitBalance - CreditBalance) 
                                    : (CreditBalance - DebitBalance);
    }

    public static class GeneralLedgerChartOfAccounts
    {
        public static Dictionary<string, LedgerAccount> Accounts { get; } = new Dictionary<string, LedgerAccount>();

        static GeneralLedgerChartOfAccounts()
        {
            RegisterAccounts();
        }

        private static void RegisterAccounts()
        {
            // Assets
            AddAccount("1010", "Cash at Commercial Bank", AccountCategory.AssetCurrent);
            AddAccount("1020", "FASTag Toll Electronic Wallet", AccountCategory.AssetCurrent);
            AddAccount("1030", "Diesel Fuel Bulk Inventory", AccountCategory.AssetCurrent);
            AddAccount("1040", "Spare Parts & Tyre Inventory", AccountCategory.AssetCurrent);
            AddAccount("1510", "Bus Fleet Capital Assets", AccountCategory.AssetFixedEquipment);
            AddAccount("1520", "Depot Land & Buildings", AccountCategory.AssetFixedEquipment);
            AddAccount("1530", "Workshop Machinery & Hydraulic Lifts", AccountCategory.AssetFixedEquipment);
            AddAccount("1590", "Accumulated Fleet Depreciation", AccountCategory.AssetFixedEquipment);

            // Liabilities
            AddAccount("2010", "Accounts Payable Trade Spares", AccountCategory.LiabilityCurrent);
            AddAccount("2020", "Driver Wages Payable", AccountCategory.LiabilityCurrent);
            AddAccount("2030", "GST Tax Output Liability", AccountCategory.LiabilityCurrent);
            AddAccount("2510", "Commercial Bank Fleet Loan", AccountCategory.LiabilityLongTermDebt);
            AddAccount("2520", "Depot Mortgage Facility", AccountCategory.LiabilityLongTermDebt);

            // Equity
            AddAccount("3010", "Founder Capital Equity", AccountCategory.EquityShareCapital);
            AddAccount("3020", "Retained Earnings", AccountCategory.EquityShareCapital);

            // Operating Revenues
            AddAccount("4010", "Passenger Ticket Revenue Ordinary", AccountCategory.RevenueOperating);
            AddAccount("4020", "Passenger Ticket Revenue Express", AccountCategory.RevenueOperating);
            AddAccount("4030", "Passenger Ticket Revenue Super Luxury", AccountCategory.RevenueOperating);
            AddAccount("4040", "Passenger Ticket Revenue Garuda AC", AccountCategory.RevenueOperating);
            AddAccount("4050", "Passenger Ticket Revenue Vennela Sleeper", AccountCategory.RevenueOperating);
            AddAccount("4110", "Luggage Cargo Freight Tariff", AccountCategory.RevenueOperating);
            AddAccount("4120", "Special Festival Surge Surcharge", AccountCategory.RevenueOperating);

            // Direct Operating Expenses
            AddAccount("5010", "High Speed Diesel Consumption", AccountCategory.ExpenseDirectOperating);
            AddAccount("5020", "AdBlue / DEF Exhaust Fluid Consumption", AccountCategory.ExpenseDirectOperating);
            AddAccount("5030", "National Highway Toll Fees FASTag", AccountCategory.ExpenseDirectOperating);
            AddAccount("5040", "Driver Trip Allowances & Wages", AccountCategory.ExpenseDirectOperating);
            AddAccount("5050", "Conductor Payroll & Commission", AccountCategory.ExpenseDirectOperating);
            AddAccount("5110", "Tyre Replacement & Retreading Spares", AccountCategory.ExpenseDirectOperating);
            AddAccount("5120", "Brake Pad & Drum Lining Spares", AccountCategory.ExpenseDirectOperating);
            AddAccount("5130", "Engine Oil Flush & Lubricants", AccountCategory.ExpenseDirectOperating);
            AddAccount("5140", "Clutch Plate Overhaul Spares", AccountCategory.ExpenseDirectOperating);
            AddAccount("5150", "Air Suspension Bellow Repairs", AccountCategory.ExpenseDirectOperating);

            // Administrative Overheads
            AddAccount("6010", "Depot Electricity & Water Utilities", AccountCategory.ExpenseAdministrativeOverhead);
            AddAccount("6020", "Depot Maintenance Shed Rent", AccountCategory.ExpenseAdministrativeOverhead);
            AddAccount("6030", "Commercial Fleet Comprehensive Insurance", AccountCategory.ExpenseAdministrativeOverhead);
            AddAccount("6040", "Driver Training & CDL Certification", AccountCategory.ExpenseAdministrativeOverhead);
            AddAccount("6050", "Passenger Refreshment Amenities", AccountCategory.ExpenseAdministrativeOverhead);
            AddAccount("6510", "Bank Loan Finance Interest Charges", AccountCategory.ExpenseAdministrativeOverhead);
            AddAccount("6520", "Monthly Fleet Depreciation Expense", AccountCategory.ExpenseAdministrativeOverhead);
        }

        private static void AddAccount(string code, string name, AccountCategory cat)
        {
            Accounts[code] = new LedgerAccount { AccountCode = code, AccountName = name, Category = cat };
        }
    }
}
