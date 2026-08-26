using System;
using UnityEngine;
using UnityEngine.UI;
using Bussigo.Game.Runtime3D.Vehicle;
using Bussigo.Game.Runtime3D.Passengers;

namespace Bussigo.Game.Runtime3D.UI
{
    public class DrivingCockpitHUDController : MonoBehaviour
    {
        public UnityBusController3D busController;
        public PassengerBoardingSystem3D passengerSystem;

        [Header("Telemetry UI Readouts")]
        public Text speedText;
        public Text rpmText;
        public Text gearText;
        public Text airPressureText;
        public Text fuelText;
        public Text retarderText;
        public Text passengerCountText;
        public Text routeGuidanceText;
        public Image leftIndicatorIcon;
        public Image rightIndicatorIcon;
        public Image headlightIcon;
        public Image handbrakeIcon;

        private void Update()
        {
            if (busController == null) return;

            // Speedometer & RPM
            if (speedText != null) speedText.text = $"{Mathf.Abs(busController.currentSpeedKmh):F0} KM/H";
            if (rpmText != null) rpmText.text = $"{busController.currentEngineRpm:F0} RPM";

            // Gear Indicator
            if (gearText != null)
            {
                if (busController.currentGearIndex == -1) gearText.text = "R";
                else if (busController.currentGearIndex == 0) gearText.text = "N";
                else gearText.text = $"D{busController.currentGearIndex}";
            }

            // Air Pressure & Retarder
            if (airPressureText != null) airPressureText.text = $"AIR: {busController.primaryAirPressureBar:F1} BAR";
            if (retarderText != null) retarderText.text = $"RETARDER: {busController.currentRetarderLevel}";

            // Fuel
            if (fuelText != null) fuelText.text = $"DIESEL: {busController.currentFuelLiters:F1} L";

            // Passenger Count
            if (passengerCountText != null && passengerSystem != null)
            {
                passengerCountText.text = $"PASSENGERS: {passengerSystem.currentBoardedPassengers} / {passengerSystem.maxSeatingCapacity}";
            }

            // Lighting & Indicators
            if (leftIndicatorIcon != null) leftIndicatorIcon.enabled = busController.isLeftIndicatorActive || busController.isHazardActive;
            if (rightIndicatorIcon != null) rightIndicatorIcon.enabled = busController.isRightIndicatorActive || busController.isHazardActive;
            if (headlightIcon != null) headlightIcon.enabled = busController.isHighBeamActive;
            if (handbrakeIcon != null) handbrakeIcon.enabled = busController.isSpringEmergencyBrakeEngaged || busController.primaryAirPressureBar < 3.8f;
        }

        private void OnGUI()
        {
            // Immediate GUI fallback to guarantee HUD visibility without requiring manual canvas prefab setup
            if (busController == null) return;

            GUIStyle hudBoxStyle = new GUIStyle(GUI.skin.box);
            hudBoxStyle.fontSize = 14;
            hudBoxStyle.normal.textColor = Color.white;
            hudBoxStyle.alignment = TextAnchor.UpperLeft;

            GUIStyle largeDigitStyle = new GUIStyle(GUI.skin.label);
            largeDigitStyle.fontSize = 24;
            largeDigitStyle.fontStyle = FontStyle.Bold;
            largeDigitStyle.normal.textColor = new Color(1.0f, 0.85f, 0.2f);

            // Dashboard Cluster (Bottom Center)
            float hudWidth = 420f;
            float hudHeight = 135f;
            float hudX = (Screen.width - hudWidth) * 0.5f;
            float hudY = Screen.height - hudHeight - 15f;

            GUI.Box(new Rect(hudX, hudY, hudWidth, hudHeight), "", hudBoxStyle);

            string gearStr = busController.currentGearIndex == -1 ? "R" : (busController.currentGearIndex == 0 ? "N" : $"D{busController.currentGearIndex}");
            GUI.Label(new Rect(hudX + 15, hudY + 10, 160, 40), $"{Mathf.Abs(busController.currentSpeedKmh):F0} KM/H", largeDigitStyle);
            GUI.Label(new Rect(hudX + 180, hudY + 10, 80, 40), $"[{gearStr}]", largeDigitStyle);
            GUI.Label(new Rect(hudX + 265, hudY + 10, 140, 40), $"{busController.currentEngineRpm:F0} RPM", largeDigitStyle);

            GUI.Label(new Rect(hudX + 15, hudY + 55, 180, 25), $"Air: {busController.primaryAirPressureBar:F1} bar {(busController.primaryAirPressureBar < 4f ? "(!) LOW" : "(OK)")}");
            GUI.Label(new Rect(hudX + 205, hudY + 55, 180, 25), $"Diesel: {busController.currentFuelLiters:F1} L");
            GUI.Label(new Rect(hudX + 15, hudY + 80, 180, 25), $"Retarder: Level {busController.currentRetarderLevel}/4");
            GUI.Label(new Rect(hudX + 205, hudY + 80, 180, 25), $"Doors: {(busController.isDoorOpen ? "OPEN (Key E)" : "CLOSED")}");

            int paxCount = (passengerSystem != null) ? passengerSystem.currentBoardedPassengers : 45;
            GUI.Label(new Rect(hudX + 15, hudY + 105, 380, 25), $"Passengers: {paxCount}/49 | Controls: W/A/S/D, C (Cam), H (Horn)");

            // Route GPS Navigation Banner (Top Center)
            GUI.Box(new Rect((Screen.width - 500) * 0.5f, 15, 500, 45), "", hudBoxStyle);
            GUI.Label(new Rect((Screen.width - 480) * 0.5f, 22, 480, 30), "GPS: NH65 Express (Vijayawada PNBS ➔ Suryapet ➔ Hyderabad MGBS)");
        }
    }
}
