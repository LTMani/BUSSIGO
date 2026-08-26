using System;
using Bussigo.Game.Core;
using Bussigo.Game.Customization;
using Bussigo.Game.Missions;
using Bussigo.Game.Progression;

namespace Bussigo.Tests.EditMode
{
    public static class ComprehensiveSubsystemAssertionTestPart12_55
    {
        public static void RunAllTests()
        {
            TestFabricCostCalculation();
            TestHighwayChallengeEvaluation();
            TestEndorsementEligibility();
        }

        public static void TestFabricCostCalculation()
        {
            var fabric = new InteriorSeatFabricPatternSpecification01();
            float totalCost = fabric.CalculateTotalBusRefitCost(45);
            if (totalCost <= 0.0f)
                throw new Exception("Fabric refit calculation must be positive.");
        }

        public static void TestHighwayChallengeEvaluation()
        {
            var challenge = new HighwayExpressChallengeTrial01();
            var (completed, bonus) = challenge.EvaluateTrialResult(170.0f, 90.0f);
            if (!completed || bonus <= 0.0f)
                throw new Exception("Challenge evaluation failed on on-time comfortable completion.");
        }

        public static void TestEndorsementEligibility()
        {
            var endorsement = new DriverCommercialEndorsementModel01();
            bool eligible = endorsement.IsEligibleForEndorsement(10000, 30);
            if (!eligible)
                throw new Exception("Driver should be eligible for endorsement with sufficient XP and clean trips.");
        }
    }
}
