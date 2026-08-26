using System;
using UnityEngine;
using Bussigo.Game.Runtime3D.Vehicle;

namespace Bussigo.Game.Runtime3D.UI
{
    public class MobileTouchInputController : MonoBehaviour
    {
        public UnityBusController3D busController;
        public UnityBusCameraSystem cameraSystem;

        public bool enableMobileTouchUI = false;

        private void Start()
        {
            // Auto enable on Mobile platforms
            if (Application.isMobilePlatform)
            {
                enableMobileTouchUI = true;
            }
        }

        private void OnGUI()
        {
            if (!enableMobileTouchUI || busController == null) return;

            // Touch Toggle Button (Top Right)
            if (GUI.Button(new Rect(Screen.width - 140, 15, 125, 40), "Toggle Touch UI"))
            {
                enableMobileTouchUI = !enableMobileTouchUI;
            }

            // Left Side: Steer Left / Right Buttons
            if (GUI.RepeatButton(new Rect(25, Screen.height - 130, 80, 80), "◀ STEER\nLEFT"))
            {
                busController.currentSteeringInput = -1.0f;
            }
            if (GUI.RepeatButton(new Rect(115, Screen.height - 130, 80, 80), "STEER ▶\nRIGHT"))
            {
                busController.currentSteeringInput = 1.0f;
            }

            // Right Side: Throttle & Brake Pedals
            if (GUI.RepeatButton(new Rect(Screen.width - 105, Screen.height - 170, 85, 120), "THROTTLE\n▲\n[ACCEL]"))
            {
                busController.currentThrottleInput01 = 1.0f;
                busController.currentBrakeInput01 = 0.0f;
            }
            if (GUI.RepeatButton(new Rect(Screen.width - 200, Screen.height - 130, 85, 80), "BRAKE\n▼"))
            {
                busController.currentThrottleInput01 = 0.0f;
                busController.currentBrakeInput01 = 1.0f;
            }

            // Middle Action Buttons
            if (GUI.Button(new Rect(Screen.width * 0.5f - 140, Screen.height - 65, 80, 45), "HORN (H)"))
            {
                busController.isHornSounding = true;
            }
            if (GUI.Button(new Rect(Screen.width * 0.5f - 50, Screen.height - 65, 80, 45), "DOOR (E)"))
            {
                busController.isDoorOpen = !busController.isDoorOpen;
            }
            if (GUI.Button(new Rect(Screen.width * 0.5f + 40, Screen.height - 65, 80, 45), "CAM (C)"))
            {
                if (cameraSystem != null) cameraSystem.CycleNextCameraMode();
            }
        }
    }
}
