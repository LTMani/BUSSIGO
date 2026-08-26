using System;
using NUnit.Framework;
using UnityEngine;
using Bussigo.Traffic;

namespace Bussigo.Tests.EditMode
{
    [TestFixture]
    public class TrafficSystemTests
    {
        [Test]
        public void TrafficVehicleProfile_CreatesDistinctVehicleCategories()
        {
            var car = TrafficVehicleProfile.CreateDefault(VehicleCategory.CarSedan);
            var truck = TrafficVehicleProfile.CreateDefault(VehicleCategory.HeavyTruck10Wheel);
            var bus = TrafficVehicleProfile.CreateDefault(VehicleCategory.IntercityBus);

            Assert.AreEqual("Sedan", car.typeName);
            Assert.AreEqual("10-Wheel Heavy Truck", truck.typeName);
            Assert.AreEqual(16000f, truck.massKg);
            Assert.Greater(car.desiredSpeedKmh, truck.desiredSpeedKmh);
            Assert.AreEqual(12.0f, bus.lengthMeters);
        }

        [Test]
        public void TrafficVehicleController_IDMSlowsDownBehindLeadVehicle()
        {
            var go = new GameObject("TestTrafficVehicle");
            var ctrl = go.AddComponent<TrafficVehicleController>();
            var profile = TrafficVehicleProfile.CreateDefault(VehicleCategory.CarSedan);
            ctrl.Initialize(profile, initialSpeedKmh: 90f, initialDistMeters: 100f);

            // Step 1: Free flow (lead vehicle very far ahead at 500m)
            ctrl.StepIDM(0.02f, distanceToLeadMeters: 500f, leadSpeedMps: 25f);
            Assert.AreEqual(DriverBehaviorState.FreeFlow, ctrl.behaviorState);

            // Step 2: Lead vehicle suddenly close ahead at 15m travelling slowly at 5 m/s
            for (int i = 0; i < 50; i++)
            {
                ctrl.StepIDM(0.02f, distanceToLeadMeters: 15f, leadSpeedMps: 5f);
            }

            // Assert car slowed down and transitioned to Braking or Following
            Assert.Less(ctrl.currentSpeedKmh, 85f);
            Assert.IsTrue(ctrl.behaviorState == DriverBehaviorState.Braking || ctrl.behaviorState == DriverBehaviorState.Following);

            GameObject.DestroyImmediate(go);
        }

        [Test]
        public void TrafficLaneAgent_SmoothLateralTransitionAndSafetyCheck()
        {
            var go = new GameObject("TestLaneAgent");
            var laneAgent = go.AddComponent<TrafficLaneAgent>();
            laneAgent.currentLaneIndex = 0;
            laneAgent.targetLaneIndex = 0;

            // Cannot change lane if front gap is dangerously close (< 12m)
            bool safe = laneAgent.EvaluateMOBILSafeLaneChange(1, frontGapMeters: 6.0f, rearGapMeters: 20.0f, minSafeGap: 12.0f);
            Assert.IsFalse(safe);

            // Can change lane if gaps are safe (> 12m)
            safe = laneAgent.EvaluateMOBILSafeLaneChange(1, frontGapMeters: 30.0f, rearGapMeters: 30.0f, minSafeGap: 12.0f);
            Assert.IsTrue(safe);
            Assert.AreEqual(LaneChangeState.ChangingRight, laneAgent.changeState);

            // Simulate lateral update steps
            for (int i = 0; i < 150; i++)
            {
                laneAgent.UpdateLateralKinematics(0.02f);
            }

            Assert.AreEqual(LaneChangeState.FollowingLane, laneAgent.changeState);
            Assert.AreEqual(1, laneAgent.currentLaneIndex);

            GameObject.DestroyImmediate(go);
        }
    }
}
