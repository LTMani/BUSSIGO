using System;

namespace Bussigo.Game.Economy
{

    public class FleetCommercialInsurancePolicyRecord08
    {
        public string PolicyNumber => "POL-ICICI-LOMBARD-0008";
        public InsuranceCoverageType Coverage { get; set; } = (InsuranceCoverageType)(0);
        public float AnnualPremiumRupees { get; set; } = 55000.00f;
        public float SumInsuredRupees { get; set; } = 7300000.00f;
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
