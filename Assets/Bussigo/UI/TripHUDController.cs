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
    }
}
