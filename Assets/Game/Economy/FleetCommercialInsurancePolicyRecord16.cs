using System;

namespace Bussigo.Game.Economy
{
    public enum InsuranceCoverageType
    {
        MandatoryThirdPartyLiability,
        ComprehensiveCommercialHull,
        DriverPassengerAccidentCover,
        AllRisksComprehensiveShield
    }

    public class FleetCommercialInsurancePolicyRecord16
    {
        public string PolicyNumber => "POL-ICICI-LOMBARD-0016";
        public InsuranceCoverageType Coverage { get; set; } = (InsuranceCoverageType)(0);
        public float AnnualPremiumRupees { get; set; } = 68000.00f;
        public float SumInsuredRupees { get; set; } = 10100000.00f;
        public float DeductiblePerClaimRupees { get; set; } = 15000.0f;
        public bool IsPolicyActive { get; set; } = true;

        public float ProcessAccidentClaim(float totalDamageAssessedRupees)
        {
            if (!IsPolicyActive || totalDamageAssessedRupees <= DeductiblePerClaimRupees)
            {
                return 0.0f;
            }

            float payable = totalDamageAssessedRupees - DeductiblePerClaimRupees;
            return MathF.Min(SumInsuredRupees, payable);
        }
    }
}
