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

            float steer = Input.GetAxis("Horizontal");
            float throttle = Mathf.Clamp01(Input.GetAxis("Vertical"));
            float brake = (Input.GetAxis("Vertical") < 0) ? -Input.GetAxis("Vertical") : 0f;

            if (Input.GetKey(KeyCode.Space)) brake = 1.0f;

            targetBus.SetDriverInputs(steer, throttle, brake);

            if (Input.GetKeyDown(KeyCode.E)) targetBus.ToggleGliderDoors();
            if (Input.GetKeyDown(KeyCode.R)) targetBus.CycleRetarder();
            targetBus.isHornSounding = Input.GetKey(KeyCode.H);
            if (Input.GetKeyDown(KeyCode.L)) targetBus.isHeadlightsActive = !targetBus.isHeadlightsActive;
        }
    }
}
