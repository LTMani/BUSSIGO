using System;
using UnityEngine;

namespace Bussigo.Vehicle
{
    /// <summary>
    /// Smoothly animates physical 3D glider passenger door meshes on open/close requests.
    /// </summary>
    public class BusDoorActuator : MonoBehaviour
    {
        public BusChassisController chassisController;
        public BusModelRigHierarchy rigHierarchy;

        [Header("Door Angle & Translation Settings")]
        public float doorOpenAngleDegrees = 85f;
        public Vector3 doorOpenOffset = new Vector3(0.15f, 0f, 0.45f);
        public float actuationSpeed = 2.5f;

        private float currentOpen01 = 0f;
        private Vector3 doorClosedLocalPos;
        private Quaternion doorClosedLocalRot;

        private void Start()
        {
            if (rigHierarchy != null && rigHierarchy.frontGliderDoorTransform != null)
            {
                doorClosedLocalPos = rigHierarchy.frontGliderDoorTransform.localPosition;
                doorClosedLocalRot = rigHierarchy.frontGliderDoorTransform.localRotation;
            }
        }

        private void Update()
        {
            if (chassisController == null || rigHierarchy == null || rigHierarchy.frontGliderDoorTransform == null) return;

            float targetOpen01 = chassisController.isDoorOpen ? 1.0f : 0.0f;
            currentOpen01 = Mathf.MoveTowards(currentOpen01, targetOpen01, Time.deltaTime * actuationSpeed);

            // Animate local position swing & rotation
            Vector3 targetPos = doorClosedLocalPos + (doorOpenOffset * currentOpen01);
            Quaternion targetRot = doorClosedLocalRot * Quaternion.Euler(0f, -doorOpenAngleDegrees * currentOpen01, 0f);

            rigHierarchy.frontGliderDoorTransform.localPosition = targetPos;
            rigHierarchy.frontGliderDoorTransform.localRotation = targetRot;
        }
    }
}
