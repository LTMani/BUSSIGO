using System;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using Bussigo.Economy;
using Bussigo.Company;
using Bussigo.Save;

namespace Bussigo.Tests.EditMode
{
    [TestFixture]
    public class TycoonAndPersistenceTests
    {
        [Test]
        public void CompanyLedger_RecordsDoubleEntryTransactionsCorrectly()
        {
            var ledger = new CompanyLedger();
            ledger.currentBalanceRupees = 10000.0;

            // Credit Transaction (Ticket Revenue)
            bool creditSuccess = ledger.RecordTransaction(TransactionCategory.TicketRevenue, EntryType.Credit, 5500.0, "Trip Fares");
            Assert.IsTrue(creditSuccess);
            Assert.AreEqual(15500.0, ledger.currentBalanceRupees);
            Assert.AreEqual(5500.0, ledger.lifetimeRevenueRupees);

            // Debit Transaction (Diesel Fuel)
            bool debitSuccess = ledger.RecordTransaction(TransactionCategory.FuelExpense, EntryType.Debit, 3200.0, "Diesel Refuel");
            Assert.IsTrue(debitSuccess);
            Assert.AreEqual(12300.0, ledger.currentBalanceRupees);
            Assert.AreEqual(3200.0, ledger.lifetimeExpensesRupees);

            // Insufficient Funds Rejection
            bool overdrawSuccess = ledger.RecordTransaction(TransactionCategory.FleetPurchase, EntryType.Debit, 50000.0, "New Bus Purchase");
            Assert.IsFalse(overdrawSuccess);
            Assert.AreEqual(12300.0, ledger.currentBalanceRupees); // Balance unchanged
        }

        [Test]
        public void EconomyManager_SettlesCompleteTripWithProfit()
        {
            var go = new GameObject("TestEconomy");
            var economy = go.AddComponent<EconomyManager>();
            economy.Initialize();

            // Settle 274.85 km trip with 40 passengers
            var report = economy.SettleTrip("TRIP_001", "Vijayawada PNBS", "Hyderabad MGBS", 40, 274.85f, 95.0f);

            Assert.IsNotNull(report);
            Assert.Greater(report.grossTicketRevenueRupees, 20000.0);
            Assert.Greater(report.netTripProfitRupees, 5000.0);
            Assert.AreEqual(40, report.totalPassengersCarried);

            GameObject.DestroyImmediate(go);
        }

        [Test]
        public void SaveSystem_SerializesAndRestoresGameState()
        {
            var goComp = new GameObject("TestCompany");
            var company = goComp.AddComponent<CompanyManager>();
            company.Initialize();
            company.companyName = "Royal Express Tests";
            company.companyLevel = 3;

            var goEcon = new GameObject("TestEconomy");
            var economy = goEcon.AddComponent<EconomyManager>();
            economy.Initialize();
            economy.ledger.currentBalanceRupees = 485000.0;

            var goSave = new GameObject("TestSave");
            var saveSys = goSave.AddComponent<SaveSystem>();
            saveSys.Initialize();

            // 1. Save
            bool saved = saveSys.SaveGame(company, economy);
            Assert.IsTrue(saved);

            // 2. Modify in-memory state
            company.companyName = "Modified";
            economy.ledger.currentBalanceRupees = 0.0;

            // 3. Load
            bool loaded = saveSys.LoadGame(company, economy);
            Assert.IsTrue(loaded);

            // 4. Verify restored
            Assert.AreEqual("Royal Express Tests", company.companyName);
            Assert.AreEqual(485000.0, economy.ledger.currentBalanceRupees);

            GameObject.DestroyImmediate(goComp);
            GameObject.DestroyImmediate(goEcon);
            GameObject.DestroyImmediate(goSave);
        }
    }
}
