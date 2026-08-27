using System;

namespace Bussigo.Game.Progression
{

    public class DriverCommercialEndorsementModel33
    {
        public string EndorsementCode => "ENDORSE-RTO-AP-033";
        public EndorsementSpecialization Specialization { get; set; } = (EndorsementSpecialization)(1);
        public int RequiredDriverXP { get; set; } = 54500;
        public float SafetyBonusMultiplier { get; set; } = 1.15f;

        public bool IsEligibleForEndorsement(long currentDriverXP, int totalCleanTripsCount)
        {
            return currentDriverXP >= RequiredDriverXP && totalCleanTripsCount >= 25;
        }
    }
}
