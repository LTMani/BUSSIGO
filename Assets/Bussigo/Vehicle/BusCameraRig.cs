using System;
using UnityEngine;

namespace Bussigo.Vehicle
{
    public enum BusCameraMode
    {
        ExteriorChase = 0,
        FrontBumper = 1,
        DriverCockpit = 2,
        PassengerCabin = 3
    }

    /// <summary>
    /// Manages switching and lerping between 4 key camera perspectives for the 3D bus.
    /// </summary>
    public class BusCameraRig : MonoBehaviour
    {
        public Camera targetCamera;
        public BusModelRigHierarchy rigHierarchy;
        public BusCameraMode activeMode = BusCameraMode.ExteriorChase;

        public float positionSmoothTime = 0.08f;
        public float rotationSmoothSpeed = 12f;

        private Vector3 currentVelocity;

        private void Update()
        {
            bool cycleKeyPressed = false;

            // 1. Try New Input System
            try
            {
                var keyboardType = Type.GetType("UnityEngine.InputSystem.Keyboard, Unity.InputSystem");
                if (keyboardType != null)
                {
                    var currentProp = keyboardType.GetProperty("current", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    var keyboard = currentProp?.GetValue(null);
                    if (keyboard != null)
                    {
                        var cKeyProp = keyboard.GetType().GetProperty("cKey");
                        var cKey = cKeyProp?.GetValue(keyboard);
                        if (cKey != null)
                        {
                            var wasPressedProp = cKey.GetType().GetProperty("wasPressedThisFrame");
                            if (wasPressedProp != null && (bool)wasPressedProp.GetValue(cKey))
                            {
                                cycleKeyPressed = true;
                            }
                        }
                    }
                }
            }
            catch { }

            // 2. Fallback to Legacy Input Manager
            if (!cycleKeyPressed)
            {
                try
                {
                    if (Input.GetKeyDown(KeyCode.C))
                    {
                        cycleKeyPressed = true;
                    }
                }
                catch { }
            }

            if (cycleKeyPressed)
            {
                CycleCameraMode();
            }
        }

        public void CycleCameraMode()
        {
            activeMode = (BusCameraMode)(((int)activeMode + 1) % 4);
        }

        private void LateUpdate()
        {
            if (targetCamera == null || rigHierarchy == null) return;

            Transform targetMount = GetActiveMountTransform();
            if (targetMount == null) return;

            if (activeMode == BusCameraMode.DriverCockpit || activeMode == BusCameraMode.PassengerCabin)
            {
                // Instant rigid lock inside vehicle cabin to avoid nausea
                targetCamera.transform.position = targetMount.position;
                targetCamera.transform.rotation = targetMount.rotation;
            }
            else
            {
                // Smooth exterior chase / bumper tracking
                targetCamera.transform.position = Vector3.SmoothDamp(
                    targetCamera.transform.position,
                    targetMount.position,
                    ref currentVelocity,
                    positionSmoothTime
                );

                targetCamera.transform.rotation = Quaternion.Slerp(
                    targetCamera.transform.rotation,
                    targetMount.rotation,
                    Time.deltaTime * rotationSmoothSpeed
                );
            }
        }

        private Transform GetActiveMountTransform()
        {
            switch (activeMode)
            {
                case BusCameraMode.ExteriorChase:
                    return rigHierarchy.cameraMountChase;
                case BusCameraMode.FrontBumper:
                    return rigHierarchy.cameraMountBumper;
                case BusCameraMode.DriverCockpit:
                    return rigHierarchy.cameraMountCockpitDriverEye;
                case BusCameraMode.PassengerCabin:
                    return rigHierarchy.cameraMountPassengerCabin;
                default:
                    return rigHierarchy.cameraMountChase;
            }
        }
    }
}
