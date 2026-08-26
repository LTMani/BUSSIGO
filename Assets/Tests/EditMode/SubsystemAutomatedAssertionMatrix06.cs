using System;
using Bussigo.Game.Core;
using Bussigo.Game.Vehicles;
using Bussigo.Game.VehiclePhysics;
using Bussigo.Game.Economy;
using Bussigo.Game.Passengers;

namespace Bussigo.Tests.EditMode
{
    public static class SubsystemAutomatedAssertionMatrix06
    {
        public static void RunAllTests()
        {
            TestLuggageCapacityEnforcement();
            TestInsuranceClaimSettlement();
        }

        public static void TestLuggageCapacityEnforcement()
        {
            var bay = new LuggageCompartmentLoadDistribution01();
            bool loaded = bay.TryLoadLuggage(50f, 0.4f);
            if (!loaded) throw new Exception("Luggage load failed for initial load.");
            if (bay.CurrentLuggageWeightKg != 50f)
                throw new Exception("Luggage bay weight tracking discrepancy.");
        }

        public static void TestInsuranceClaimSettlement()
        {
            var policy = new FleetCommercialInsurancePolicyRecord01();
            float payout = policy.ProcessAccidentClaim(85000f);
            if (payout != 70000f) // 85,000 - 15,000 deductible
                throw new Exception("Insurance payout claim calculation failed against policy deductible.");
        }
    }
}
