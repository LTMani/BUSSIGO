using System;
using UnityEngine;
using Bussigo.Physics;

namespace Bussigo.Vehicle
{
    /// <summary>
    /// Factory for instantiating and rigging the genuine 12.5m Indian Intercity Luxury Coach 3D asset.
    /// </summary>
    public static class IndianCoachAssetFactory
    {
        public static GameObject CreateRiggedCoach(Vector3 spawnPosition, Quaternion spawnRotation, GameObject imported3DModelPrefab = null)
        {
            GameObject busRoot = new GameObject("BUSSIGO_12M_IndianIntercityCoach");
            busRoot.transform.position = spawnPosition;
            busRoot.transform.rotation = spawnRotation;
            busRoot.tag = "BusPlayer";

            // 1. Physical Chassis & Rigidbody
            Rigidbody rb = busRoot.AddComponent<Rigidbody>();
            rb.mass = 14500f; // 14.5t curb mass
            rb.centerOfMass = new Vector3(0f, -0.65f, 0.2f);
            rb.interpolation = RigidbodyInterpolation.Interpolate;

            BoxCollider chassisCollider = busRoot.AddComponent<BoxCollider>();
            chassisCollider.size = new Vector3(2.6f, 3.2f, 12.5f);
            chassisCollider.center = new Vector3(0f, 1.8f, 0f);

            // 2. Chassis Controller & Physics Model
            BusChassisController chassisController = busRoot.AddComponent<BusChassisController>();
            chassisController.physicsModel.curbMassKg = 14500f;
            chassisController.physicsModel.wheelRadiusMeters = 0.52f;
            chassisController.physicsModel.wheelbaseMeters = 6.2f;

            // 3. Hierarchy Rig Structure
            BusModelRigHierarchy rig = busRoot.AddComponent<BusModelRigHierarchy>();

            // Setup container transforms
            GameObject chassisGo = new GameObject("Chassis");
            chassisGo.transform.SetParent(busRoot.transform, false);
            rig.chassisRoot = chassisGo.transform;

            GameObject exteriorGo = new GameObject("Exterior");
            exteriorGo.transform.SetParent(busRoot.transform, false);
            rig.exteriorRoot = exteriorGo.transform;

            GameObject interiorGo = new GameObject("Interior");
            interiorGo.transform.SetParent(busRoot.transform, false);
            rig.interiorRoot = interiorGo.transform;

            GameObject cockpitGo = new GameObject("Cockpit");
            cockpitGo.transform.SetParent(interiorGo.transform, false);
            rig.cockpitRoot = cockpitGo.transform;

            // Cockpit steering wheel node
            GameObject steerGo = new GameObject("SteeringWheel");
            steerGo.transform.SetParent(cockpitGo.transform, false);
            steerGo.transform.localPosition = new Vector3(-0.60f, 1.65f, 5.15f);
            rig.steeringWheelTransform = steerGo.transform;

            // Passenger door node
            GameObject doorGo = new GameObject("FrontGliderDoor");
            doorGo.transform.SetParent(exteriorGo.transform, false);
            doorGo.transform.localPosition = new Vector3(1.29f, 0.55f, 4.40f);
            rig.frontGliderDoorTransform = doorGo.transform;

            // 6-Wheel Nodes
            GameObject wheelsRoot = new GameObject("Wheels");
            wheelsRoot.transform.SetParent(chassisGo.transform, false);

            rig.wheelFrontLeft = CreateWheelNode(wheelsRoot.transform, "FrontLeft", new Vector3(-1.15f, 0.52f, 3.60f));
            rig.wheelFrontRight = CreateWheelNode(wheelsRoot.transform, "FrontRight", new Vector3(1.15f, 0.52f, 3.60f));
            rig.wheelRearLeftOuter = CreateWheelNode(wheelsRoot.transform, "RearLeftOuter", new Vector3(-1.22f, 0.52f, -3.20f));
            rig.wheelRearLeftInner = CreateWheelNode(wheelsRoot.transform, "RearLeftInner", new Vector3(-0.90f, 0.52f, -3.20f));
            rig.wheelRearRightInner = CreateWheelNode(wheelsRoot.transform, "RearRightInner", new Vector3(0.90f, 0.52f, -3.20f));
            rig.wheelRearRightOuter = CreateWheelNode(wheelsRoot.transform, "RearRightOuter", new Vector3(1.22f, 0.52f, -3.20f));

            // Camera Mounts
            GameObject cameraMounts = new GameObject("CameraMounts");
            cameraMounts.transform.SetParent(busRoot.transform, false);

            rig.cameraMountChase = CreateMount(cameraMounts.transform, "Mount_ExteriorChase", new Vector3(0f, 4.2f, -12.5f), Quaternion.Euler(14f, 0f, 0f));
            rig.cameraMountBumper = CreateMount(cameraMounts.transform, "Mount_FrontBumper", new Vector3(0f, 0.85f, 6.45f), Quaternion.identity);
            rig.cameraMountCockpitDriverEye = CreateMount(cameraMounts.transform, "Mount_DriverEye", new Vector3(-0.60f, 2.15f, 4.75f), Quaternion.identity);
            rig.cameraMountPassengerCabin = CreateMount(cameraMounts.transform, "Mount_PassengerCabin", new Vector3(0f, 2.35f, 1.20f), Quaternion.identity);

            // 4. Attach Actuators and Subsystem Controllers
            var wheelSync = busRoot.AddComponent<BusWheelVisualSync>();
            wheelSync.chassisController = chassisController;
            wheelSync.rigHierarchy = rig;

            var cockpitCtrl = busRoot.AddComponent<BusCockpitController>();
            cockpitCtrl.chassisController = chassisController;
            cockpitCtrl.rigHierarchy = rig;

            var doorActuator = busRoot.AddComponent<BusDoorActuator>();
            doorActuator.chassisController = chassisController;
            doorActuator.rigHierarchy = rig;

            var cameraRig = busRoot.AddComponent<BusCameraRig>();
            cameraRig.rigHierarchy = rig;

            // If 3D Model prefab is supplied, instantiate and parent inside Exterior/Interior
            if (imported3DModelPrefab != null)
            {
                GameObject modelInstance = UnityEngine.Object.Instantiate(imported3DModelPrefab, busRoot.transform, false);
                modelInstance.name = "Imported3DModelInstance";
            }

            Debug.Log("[BUSSIGO] 12.5m Indian Intercity Luxury Coach Rigged & Initialized.");
            return busRoot;
        }

        private static Transform CreateWheelNode(Transform parent, string name, Vector3 localPos)
        {
            GameObject w = new GameObject(name);
            w.transform.SetParent(parent, false);
            w.transform.localPosition = localPos;
            return w.transform;
        }

        private static Transform CreateMount(Transform parent, string name, Vector3 localPos, Quaternion localRot)
        {
            GameObject m = new GameObject(name);
            m.transform.SetParent(parent, false);
            m.transform.localPosition = localPos;
            m.transform.localRotation = localRot;
            return m.transform;
        }
    }
}
