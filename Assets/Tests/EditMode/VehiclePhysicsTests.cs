using System;
using NUnit.Framework;
using UnityEngine;
using Bussigo.Physics;

namespace Bussigo.Tests.EditMode
{
    [TestFixture]
    public class VehiclePhysicsTests
    {
        [Test]
        public void EnginePowertrain_TorqueCurve_PeakInOptimumBand()
        {
            var powertrain = new EnginePowertrain();

            // Idle torque (650 RPM)
            powertrain.currentRpm = 650f;
            float idleTorque = powertrain.CalculateEngineTorque(1.0f);
            Assert.AreEqual(1400f * 0.70f, idleTorque, 1.0f);

            // Peak torque (1400 RPM)
            powertrain.currentRpm = 1400f;
            float peakTorque = powertrain.CalculateEngineTorque(1.0f);
            Assert.AreEqual(1400f, peakTorque, 0.1f);

            // Redline torque (2400 RPM)
            powertrain.currentRpm = 2400f;
            float redlineTorque = powertrain.CalculateEngineTorque(1.0f);
            Assert.AreEqual(1400f * 0.75f, redlineTorque, 1.0f);
        }

        [Test]
        public void EnginePowertrain_GearShifting_ChangesRatios()
        {
            var powertrain = new EnginePowertrain();
            powertrain.currentGear = 1;
            Assert.AreEqual(6.82f, powertrain.GetCurrentGearRatio());

            powertrain.ShiftUp();
            Assert.AreEqual(2, powertrain.currentGear);
            Assert.AreEqual(3.68f, powertrain.GetCurrentGearRatio());

            powertrain.ShiftDown();
            Assert.AreEqual(1, powertrain.currentGear);
        }

        [Test]
        public void PneumaticAirCircuit_Braking_ReducesAirPressureAndRecovers()
        {
            var airCircuit = new PneumaticAirCircuit();
            airCircuit.currentReservoirPressureBar = 8.5f;

            // Apply full brake for 2 seconds without engine compressor
            airCircuit.UpdateCircuit(2.0f, brakeInput01: 1.0f, engineRunning: false);
            Assert.Less(airCircuit.currentReservoirPressureBar, 8.5f);

            // Run compressor without brake for 5 seconds
            float drainedPressure = airCircuit.currentReservoirPressureBar;
            airCircuit.UpdateCircuit(5.0f, brakeInput01: 0.0f, engineRunning: true);
            Assert.Greater(airCircuit.currentReservoirPressureBar, drainedPressure);
        }

        [Test]
        public void RetarderBrakeSystem_Stages_ProvideProgressiveDrag()
        {
            var retarder = new RetarderBrakeSystem();
            Assert.AreEqual(0f, retarder.CalculateRetarderBrakingForce(60f));

            retarder.SetStage(1);
            float force1 = retarder.CalculateRetarderBrakingForce(60f);
            Assert.Greater(force1, 0f);

            retarder.SetStage(4);
            float force4 = retarder.CalculateRetarderBrakingForce(60f);
            Assert.Greater(force4, force1);
        }

        [Test]
        public void HeavyVehiclePhysicsModel_Payload_IncreasesTotalMass()
        {
            var model = new HeavyVehiclePhysicsModel();
            model.curbMassKg = 14500f;
            Assert.AreEqual(14500f, model.TotalMassKg);

            // 45 passengers with 15kg luggage each (80kg total per passenger)
            model.UpdatePayload(45, luggageMassPerPaxKg: 15f);
            float expectedPayload = 45 * 80f; // 3,600 kg
            Assert.AreEqual(14500f + expectedPayload, model.TotalMassKg);
        }
    }
}
