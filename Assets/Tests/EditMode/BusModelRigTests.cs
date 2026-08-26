using System;
using NUnit.Framework;
using UnityEngine;
using Bussigo.Vehicle;

namespace Bussigo.Tests.EditMode
{
    [TestFixture]
    public class BusModelRigTests
    {
        [Test]
        public void BusModelRigHierarchy_Validate_DetectsMissingNodes()
        {
            var go = new GameObject("TestBusModelRig");
            var rig = go.AddComponent<BusModelRigHierarchy>();

            bool isValid = rig.ValidateHierarchy(out string msg);
            Assert.IsFalse(isValid);
            Assert.IsTrue(msg.Contains("Missing"));

            // Setup required mock roots
            rig.chassisRoot = new GameObject("Chassis").transform;
            rig.exteriorRoot = new GameObject("Exterior").transform;
            rig.interiorRoot = new GameObject("Interior").transform;
            rig.cockpitRoot = new GameObject("Cockpit").transform;
            rig.steeringWheelTransform = new GameObject("SteeringWheel").transform;
            rig.wheelFrontLeft = new GameObject("FL").transform;
            rig.wheelFrontRight = new GameObject("FR").transform;
            rig.wheelRearLeftOuter = new GameObject("RLO").transform;
            rig.wheelRearRightOuter = new GameObject("RRO").transform;

            isValid = rig.ValidateHierarchy(out msg);
            Assert.IsTrue(isValid);
            Assert.AreEqual("Valid 3D Bus Hierarchy", msg);

            GameObject.DestroyImmediate(go);
        }

        [Test]
        public void BusCameraRig_CycleMode_CyclesAllFourPerspectives()
        {
            var go = new GameObject("TestCameraRig");
            var cameraRig = go.AddComponent<BusCameraRig>();

            Assert.AreEqual(BusCameraMode.ExteriorChase, cameraRig.activeMode);

            cameraRig.CycleCameraMode();
            Assert.AreEqual(BusCameraMode.FrontBumper, cameraRig.activeMode);

            cameraRig.CycleCameraMode();
            Assert.AreEqual(BusCameraMode.DriverCockpit, cameraRig.activeMode);

            cameraRig.CycleCameraMode();
            Assert.AreEqual(BusCameraMode.PassengerCabin, cameraRig.activeMode);

            cameraRig.CycleCameraMode();
            Assert.AreEqual(BusCameraMode.ExteriorChase, cameraRig.activeMode);

            GameObject.DestroyImmediate(go);
        }
    }
}
