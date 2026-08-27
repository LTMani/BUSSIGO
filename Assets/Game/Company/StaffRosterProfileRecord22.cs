using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Company
{

    public class StaffRosterProfileRecord22
    {
        public string EmployeeId => "EMP-SOUTH-022";
        public string FullName { get; set; } = "Transport Staff Member 22";
        public StaffRole Role { get; set; } = (StaffRole)(4);
        public float MonthlySalaryRupees { get; set; } = 49000.00f;
        public float FatigueLevel01 { get; set; } = 0.15f;
        public float SafetyRatingStars { get; set; } = 4.30f;
        public float FuelEfficiencySkill01 { get; set; } = 0.85f;
        public bool IsOnDutyShift { get; set; } = true;

        public void RestAndRecoverFatigue(float hoursRest)
        {
            FatigueLevel01 = MathF.Max(0.0f, FatigueLevel01 - (hoursRest / 8.0f));
        }

        public void AccumulateDrivingFatigue(float hoursDriven)
        {
            FatigueLevel01 = MathF.Min(1.0f, FatigueLevel01 + (hoursDriven / 9.5f));
        }
    }
}
