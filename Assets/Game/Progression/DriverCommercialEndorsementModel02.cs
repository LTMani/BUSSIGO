using System;

namespace Bussigo.Game.Progression
{

    public class DriverCommercialEndorsementModel02
    {
        public string EndorsementCode => "ENDORSE-RTO-AP-002";
        public EndorsementSpecialization Specialization { get; set; } = (EndorsementSpecialization)(2);
        public int RequiredDriverXP { get; set; } = 8000;
        public float SafetyBonusMultiplier { get; set; } = 1.20f;

        public bool IsEligibleForEndorsement(long currentDriverXP, int totalCleanTripsCount)
        {
            return currentDriverXP >= RequiredDriverXP && totalCleanTripsCount >= 25;
        }
    }
}
