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

    public class DriverCommercialEndorsementModel12
    {
        public string EndorsementCode => "ENDORSE-RTO-AP-012";
        public EndorsementSpecialization Specialization { get; set; } = (EndorsementSpecialization)(0);
        public int RequiredDriverXP { get; set; } = 23000;
        public float SafetyBonusMultiplier { get; set; } = 1.10f;

        public bool IsEligibleForEndorsement(long currentDriverXP, int totalCleanTripsCount)
        {
            return currentDriverXP >= RequiredDriverXP && totalCleanTripsCount >= 25;
        }
    }
}
