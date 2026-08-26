using System;
using UnityEngine;

namespace Bussigo.Company
{
    [Serializable]
    public class DriverProfile
    {
        public string driverID;
        public string driverName;
        public int experienceLevel = 1; // 1 to 10
        public double monthlySalaryRupees = 28000.0; // ₹28,000 / month
        public float drivingSkillRating = 4.2f; // 1.0 to 5.0
        [Range(0f, 100f)] public float fatiguePercent = 0f;
        public string assignedBusID = "";
        public string assignedRouteID = "ROUTE_NH65_VJA_HYD";
        public int totalCompletedTrips = 0;

        public DriverProfile() { }

        public DriverProfile(string id, string name, int exp, double salary, float skill)
        {
            driverID = id;
            driverName = name;
            experienceLevel = exp;
            monthlySalaryRupees = salary;
            drivingSkillRating = skill;
            fatiguePercent = 0f;
        }

        public void AddDrivingFatigue(float tripHours)
        {
            // 8% fatigue per hour of driving
            fatiguePercent = Mathf.Clamp(fatiguePercent + (tripHours * 8.0f), 0f, 100f);
        }

        public void RestAndRecover()
        {
            fatiguePercent = 0f;
        }
    }
}
