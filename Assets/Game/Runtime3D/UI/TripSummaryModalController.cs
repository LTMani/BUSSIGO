using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bussigo.Game.Runtime3D.UI
{
    public class TripSummaryModalController : MonoBehaviour
    {
        public bool isTripSummaryVisible = false;
        public string routeName = "Vijayawada (PNBS) ➔ Hyderabad (MGBS) via NH65";
        public float routeDistanceKm = 274.5f;
        public int passengersTransported = 45;
        public float grossFareRevenueRupees = 21600f;
        public float dieselFuelCostRupees = 6720f;
        public float fastagTollFeeRupees = 385f;
        public float netOperatingProfitRupees = 14495f;
        public int driverXpAwarded = 960;

        public void DisplayTripResult(int passengerCount, float fuelBurnedLiters, float tollPaid, float comfortScore)
        {
            passengersTransported = passengerCount;
            grossFareRevenueRupees = passengerCount * 480f;
            dieselFuelCostRupees = fuelBurnedLiters * 94.0f; // ₹94/L
            fastagTollFeeRupees = tollPaid;
            netOperatingProfitRupees = grossFareRevenueRupees - dieselFuelCostRupees - fastagTollFeeRupees;
            driverXpAwarded = (int)(routeDistanceKm * 3.5f * (comfortScore / 100f));
            
            isTripSummaryVisible = true;
            Time.timeScale = 0f; // Pause game during summary
        }

        private void OnGUI()
        {
            if (!isTripSummaryVisible) return;

            float modalWidth = 520f;
            float modalHeight = 360f;
            float modalX = (Screen.width - modalWidth) * 0.5f;
            float modalY = (Screen.height - modalHeight) * 0.5f;

            GUIStyle modalBoxStyle = new GUIStyle(GUI.skin.box);
            modalBoxStyle.fontSize = 15;
            modalBoxStyle.normal.textColor = Color.white;

            GUIStyle headerStyle = new GUIStyle(GUI.skin.label);
            headerStyle.fontSize = 20;
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.alignment = TextAnchor.MiddleCenter;
            headerStyle.normal.textColor = new Color(0.2f, 0.95f, 0.4f);

            GUI.Box(new Rect(modalX, modalY, modalWidth, modalHeight), "", modalBoxStyle);

            GUI.Label(new Rect(modalX + 10, modalY + 15, modalWidth - 20, 35), "★ TRIP COMPLETED SUCCESSFULLY! ★", headerStyle);
            GUI.Label(new Rect(modalX + 20, modalY + 55, modalWidth - 40, 25), $"Route: {routeName}");
            GUI.Label(new Rect(modalX + 20, modalY + 80, modalWidth - 40, 25), $"Distance Covered: {routeDistanceKm:F1} km | Passengers: {passengersTransported}");

            GUI.Label(new Rect(modalX + 20, modalY + 115, modalWidth - 40, 25), $"Gross Ticket Revenue: +₹{grossFareRevenueRupees:N2}");
            GUI.Label(new Rect(modalX + 20, modalY + 140, modalWidth - 40, 25), $"Diesel Fuel Consumption: -₹{dieselFuelCostRupees:N2}");
            GUI.Label(new Rect(modalX + 20, modalY + 165, modalWidth - 40, 25), $"FASTag Electronic Toll: -₹{fastagTollFeeRupees:N2}");

            GUIStyle profitStyle = new GUIStyle(GUI.skin.label);
            profitStyle.fontSize = 18;
            profitStyle.fontStyle = FontStyle.Bold;
            profitStyle.normal.textColor = new Color(1.0f, 0.85f, 0.2f);

            GUI.Label(new Rect(modalX + 20, modalY + 205, modalWidth - 40, 30), $"Net Profit Earned: ₹{netOperatingProfitRupees:N2}", profitStyle);
            GUI.Label(new Rect(modalX + 20, modalY + 240, modalWidth - 40, 25), $"Driver XP Gained: +{driverXpAwarded} XP");

            if (GUI.Button(new Rect(modalX + (modalWidth - 220) * 0.5f, modalY + 285, 220, 45), "CONTINUE TO HQ"))
            {
                Time.timeScale = 1.0f;
                isTripSummaryVisible = false;
                SceneManager.LoadScene(0); // Load Main Menu
            }
        }
    }
}
