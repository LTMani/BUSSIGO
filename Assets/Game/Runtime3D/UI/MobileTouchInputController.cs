using System;
using UnityEngine;
using Bussigo.Game.Runtime3D.Vehicle;

namespace Bussigo.Game.Runtime3D.UI
{
    public class MobileTouchInputController : MonoBehaviour
    {
        public UnityBusController3D busController;
        public UnityBusCameraSystem cameraSystem;

        [Header("Mobile Configuration")]
        public bool forceEnableMobileUI = true;
        public bool useAnalogSteeringWheel = false;
        public float touchDeadZonePixels = 15f;

        private float virtualSteeringValue = 0f;
        private bool isTouchingSteerLeft = false;
        private bool isTouchingSteerRight = false;
        private bool isTouchingThrottle = false;
        private bool isTouchingBrake = false;

        private void Start()
        {
            if (Application.isMobilePlatform)
            {
                forceEnableMobileUI = true;
                Screen.orientation = ScreenOrientation.LandscapeLeft;
            }
        }

        private void Update()
        {
            if (!forceEnableMobileUI || busController == null) return;

            // Smooth touch steering calculation
            if (isTouchingSteerLeft)
            {
                virtualSteeringValue = Mathf.MoveTowards(virtualSteeringValue, -1.0f, Time.deltaTime * 3.5f);
            }
            else if (isTouchingSteerRight)
            {
                virtualSteeringValue = Mathf.MoveTowards(virtualSteeringValue, 1.0f, Time.deltaTime * 3.5f);
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) < 0.05f)
            {
                // Return steering wheel to center
                virtualSteeringValue = Mathf.MoveTowards(virtualSteeringValue, 0.0f, Time.deltaTime * 4.5f);
            }

            if (Mathf.Abs(virtualSteeringValue) > 0.01f)
            {
                busController.currentSteeringInput = virtualSteeringValue;
            }

            // Throttle & Brake touch application
            if (isTouchingThrottle)
            {
                busController.currentThrottleInput01 = 1.0f;
                busController.currentBrakeInput01 = 0.0f;
            }
            else if (isTouchingBrake)
            {
                busController.currentThrottleInput01 = 0.0f;
                busController.currentBrakeInput01 = 1.0f;
            }
        }

        private void OnGUI()
        {
            if (!forceEnableMobileUI || busController == null) return;

            GUIStyle padBtnStyle = new GUIStyle(GUI.skin.button);
            padBtnStyle.fontSize = 15;
            padBtnStyle.fontStyle = FontStyle.Bold;
            padBtnStyle.alignment = TextAnchor.MiddleCenter;

            GUIStyle actionBtnStyle = new GUIStyle(GUI.skin.button);
            actionBtnStyle.fontSize = 12;
            actionBtnStyle.fontStyle = FontStyle.Bold;
            actionBtnStyle.alignment = TextAnchor.MiddleCenter;

            // Reset touch states each GUI frame before evaluation
            isTouchingSteerLeft = false;
            isTouchingSteerRight = false;
            isTouchingThrottle = false;
            isTouchingBrake = false;

            // 1. Left Side Controls: Steering Buttons
            float steerY = Screen.height - 145f;
            if (GUI.RepeatButton(new Rect(20, steerY, 90, 95), "◀\nSTEER\nLEFT", padBtnStyle))
            {
                isTouchingSteerLeft = true;
            }
            if (GUI.RepeatButton(new Rect(120, steerY, 90, 95), "▶\nSTEER\nRIGHT", padBtnStyle))
            {
                isTouchingSteerRight = true;
            }

            // 2. Right Side Controls: Throttle & Brake Pedals
            float pedalY = Screen.height - 180f;
            if (GUI.RepeatButton(new Rect(Screen.width - 110, pedalY, 90, 130), "ACCEL\n▲\n[GAS]", padBtnStyle))
            {
                isTouchingThrottle = true;
            }
            if (GUI.RepeatButton(new Rect(Screen.width - 215, Screen.height - 135f, 90, 85), "BRAKE\n▼\n[STOP]", padBtnStyle))
            {
                isTouchingBrake = true;
            }

            // 3. Transmission Shifter Lever (D, N, R)
            float gearX = Screen.width - 110;
            if (GUI.Button(new Rect(gearX, 80, 85, 38), "SHIFT ▲", actionBtnStyle))
            {
                busController.ShiftGearUp();
            }
            if (GUI.Button(new Rect(gearX, 125, 85, 38), "SHIFT ▼", actionBtnStyle))
            {
                busController.ShiftGearDown();
            }

            // 4. Mobile Quick Action Toolbar (Bottom Center)
            float barY = Screen.height - 55f;
            float barStartX = Screen.width * 0.5f - 240f;
            float btnW = 75f;
            float btnH = 45f;
            float gap = 6f;

            if (GUI.Button(new Rect(barStartX, barY, btnW, btnH), "HORN\n(H)", actionBtnStyle))
            {
                busController.isHornSounding = true;
            }
            else
            {
                if (!Input.GetKey(KeyCode.H)) busController.isHornSounding = false;
            }

            if (GUI.Button(new Rect(barStartX + (btnW + gap) * 1, barY, btnW, btnH), "DOOR\n(E)", actionBtnStyle))
            {
                if (busController.currentSpeedKmh < 3.0f)
                {
                    busController.isDoorOpen = !busController.isDoorOpen;
                }
            }

            if (GUI.Button(new Rect(barStartX + (btnW + gap) * 2, barY, btnW, btnH), "CAM\n(C)", actionBtnStyle))
            {
                if (cameraSystem != null) cameraSystem.CycleNextCameraMode();
            }

            if (GUI.Button(new Rect(barStartX + (btnW + gap) * 3, barY, btnW, btnH), "LIGHTS\n(L)", actionBtnStyle))
            {
                busController.isHighBeamActive = !busController.isHighBeamActive;
            }

            if (GUI.Button(new Rect(barStartX + (btnW + gap) * 4, barY, btnW, btnH), "RETARD\n(R)", actionBtnStyle))
            {
                busController.currentRetarderLevel = (busController.currentRetarderLevel + 1) % 5;
            }

            if (GUI.Button(new Rect(barStartX + (btnW + gap) * 5, barY, btnW, btnH), "PARK\n[P]", actionBtnStyle))
            {
                busController.isSpringEmergencyBrakeEngaged = !busController.isSpringEmergencyBrakeEngaged;
            }
        }
    }
}
