using System;

namespace Bussigo.Game.Progression
{
    public enum EndorsementSpecialization
    {
        HillGhatRoadCertified,
        OvernightMonsoonSpecialist,
        MultiAxleVolvo14MCoach,
        VIPCharterExecutive
    }

    public class DriverCommercialEndorsementModel10
    {
        public string EndorsementCode => "ENDORSE-RTO-AP-010";
        public EndorsementSpecialization Specialization { get; set; } = (EndorsementSpecialization)(2);
        public int RequiredDriverXP { get; set; } = 20000;
        public float SafetyBonusMultiplier { get; set; } = 1.20f;

        public bool IsEligibleForEndorsement(long currentDriverXP, int totalCleanTripsCount)
        {
            return currentDriverXP >= RequiredDriverXP && totalCleanTripsCount >= 25;
        }
    }
}
