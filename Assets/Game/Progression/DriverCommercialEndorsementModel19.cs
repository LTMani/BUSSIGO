using System;

namespace Bussigo.Game.Progression
{

    public class DriverCommercialEndorsementModel19
    {
        public string EndorsementCode => "ENDORSE-RTO-AP-019";
        public EndorsementSpecialization Specialization { get; set; } = (EndorsementSpecialization)(3);
        public int RequiredDriverXP { get; set; } = 33500;
        public float SafetyBonusMultiplier { get; set; } = 1.25f;

        public bool IsEligibleForEndorsement(long currentDriverXP, int totalCleanTripsCount)
        {
            return currentDriverXP >= RequiredDriverXP && totalCleanTripsCount >= 25;
        }
    }
}
