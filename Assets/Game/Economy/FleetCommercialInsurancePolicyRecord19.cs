using System;

namespace Bussigo.Game.Economy
{

    public class FleetCommercialInsurancePolicyRecord19
    {
        public string PolicyNumber => "POL-ICICI-LOMBARD-0019";
        public InsuranceCoverageType Coverage { get; set; } = (InsuranceCoverageType)(3);
        public float AnnualPremiumRupees { get; set; } = 48500.00f;
        public float SumInsuredRupees { get; set; } = 11150000.00f;
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
