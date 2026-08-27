using System;

namespace Bussigo.Game.Economy
{

    public class FleetCommercialInsurancePolicyRecord01
    {
        public string PolicyNumber => "POL-ICICI-LOMBARD-0001";
        public InsuranceCoverageType Coverage { get; set; } = (InsuranceCoverageType)(1);
        public float AnnualPremiumRupees { get; set; } = 48500.00f;
        public float SumInsuredRupees { get; set; } = 4850000.00f;
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
