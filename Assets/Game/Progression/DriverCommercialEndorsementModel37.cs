using System;

namespace Bussigo.Game.Progression
{

    public class DriverCommercialEndorsementModel37
    {
        public string EndorsementCode => "ENDORSE-RTO-AP-037";
        public EndorsementSpecialization Specialization { get; set; } = (EndorsementSpecialization)(1);
        public int RequiredDriverXP { get; set; } = 60500;
        public float SafetyBonusMultiplier { get; set; } = 1.15f;

        public bool IsEligibleForEndorsement(long currentDriverXP, int totalCleanTripsCount)
        {
            return currentDriverXP >= RequiredDriverXP && totalCleanTripsCount >= 25;
        }
    }
}
