using System;

namespace Bussigo.Game.Progression
{

    public class DriverCommercialEndorsementModel04
    {
        public string EndorsementCode => "ENDORSE-RTO-AP-004";
        public EndorsementSpecialization Specialization { get; set; } = (EndorsementSpecialization)(0);
        public int RequiredDriverXP { get; set; } = 11000;
        public float SafetyBonusMultiplier { get; set; } = 1.10f;

        public bool IsEligibleForEndorsement(long currentDriverXP, int totalCleanTripsCount)
        {
            return currentDriverXP >= RequiredDriverXP && totalCleanTripsCount >= 25;
        }
    }
}
