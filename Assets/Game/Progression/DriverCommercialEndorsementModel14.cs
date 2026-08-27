using System;

namespace Bussigo.Game.Progression
{

    public class DriverCommercialEndorsementModel14
    {
        public string EndorsementCode => "ENDORSE-RTO-AP-014";
        public EndorsementSpecialization Specialization { get; set; } = (EndorsementSpecialization)(2);
        public int RequiredDriverXP { get; set; } = 26000;
        public float SafetyBonusMultiplier { get; set; } = 1.20f;

        public bool IsEligibleForEndorsement(long currentDriverXP, int totalCleanTripsCount)
        {
            return currentDriverXP >= RequiredDriverXP && totalCleanTripsCount >= 25;
        }
    }
}
