using System;
using UnityEngine;

namespace Bussigo.Vehicle
{
    /// <summary>
    /// Explicit hierarchy contract and binding container for imported 3D bus models (FBX/GLTF).
    /// </summary>
    public class BusModelRigHierarchy : MonoBehaviour
    {
        [Header("Model Root Containers")]
        public Transform chassisRoot;
        public Transform exteriorRoot;
        public Transform interiorRoot;
        public Transform cockpitRoot;

        [Header("Animated Dynamic Transforms")]
        public Transform steeringWheelTransform;
        public Transform frontGliderDoorTransform;
        public Transform rearEmergencyDoorTransform;

        [Header("6-Wheel Transform Assemblies")]
        public Transform wheelFrontLeft;
        public Transform wheelFrontRight;
        public Transform wheelRearLeftInner;
        public Transform wheelRearLeftOuter;
        public Transform wheelRearRightInner;
        public Transform wheelRearRightOuter;

        [Header("Lighting Points")]
        public Transform headlightsRoot;
        public Transform taillightsRoot;
        public Transform indicatorLightsRoot;

        [Header("Camera Rig Mounts")]
        public Transform cameraMountChase;
        public Transform cameraMountBumper;
        public Transform cameraMountCockpitDriverEye;
        public Transform cameraMountPassengerCabin;

        public bool ValidateHierarchy(out string missingNodeMessage)
        {
            if (chassisRoot == null) { missingNodeMessage = "Missing Chassis Root"; return false; }
            if (exteriorRoot == null) { missingNodeMessage = "Missing Exterior Root"; return false; }
            if (interiorRoot == null) { missingNodeMessage = "Missing Interior Root"; return false; }
            if (cockpitRoot == null) { missingNodeMessage = "Missing Cockpit Root"; return false; }
            if (steeringWheelTransform == null) { missingNodeMessage = "Missing Steering Wheel Transform"; return false; }
            if (wheelFrontLeft == null || wheelFrontRight == null) { missingNodeMessage = "Missing Front Wheel Transforms"; return false; }
            if (wheelRearLeftOuter == null || wheelRearRightOuter == null) { missingNodeMessage = "Missing Rear Wheel Transforms"; return false; }

            missingNodeMessage = "Valid 3D Bus Hierarchy";
            return true;
        }
    }
}
