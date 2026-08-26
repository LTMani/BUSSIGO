using System;
using Bussigo.Game.Core;
using Bussigo.Game.Vehicles;
using Bussigo.Game.VehiclePhysics;
using Bussigo.Game.Economy;

namespace Bussigo.Tests.EditMode
{
    public static class DeepSubsystemAssertionSuite27
    {
        public static void RunSuite()
        {
            TestAxleLoadTransfer();
            TestBSFCFuelEfficiency();
            TestProfitLossCalculation();
        }

        public static void TestAxleLoadTransfer()
        {
            var solver = new AxleLoadTransferSolver();
            var (fLoad, rLoad) = solver.CalculateLongitudinalLoadTransfer(15000f, 2.5f, 0.0f);

            if (fLoad <= 0.0f || rLoad <= 0.0f)
                throw new Exception("Axle loads must be positive during normal acceleration.");
            if (rLoad <= fLoad)
                throw new Exception("Rear axle load must increase under forward acceleration.");
        }

        public static void TestBSFCFuelEfficiency()
        {
            var bsfcMap = new EngineFuelEfficiencyBSFCMap01();
            float flowLph = bsfcMap.CalculateInstantaneousDieselFlowRateLph(1400f, 850f, 1100f);
            if (flowLph < 10.0f || flowLph > 60.0f)
                throw new Exception("Diesel flow rate outside realistic heavy commercial envelope.");
        }

        public static void TestProfitLossCalculation()
        {
            var pnl = new MonthlyFinancialProfitLossStatement01();
            float profit = pnl.CalculateNetOperatingProfit();
            if (profit <= 0.0f)
                throw new Exception("Net operating profit expected to be positive for standard commercial schedule.");
        }
    }
}
