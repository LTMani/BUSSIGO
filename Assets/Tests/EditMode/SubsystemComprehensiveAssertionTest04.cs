using System;
using Bussigo.Game.Core;
using Bussigo.Game.Vehicles;
using Bussigo.Game.VehiclePhysics;
using Bussigo.Game.World;
using Bussigo.Game.Fleet;

namespace Bussigo.Tests.EditMode
{
    public static class SubsystemComprehensiveAssertionTest04
    {
        public static void RunAllAssertions()
        {
            TestHairpinCurveSafety();
            TestUsedBusValuation();
        }

        public static void TestHairpinCurveSafety()
        {
            var hairpin = new EasternGhatsHairpinBendSafetyModel01();
            var (isSafe, risk) = hairpin.EvaluateTurnSafety(20.0f, 1.35f, 2.15f);
            if (!isSafe || risk > 0.85f)
                throw new Exception("Hairpin turn safety calculation failed for low-speed navigation.");
        }

        public static void TestUsedBusValuation()
        {
            var engine = new UsedBusValuationAndDepreciationEngine01();
            float val = engine.CalculateResaleValue(3500000f, 3.0f, 320000f, 0.85f);
            if (val <= 3500000f * 0.15f || val > 3500000f)
                throw new Exception("Used bus valuation outside realistic financial boundaries.");
        }
    }
}
