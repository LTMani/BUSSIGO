using System;
using UnityEngine;
using Bussigo.Core;
using Bussigo.Vehicle;
using Bussigo.Route;
using Bussigo.Navigation;
using Bussigo.Weather;
using Bussigo.Passengers;
using Bussigo.Economy;

namespace Bussigo.UI
{
    [Serializable]
    public struct HUDDisplayState
    {
        public float speedKmh;
        public float engineRpm;
        public int currentGear;
        public float airPressureBar;
        public int retarderLevel;
        public bool isDoorOpen;
        public float remainingDistanceKm;
        public float traveledDistanceKm;
        public string currentLocation;
        public string destination;
        public int passengerCount;
        public float satisfactionPercent;
        public double companyBalanceRupees;
        public string timeOfDayString;
        public string weatherName;
    }

    /// <summary>
    /// Master in-game simulator HUD controller aggregating telemetry from physics, navigation, weather, and economy.
    /// </summary>
    public class TripHUDController : MonoBehaviour
    {
        public BusChassisController chassis;
        public GPSRouteService gpsService;
        public TimeOfDayService timeService;
        public DynamicWeatherManager weatherManager;
        public PassengerManager passengerManager;
        public EconomyManager economyManager;

        public HUDDisplayState CurrentState { get; private set; }

        private void Update()
        {
            float speed = (chassis != null) ? Mathf.Abs(chassis.currentSpeedKmh) : 0f;
            float rpm = (chassis != null) ? chassis.currentEngineRpm : 650f;
            int gear = (chassis != null) ? chassis.currentGear : 1;
            float air = (chassis != null) ? chassis.primaryAirPressureBar : 8.5f;
            int retarder = (chassis != null) ? chassis.retarderLevel : 0;
            bool door = (chassis != null) && chassis.isDoorOpen;

            float remKm = (gpsService != null) ? gpsService.CurrentTelemetry.physicalRemainingKm : 274.85f;
            float travKm = (gpsService != null) ? gpsService.CurrentTelemetry.physicalTraveledKm : 0f;
            string loc = (gpsService != null) ? gpsService.CurrentTelemetry.currentLocationName : "Vijayawada PNBS";
            string dest = (gpsService != null) ? gpsService.CurrentTelemetry.destinationName : "Hyderabad MGBS";

            int pax = (passengerManager != null) ? passengerManager.OnboardPassengerCount : 0;
            float sat = (passengerManager != null) ? passengerManager.AverageSatisfaction : 100f;
            double bal = (economyManager != null) ? economyManager.CurrentBalance : 250000.0;

            float hour = (timeService != null) ? timeService.currentHour : 14.5f;
            int h = (int)hour;
            int m = (int)((hour - h) * 60f);
            string timeStr = $"{h:D2}:{m:D2}";

            string weather = (weatherManager != null && weatherManager.activeProfile != null) ? weatherManager.activeProfile.conditionName : "Clear";

            CurrentState = new HUDDisplayState
            {
                speedKmh = speed,
                engineRpm = rpm,
                currentGear = gear,
                airPressureBar = air,
                retarderLevel = retarder,
                isDoorOpen = door,
                remainingDistanceKm = remKm,
                traveledDistanceKm = travKm,
                currentLocation = loc,
                destination = dest,
                passengerCount = pax,
                satisfactionPercent = sat,
                companyBalanceRupees = bal,
                timeOfDayString = timeStr,
                weatherName = weather
            };
        }

        private void OnGUI()
        {
            // Top Header: Route & Time
            GUI.Box(new Rect(10, 10, 440, 95), "");
            GUI.Label(new Rect(20, 15, 420, 25), $"<b><size=14>BUSSIGO ROYAL TRAVELS -- NH65 HIGHWAY CORRIDOR</size></b>");
            GUI.Label(new Rect(20, 38, 420, 20), $"Route: <b>{CurrentState.currentLocation}</b>  -->  <b>{CurrentState.destination}</b>");
            GUI.Label(new Rect(20, 58, 420, 20), $"Distance: <b>{CurrentState.traveledDistanceKm:F1} km</b> traveled | <b>{CurrentState.remainingDistanceKm:F1} km</b> remaining");
            GUI.Label(new Rect(20, 78, 420, 20), $"Time: <b>{CurrentState.timeOfDayString}</b> | Weather: <b>{CurrentState.weatherName}</b> | Cash: <color=#90EE90>₹{CurrentState.companyBalanceRupees:N0}</color>");

            // Bottom Center: Cockpit Cluster
            float screenW = Screen.width;
            float screenH = Screen.height;
            float clusterW = 580;
            float clusterH = 110;
            float clusterX = (screenW - clusterW) * 0.5f;
            float clusterY = screenH - clusterH - 15;

            GUI.Box(new Rect(clusterX, clusterY, clusterW, clusterH), "");
            GUI.Label(new Rect(clusterX + 15, clusterY + 10, 550, 28), $"<size=18><b>SPEED: {CurrentState.speedKmh:F0} km/h</b>   |   <b>GEAR: D{CurrentState.currentGear}</b>   |   <b>RPM: {CurrentState.engineRpm:F0}</b></size>");
            GUI.Label(new Rect(clusterX + 15, clusterY + 40, 550, 22), $"Air Pressure: <b>{CurrentState.airPressureBar:F1} bar</b> {(CurrentState.airPressureBar < 6.5f ? "<color=red>[LOW AIR]</color>" : "<color=green>[OPTIMAL]</color>")}   |   Retarder: <b>Stage {CurrentState.retarderLevel}</b>");
            GUI.Label(new Rect(clusterX + 15, clusterY + 62, 550, 22), $"Passengers: <b>{CurrentState.passengerCount} / 44</b>   |   Satisfaction: <b>{CurrentState.satisfactionPercent:F0}%</b>   |   Doors: {(CurrentState.isDoorOpen ? "<color=yellow>OPEN</color>" : "<color=green>CLOSED</color>")}");
            GUI.Label(new Rect(clusterX + 15, clusterY + 84, 550, 20), $"<color=#DDDDDD><size=11>Controls: [W/S] Throttle/Brake | [A/D] Steer | [Space] Handbrake | [E] Door | [C] Camera | [H] Horn | [L] Lights</size></color>");
        }
    }
}
