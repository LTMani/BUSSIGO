using System;
using UnityEngine;
using Bussigo.Core;
using Bussigo.Vehicle;

namespace Bussigo.UI
{
    public class TelemetryHUDView : MonoBehaviour
    {
        public BusChassisController bus;

        private void OnGUI()
        {
            if (bus == null || BussigoGameManager.Instance == null) return;

            GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
            boxStyle.fontSize = 14;
            boxStyle.normal.textColor = Color.white;

            GUIStyle largeStyle = new GUIStyle(GUI.skin.label);
            largeStyle.fontSize = 22;
            largeStyle.fontStyle = FontStyle.Bold;
            largeStyle.normal.textColor = new Color(1f, 0.85f, 0.2f);

            float width = 420f;
            float height = 120f;
            float x = (Screen.width - width) * 0.5f;
            float y = Screen.height - height - 15f;

            GUI.Box(new Rect(x, y, width, height), "", boxStyle);
            GUI.Label(new Rect(x + 15, y + 10, 160, 35), $"{Mathf.Abs(bus.currentSpeedKmh):F0} KM/H", largeStyle);
            GUI.Label(new Rect(x + 180, y + 10, 140, 35), $"AIR: {bus.primaryAirPressureBar:F1} BAR", largeStyle);

            GUI.Label(new Rect(x + 15, y + 50, 390, 25), $"PASSENGERS: {BussigoGameManager.Instance.boardedPassengers}/{BussigoGameManager.Instance.maxPassengerCapacity} | RETARDER: STAGE {bus.retarderLevel}");
            GUI.Label(new Rect(x + 15, y + 75, 390, 25), $"ROUTE: {BussigoGameManager.Instance.activeCorridorName}");
            GUI.Label(new Rect(x + 15, y + 95, 390, 25), $"CONTROLS: W/A/S/D, E (Doors), H (Horn), R (Retarder)");
        }
    }
}
