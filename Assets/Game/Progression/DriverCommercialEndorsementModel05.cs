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

    public class DriverCommercialEndorsementModel05
    {
        public string EndorsementCode => "ENDORSE-RTO-AP-005";
        public EndorsementSpecialization Specialization { get; set; } = (EndorsementSpecialization)(1);
        public int RequiredDriverXP { get; set; } = 12500;
        public float SafetyBonusMultiplier { get; set; } = 1.15f;

        public bool IsEligibleForEndorsement(long currentDriverXP, int totalCleanTripsCount)
        {
            return currentDriverXP >= RequiredDriverXP && totalCleanTripsCount >= 25;
        }
    }
}
