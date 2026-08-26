using System;
using UnityEngine;

namespace Bussigo.Game.Runtime3D.Vehicle
{
    public enum BusCameraMode
    {
        ChaseThirdPerson = 0,
        DriverCockpitFirstPerson = 1,
        DriverEyeView = 2,
        PassengerCabinView = 3,
        FrontBumperCamera = 4,
        RearDockingCamera = 5,
        SideWingMirrorView = 6,
        CinematicFlyBy = 7
    }

    public class UnityBusCameraSystem : MonoBehaviour
    {
        [Header("Target Bus")]
        public UnityBusController3D targetBus;

        [Header("Active Mode")]
        public BusCameraMode currentCameraMode = BusCameraMode.ChaseThirdPerson;

        [Header("Chase Camera Configuration")]
        public Vector3 chaseCameraOffset = new Vector3(0f, 3.6f, -10.5f);
        public float chaseFollowDamping = 8.5f;
        public float chaseRotationDamping = 6.5f;
        public float baseFOV = 60f;
        public float maxSpeedFOVBonus = 12f;

        [Header("Interior & Exterior Offsets")]
        public Vector3 cockpitCameraLocalOffset = new Vector3(-0.60f, 1.95f, 4.2f);
        public Vector3 driverEyeLocalOffset = new Vector3(-0.60f, 2.05f, 4.0f);
        public Vector3 passengerSeatLocalOffset = new Vector3(0.75f, 1.85f, 0.5f);
        public Vector3 bumperCameraLocalOffset = new Vector3(0f, 0.85f, 6.2f);
        public Vector3 rearDockingLocalOffset = new Vector3(0f, 3.2f, -6.5f);
        public Vector3 sideMirrorLocalOffset = new Vector3(-1.45f, 2.1f, 4.5f);

        private float mouseOrbitYaw = 0f;
        private float mouseOrbitPitch = 0f;
        private Camera camComponent;
        private Vector3 flyByStationaryPoint;
        private bool isFlyByActive = false;

        private void Awake()
        {
            camComponent = GetComponent<Camera>();
            if (camComponent == null) camComponent = Camera.main;
        }

        private void LateUpdate()
        {
            if (targetBus == null) return;

            // Camera Mode Cycle Key
            if (Input.GetKeyDown(KeyCode.C))
            {
                CycleNextCameraMode();
            }

            // Dynamic FOV based on speed
            if (camComponent != null)
            {
                float speed01 = Mathf.Clamp01(targetBus.currentSpeedKmh / 100f);
                camComponent.fieldOfView = Mathf.Lerp(camComponent.fieldOfView, baseFOV + (speed01 * maxSpeedFOVBonus), Time.deltaTime * 3.5f);
            }

            switch (currentCameraMode)
            {
                case BusCameraMode.ChaseThirdPerson:
                    UpdateChaseCamera(Time.deltaTime);
                    break;
                case BusCameraMode.DriverCockpitFirstPerson:
                    UpdateAttachedCamera(cockpitCameraLocalOffset, false, 0f);
                    break;
                case BusCameraMode.DriverEyeView:
                    UpdateDriverEyeCamera(Time.deltaTime);
                    break;
                case BusCameraMode.PassengerCabinView:
                    UpdateAttachedCamera(passengerSeatLocalOffset, true, 45f);
                    break;
                case BusCameraMode.FrontBumperCamera:
                    UpdateAttachedCamera(bumperCameraLocalOffset, false, 0f);
                    break;
                case BusCameraMode.RearDockingCamera:
                    UpdateRearDockingCamera();
                    break;
                case BusCameraMode.SideWingMirrorView:
                    UpdateAttachedCamera(sideMirrorLocalOffset, false, -165f);
                    break;
                case BusCameraMode.CinematicFlyBy:
                    UpdateCinematicFlyBy(Time.deltaTime);
                    break;
            }
        }

        public void CycleNextCameraMode()
        {
            currentCameraMode = (BusCameraMode)(((int)currentCameraMode + 1) % 8);
            if (currentCameraMode == BusCameraMode.CinematicFlyBy)
            {
                isFlyByActive = false;
            }
        }

        private void UpdateChaseCamera(float dt)
        {
            // Right-click Orbit
            if (Input.GetMouseButton(1))
            {
                mouseOrbitYaw += Input.GetAxis("Mouse X") * 3.8f;
                mouseOrbitPitch -= Input.GetAxis("Mouse Y") * 2.5f;
                mouseOrbitPitch = Mathf.Clamp(mouseOrbitPitch, -15f, 55f);
            }
            else
            {
                mouseOrbitYaw = Mathf.Lerp(mouseOrbitYaw, 0f, dt * 2.5f);
                mouseOrbitPitch = Mathf.Lerp(mouseOrbitPitch, 0f, dt * 2.5f);
            }

            Quaternion targetRotation = Quaternion.Euler(mouseOrbitPitch, targetBus.transform.eulerAngles.y + mouseOrbitYaw, 0f);
            Vector3 rotatedOffset = targetRotation * chaseCameraOffset;
            Vector3 desiredPosition = targetBus.transform.position + rotatedOffset;

            transform.position = Vector3.Lerp(transform.position, desiredPosition, dt * chaseFollowDamping);

            Vector3 lookTarget = targetBus.transform.position + targetBus.transform.forward * 2.8f + Vector3.up * 1.6f;
            Quaternion desiredLookRot = Quaternion.LookRotation(lookTarget - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredLookRot, dt * chaseRotationDamping);
        }

        private void UpdateDriverEyeCamera(float dt)
        {
            Vector3 worldPos = targetBus.transform.TransformPoint(driverEyeLocalOffset);
            transform.position = worldPos;

            // Subtle head roll during cornering
            float steerRoll = -targetBus.currentSteeringInput * 4.5f;
            Quaternion eyeRotation = targetBus.transform.rotation * Quaternion.Euler(0f, 0f, steerRoll);
            transform.rotation = Quaternion.Slerp(transform.rotation, eyeRotation, dt * 10f);
        }

        private void UpdateRearDockingCamera()
        {
            Vector3 worldPos = targetBus.transform.TransformPoint(rearDockingLocalOffset);
            transform.position = worldPos;
            transform.rotation = targetBus.transform.rotation * Quaternion.Euler(22f, 180f, 0f);
        }

        private void UpdateAttachedCamera(Vector3 localOffset, bool isSideAngle, float customYaw)
        {
            Vector3 worldPos = targetBus.transform.TransformPoint(localOffset);
            transform.position = worldPos;
            transform.rotation = targetBus.transform.rotation * Quaternion.Euler(0f, customYaw, 0f);
        }

        private void UpdateCinematicFlyBy(float dt)
        {
            if (!isFlyByActive || Vector3.Distance(transform.position, targetBus.transform.position) > 65f)
            {
                flyByStationaryPoint = targetBus.transform.position + targetBus.transform.forward * 35f + targetBus.transform.right * 7.5f + Vector3.up * 1.8f;
                transform.position = flyByStationaryPoint;
                isFlyByActive = true;
            }

            Vector3 targetLook = targetBus.transform.position + Vector3.up * 1.5f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetLook - transform.position), dt * 8f);
        }
    }
}
