using System;
using NUnit.Framework;
using UnityEngine;
using Bussigo.Vehicle;

namespace Bussigo.Tests.EditMode
{
    [TestFixture]
    public class IndianCoachFactoryTests
    {
        [Test]
        public void IndianCoachAssetFactory_CreatesCompleteRiggedBus()
        {
            GameObject bus = IndianCoachAssetFactory.CreateRiggedCoach(Vector3.zero, Quaternion.identity);

            Assert.IsNotNull(bus);
            Assert.AreEqual("BUSSIGO_12M_IndianIntercityCoach", bus.name);

            var chassisController = bus.GetComponent<BusChassisController>();
            Assert.IsNotNull(chassisController);
            Assert.AreEqual(14500f, chassisController.physicsModel.curbMassKg);

            var rig = bus.GetComponent<BusModelRigHierarchy>();
            Assert.IsNotNull(rig);
            Assert.IsTrue(rig.ValidateHierarchy(out string msg), $"Validation failed: {msg}");

            // Verify 6 Wheels Presence
            Assert.IsNotNull(rig.wheelFrontLeft);
            Assert.IsNotNull(rig.wheelFrontRight);
            Assert.IsNotNull(rig.wheelRearLeftInner);
            Assert.IsNotNull(rig.wheelRearLeftOuter);
            Assert.IsNotNull(rig.wheelRearRightInner);
            Assert.IsNotNull(rig.wheelRearRightOuter);

            // Verify Camera Mounts
            Assert.IsNotNull(rig.cameraMountChase);
            Assert.IsNotNull(rig.cameraMountBumper);
            Assert.IsNotNull(rig.cameraMountCockpitDriverEye);
            Assert.IsNotNull(rig.cameraMountPassengerCabin);

            // Verify Subsystem Components
            Assert.IsNotNull(bus.GetComponent<BusWheelVisualSync>());
            Assert.IsNotNull(bus.GetComponent<BusCockpitController>());
            Assert.IsNotNull(bus.GetComponent<BusDoorActuator>());
            Assert.IsNotNull(bus.GetComponent<BusCameraRig>());

            GameObject.DestroyImmediate(bus);
        }
    }
}
