using System;

namespace Bussigo.Game.Progression
{

    public class DriverCommercialEndorsementModel09
    {
        public string EndorsementCode => "ENDORSE-RTO-AP-009";
        public EndorsementSpecialization Specialization { get; set; } = (EndorsementSpecialization)(1);
        public int RequiredDriverXP { get; set; } = 18500;
        public float SafetyBonusMultiplier { get; set; } = 1.15f;

        public bool IsEligibleForEndorsement(long currentDriverXP, int totalCleanTripsCount)
        {
            return currentDriverXP >= RequiredDriverXP && totalCleanTripsCount >= 25;
        }
    }
}
