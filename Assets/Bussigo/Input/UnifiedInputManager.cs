using System;
using UnityEngine;
using Bussigo.Vehicle;

namespace Bussigo.InputSystem
{
    public class UnifiedInputManager : MonoBehaviour
    {
        public BusChassisController targetBus;

        private void Update()
        {
            if (targetBus == null) return;

            float steer = 0f;
            float throttle = 0f;
            float brake = 0f;
            bool spacePressed = false;
            bool ePressed = false;
            bool rPressed = false;
            bool hPressed = false;
            bool lPressed = false;

            // 1. Try New Input System via dynamic reflection
            try
            {
                var keyboardType = Type.GetType("UnityEngine.InputSystem.Keyboard, Unity.InputSystem");
                if (keyboardType != null)
                {
                    var currentProp = keyboardType.GetProperty("current", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    var keyboard = currentProp?.GetValue(null);
                    if (keyboard != null)
                    {
                        var aKey = keyboard.GetType().GetProperty("aKey")?.GetValue(keyboard);
                        var dKey = keyboard.GetType().GetProperty("dKey")?.GetValue(keyboard);
                        var wKey = keyboard.GetType().GetProperty("wKey")?.GetValue(keyboard);
                        var sKey = keyboard.GetType().GetProperty("sKey")?.GetValue(keyboard);
                        var spaceKey = keyboard.GetType().GetProperty("spaceKey")?.GetValue(keyboard);
                        var eKey = keyboard.GetType().GetProperty("eKey")?.GetValue(keyboard);
                        var rKey = keyboard.GetType().GetProperty("rKey")?.GetValue(keyboard);
                        var hKey = keyboard.GetType().GetProperty("hKey")?.GetValue(keyboard);
                        var lKey = keyboard.GetType().GetProperty("lKey")?.GetValue(keyboard);

                        bool isPressed(object control)
                        {
                            if (control == null) return false;
                            var isPressedProp = control.GetType().GetProperty("isPressed");
                            return isPressedProp != null && (bool)isPressedProp.GetValue(control);
                        }

                        bool wasPressed(object control)
                        {
                            if (control == null) return false;
                            var wasPressedProp = control.GetType().GetProperty("wasPressedThisFrame");
                            return wasPressedProp != null && (bool)wasPressedProp.GetValue(control);
                        }

                        if (isPressed(aKey)) steer -= 1f;
                        if (isPressed(dKey)) steer += 1f;
                        if (isPressed(wKey)) throttle = 1f;
                        if (isPressed(sKey)) brake = 1f;
                        if (isPressed(spaceKey)) spacePressed = true;
                        if (wasPressed(eKey)) ePressed = true;
                        if (wasPressed(rKey)) rPressed = true;
                        if (isPressed(hKey)) hPressed = true;
                        if (wasPressed(lKey)) lPressed = true;
                    }
                }
            }
            catch { }

            // 2. Try Legacy Input Manager fallback
            try
            {
                if (steer == 0f) steer = Input.GetAxis("Horizontal");
                if (throttle == 0f && brake == 0f)
                {
                    float v = Input.GetAxis("Vertical");
                    if (v > 0f) throttle = v;
                    else if (v < 0f) brake = -v;
                }
                if (Input.GetKey(KeyCode.Space)) spacePressed = true;
                if (Input.GetKeyDown(KeyCode.E)) ePressed = true;
                if (Input.GetKeyDown(KeyCode.R)) rPressed = true;
                if (Input.GetKey(KeyCode.H)) hPressed = true;
                if (Input.GetKeyDown(KeyCode.L)) lPressed = true;
            }
            catch { }

            if (spacePressed) brake = 1.0f;

            targetBus.SetDriverInputs(steer, throttle, brake);

            if (ePressed) targetBus.ToggleGliderDoors();
            if (rPressed) targetBus.CycleRetarder();
            targetBus.isHornSounding = hPressed;
            if (lPressed) targetBus.isHeadlightsActive = !targetBus.isHeadlightsActive;
        }
    }
}
