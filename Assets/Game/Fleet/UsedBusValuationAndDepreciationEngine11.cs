using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Fleet
{
    public class UsedBusValuationAndDepreciationEngine11
    {
        public string ValuationModelId => "VALUATION-MODEL-011";
        public float AnnualDepreciationRatePercent { get; set; } = 17.0f;
        public float MinimumScrapValueResidualFloorPercent { get; set; } = 15.0f;

        public float CalculateResaleValue(float originalPurchasePriceCoins, float vehicleAgeYears, float totalOdometerKm, float mechanicalCondition01)
        {
            // Diminishing balance depreciation
            float ageFactor = MathF.Pow(1.0f - (AnnualDepreciationRatePercent / 100.0f), vehicleAgeYears);

            // Mileage impact (standard bus does ~120,000 km/year)
            float expectedKm = vehicleAgeYears * 120000.0f;
            float mileageRatio = totalOdometerKm / MathF.Max(10000.0f, expectedKm);
            float mileagePenalty = CoreMath.Clamp(1.0f - (mileageRatio - 1.0f) * 0.15f, 0.70f, 1.15f);

            // Condition multiplier
            float conditionMultiplier = 0.5f + mechanicalCondition01 * 0.5f;

            float calculatedValue = originalPurchasePriceCoins * ageFactor * mileagePenalty * conditionMultiplier;
            float minResidualValue = originalPurchasePriceCoins * (MinimumScrapValueResidualFloorPercent / 100.0f);

            return MathF.Max(minResidualValue, calculatedValue);
        }
    }
}
