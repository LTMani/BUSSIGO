using System;

namespace Bussigo.Game.Progression
{

    public class DriverCommercialEndorsementModel32
    {
        public string EndorsementCode => "ENDORSE-RTO-AP-032";
        public EndorsementSpecialization Specialization { get; set; } = (EndorsementSpecialization)(0);
        public int RequiredDriverXP { get; set; } = 53000;
        public float SafetyBonusMultiplier { get; set; } = 1.10f;

        public bool IsEligibleForEndorsement(long currentDriverXP, int totalCleanTripsCount)
        {
            return currentDriverXP >= RequiredDriverXP && totalCleanTripsCount >= 25;
        }
    }
}
