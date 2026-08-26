using System;
using UnityEngine;

namespace Bussigo.Company
{
    public enum BusChassisType
    {
        Standard12MCoach = 0,
        MultiAxle13_8MCoach = 1,
        SleeperDoubleDecker = 2
    }

    [Serializable]
    public class FleetBusData
    {
        public string busID;
        public string modelName;
        public string registrationNumber;
        public BusChassisType chassisType;
        public int seatingCapacity = 44;
        public double purchasePriceRupees;
        public float odometerKm = 0f;
        [Range(0f, 100f)] public float mechanicalConditionPercent = 100f;
        public float fuelTankCapacityLitres = 350f;
        public float currentFuelLitres = 350f;
        public bool isAssignedToActiveTrip = false;
        public string assignedDriverID = "";

        public FleetBusData() { }

        public FleetBusData(string id, string model, string regNum, BusChassisType type, double price, int seats = 44)
        {
            busID = id;
            modelName = model;
            registrationNumber = regNum;
            chassisType = type;
            purchasePriceRupees = price;
            seatingCapacity = seats;
            mechanicalConditionPercent = 100f;
            currentFuelLitres = 350f;
        }

        public void ConsumeFuel(float litres)
        {
            currentFuelLitres = Mathf.Max(0f, currentFuelLitres - litres);
        }

        public void AddTripMileage(float distanceKm)
        {
            odometerKm += distanceKm;
            // 0.015% condition wear per km driven
            mechanicalConditionPercent = Mathf.Clamp(mechanicalConditionPercent - (distanceKm * 0.015f), 10f, 100f);
        }

        public void PerformServiceOverhaul()
        {
            mechanicalConditionPercent = 100f;
            currentFuelLitres = fuelTankCapacityLitres;
        }
    }
}
