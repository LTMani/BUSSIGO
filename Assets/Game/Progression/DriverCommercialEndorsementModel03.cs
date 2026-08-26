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

    public class DriverCommercialEndorsementModel03
    {
        public string EndorsementCode => "ENDORSE-RTO-AP-003";
        public EndorsementSpecialization Specialization { get; set; } = (EndorsementSpecialization)(3);
        public int RequiredDriverXP { get; set; } = 9500;
        public float SafetyBonusMultiplier { get; set; } = 1.25f;

        public bool IsEligibleForEndorsement(long currentDriverXP, int totalCleanTripsCount)
        {
            return currentDriverXP >= RequiredDriverXP && totalCleanTripsCount >= 25;
        }
    }
}
