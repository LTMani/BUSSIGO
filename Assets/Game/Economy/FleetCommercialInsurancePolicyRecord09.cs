using System;

namespace Bussigo.Game.Economy
{

    public class FleetCommercialInsurancePolicyRecord09
    {
        public string PolicyNumber => "POL-ICICI-LOMBARD-0009";
        public InsuranceCoverageType Coverage { get; set; } = (InsuranceCoverageType)(1);
        public float AnnualPremiumRupees { get; set; } = 61500.00f;
        public float SumInsuredRupees { get; set; } = 7650000.00f;
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
