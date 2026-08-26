using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Company
{
    public class DriverSkillTreeConfiguration13
    {
        public string SkillTreeId => "SKILL-TREE-DRIVER-013";
        public int EcoDrivingHypermilingLevel { get; set; } = 4;
        public int MountainGhatRoadMasteryLevel { get; set; } = 5;
        public int PunctualityExpressNavigatorLevel { get; set; } = 1;
        public int PassengerCareCustomerServiceLevel { get; set; } = 2;

        public float GetFuelSavingsPercentage()
        {
            return EcoDrivingHypermilingLevel * 3.5f; // Up to 17.5% diesel reduction
        }

        public float GetComfortScoreBonus()
        {
            return PassengerCareCustomerServiceLevel * 2.8f;
        }

        public float GetGhatDescentSafetyMultiplier()
        {
            return 1.0f + MountainGhatRoadMasteryLevel * 0.15f;
        }
    }
}
