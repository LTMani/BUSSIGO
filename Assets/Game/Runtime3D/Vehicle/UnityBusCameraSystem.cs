using System;
using UnityEngine;

namespace Bussigo.Game.Runtime3D.Vehicle
{
    public enum BusCameraMode
    {
        ChaseThirdPerson = 0,
        DriverCockpitFirstPerson = 1,
        FrontBumperCamera = 2,
        PassengerCabinView = 3
    }

    public class UnityBusCameraSystem : MonoBehaviour
    {
        [Header("Target Bus")]
        public UnityBusController3D targetBus;

        [Header("Camera Perspectives")]
        public BusCameraMode currentCameraMode = BusCameraMode.ChaseThirdPerson;

        [Header("Chase Camera Settings")]
        public Vector3 chaseCameraOffset = new Vector3(0f, 3.2f, -8.5f);
        public float chaseFollowDamping = 8.0f;
        public float chaseRotationDamping = 6.0f;

        [Header("Cockpit Camera Settings")]
        public Vector3 cockpitCameraLocalOffset = new Vector3(-0.65f, 1.85f, 3.8f);

        [Header("Bumper Camera Settings")]
        public Vector3 bumperCameraLocalOffset = new Vector3(0f, 0.9f, 5.8f);

        [Header("Passenger Cabin View Settings")]
        public Vector3 passengerSeatLocalOffset = new Vector3(0.65f, 1.75f, 0.5f);

        private float mouseOrbitYaw = 0f;
        private float mouseOrbitPitch = 0f;

        private void LateUpdate()
        {
            if (targetBus == null) return;

            // Camera switch key
            if (Input.GetKeyDown(KeyCode.C))
            {
                CycleNextCameraMode();
            }

            switch (currentCameraMode)
            {
                case BusCameraMode.ChaseThirdPerson:
                    UpdateChaseCamera(Time.deltaTime);
                    break;
                case BusCameraMode.DriverCockpitFirstPerson:
                    UpdateAttachedCamera(cockpitCameraLocalOffset, true);
                    break;
                case BusCameraMode.FrontBumperCamera:
                    UpdateAttachedCamera(bumperCameraLocalOffset, false);
                    break;
                case BusCameraMode.PassengerCabinView:
                    UpdateAttachedCamera(passengerSeatLocalOffset, true);
                    break;
            }
        }

        public void CycleNextCameraMode()
        {
            currentCameraMode = (BusCameraMode)(((int)currentCameraMode + 1) % 4);
        }

        private void UpdateChaseCamera(float dt)
        {
            // Mouse Orbit
            if (Input.GetMouseButton(1))
            {
                mouseOrbitYaw += Input.GetAxis("Mouse X") * 3.5f;
                mouseOrbitPitch -= Input.GetAxis("Mouse Y") * 2.5f;
                mouseOrbitPitch = Mathf.Clamp(mouseOrbitPitch, -20f, 60f);
            }
            else
            {
                mouseOrbitYaw = Mathf.Lerp(mouseOrbitYaw, 0f, dt * 2.0f);
                mouseOrbitPitch = Mathf.Lerp(mouseOrbitPitch, 0f, dt * 2.0f);
            }

            Quaternion targetRotation = Quaternion.Euler(0f, targetBus.transform.eulerAngles.y + mouseOrbitYaw, 0f);
            Vector3 rotatedOffset = targetRotation * chaseCameraOffset;
            Vector3 desiredPosition = targetBus.transform.position + rotatedOffset;

            transform.position = Vector3.Lerp(transform.position, desiredPosition, dt * chaseFollowDamping);

            Vector3 lookTarget = targetBus.transform.position + targetBus.transform.forward * 2.5f + Vector3.up * 1.5f;
            Quaternion desiredLookRot = Quaternion.LookRotation(lookTarget - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredLookRot, dt * chaseRotationDamping);
        }

        private void UpdateAttachedCamera(Vector3 localOffset, bool allowHeadLook)
        {
            Vector3 worldPos = targetBus.transform.TransformPoint(localOffset);
            transform.position = worldPos;

            float headYaw = 0f;
            if (allowHeadLook && Input.GetMouseButton(1))
            {
                headYaw = Input.GetAxis("Mouse X") * 45f;
            }

            transform.rotation = targetBus.transform.rotation * Quaternion.Euler(0f, headYaw, 0f);
        }
    }
}
