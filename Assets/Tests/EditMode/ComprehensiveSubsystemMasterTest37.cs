using System;
using Bussigo.Game.Core;
using Bussigo.Game.Analytics;
using Bussigo.Game.Store;
using Bussigo.Game.World;
using Bussigo.Game.Audio;
using Bussigo.Game.Economy;

namespace Bussigo.Tests.EditMode
{
    public static class ComprehensiveSubsystemMasterTest37
    {
        public static void RunAllTests()
        {
            TestTelemetryEventBatching();
            TestPhotocellLightingSwitch();
            TestSyntheticBillingPurchase();
        }

        public static void TestTelemetryEventBatching()
        {
            var batch = new TelemetrySessionEventBatch01();
            batch.RecordTelemetryEvent("TripStarted", 100f, "Vijayawada-Hyderabad");
            if (batch.GetPendingEventCount() != 1)
                throw new Exception("Telemetry event batch failed to record entry.");
        }

        public static void TestPhotocellLightingSwitch()
        {
            var light = new HighwayHighMastLightingController01();
            light.EvaluatePhotocellSensor(-10.0f);
            if (!light.IsPhotocellNightActive)
                throw new Exception("High mast lighting photocell failed to activate at night.");
        }

        public static void TestSyntheticBillingPurchase()
        {
            var ledger = new FinancialLedger();
            var tx = new SyntheticBillingTransactionRecord01();
            bool success = tx.ProcessSyntheticPurchase(ledger);
            if (!success || !tx.IsTransactionValidated)
                throw new Exception("Synthetic billing transaction failed.");
        }
    }
}
