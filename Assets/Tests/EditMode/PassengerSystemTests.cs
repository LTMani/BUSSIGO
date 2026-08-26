using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Bussigo.Passengers;

namespace Bussigo.Tests.EditMode
{
    [TestFixture]
    public class PassengerSystemTests
    {
        [Test]
        public void SeatManager_Enforces44SeatCapacityAndPreventsDuplicates()
        {
            var seatMgr = new SeatManager();
            Assert.AreEqual(44, seatMgr.Seats.Count);

            var assignedSeats = new HashSet<int>();

            // Assign all 44 seats
            for (int i = 1; i <= 44; i++)
            {
                int seatNum = seatMgr.AssignNextAvailableSeat($"PAX_{i}");
                Assert.AreNotEqual(-1, seatNum);
                Assert.IsTrue(assignedSeats.Add(seatNum), $"Duplicate seat {seatNum} assigned");
            }

            // 45th passenger must be rejected (Bus Full)
            int overflowSeat = seatMgr.AssignNextAvailableSeat("PAX_45");
            Assert.AreEqual(-1, overflowSeat);
            Assert.AreEqual(44, seatMgr.OccupiedSeatCount);
        }

        [Test]
        public void BoardingManager_HandlesSuryapetDeboardingAndContinuingPassengers()
        {
            var bMgr = new BoardingManager();
            bMgr.PopulateTerminalQueue("NODE_VJA_PNBS", 30);

            // Board all passengers
            while (bMgr.terminalWaitingQueue.Count > 0)
            {
                bMgr.ProcessNextBoardingPassenger(out _);
            }

            Assert.AreEqual(30, bMgr.onboardPassengers.Count);

            // Arrive at Suryapet (NODE_SYP_HUB)
            var sypDeboarded = bMgr.ProcessDeboardingAtNode("NODE_SYP_HUB");
            Assert.Greater(sypDeboarded.Count, 0);

            // Remaining passengers must continue to Hyderabad
            for (int i = 0; i < bMgr.onboardPassengers.Count; i++)
            {
                Assert.AreEqual("NODE_HYD_MGBS", bMgr.onboardPassengers[i].destinationNodeID);
            }

            // Arrive at Hyderabad (NODE_HYD_MGBS)
            var hydDeboarded = bMgr.ProcessDeboardingAtNode("NODE_HYD_MGBS");
            Assert.Greater(hydDeboarded.Count, 0);
            Assert.AreEqual(0, bMgr.onboardPassengers.Count); // All passengers deboarded
        }

        [Test]
        public void PassengerSatisfactionSystem_PenalizesHarshBraking()
        {
            var satSystem = new PassengerSatisfactionSystem();
            var passengers = new List<PassengerProfile>
            {
                new PassengerProfile("P1", "Test", PassengerCategory.SoloTraveller, "VJA", "HYD", LuggageSize.SmallHandbag)
            };

            Assert.AreEqual(100f, passengers[0].satisfactionScore);

            // Apply harsh braking (-5.0 m/s^2) for 10 seconds
            for (int i = 0; i < 500; i++)
            {
                satSystem.ApplyDrivingTelemetryEvent(passengers, longitudinalAccelMss: -5.0f, lateralAccelMss: 0f, deltaTime: 0.02f);
            }

            Assert.Less(passengers[0].satisfactionScore, 80f);
        }
    }
}
