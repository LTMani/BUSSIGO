using System;
using Bussigo.Game.Traffic;
using Bussigo.Game.Passengers;

namespace Bussigo.Tests.PlayMode
{
    public static class TrafficAIAndPassengerTests
    {
        public static void RunAllTests()
        {
            TestIDMAccelerationStability();
            TestPassengerSatisfactionDynamics();
        }

        public static void TestIDMAccelerationStability()
        {
            var p = new IDMParameters();
            // Approaching slower leader at close distance
            float accel = IDMTrafficSolver.CalculateIDMAcceleration(25f, 15f, 20f, p);
            if (accel >= 0.0f) throw new Exception("IDM should decelerate when closing in on slower leader.");
        }

        public static void TestPassengerSatisfactionDynamics()
        {
            var model = new PassengerSatisfactionModel();
            // Simulating harsh braking
            model.EvaluateDrivingDynamics(0.0f, -0.65f, 75f, 80f, 0.1f);
            if (model.Metrics.DrivingSmoothnessScore >= 100.0f)
                throw new Exception("Passenger satisfaction score should drop after harsh braking.");
        }
    }
}
