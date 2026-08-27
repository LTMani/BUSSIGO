using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Passengers
{
    public class ParcelCargoTariffCalculator25
    {
        public float BaseFreightRatePerKgPer100Km { get; set; } = 14.50f;
        public float MinimumFreightDocketChargeRupees { get; set; } = 870.0f;
        public float ExpressParcelSurchargePercent { get; set; } = 145.0f;

        public float CalculateFreightFare(float weightKg, float distanceKm, bool isExpressDelivery)
        {
            float ratePerKg = BaseFreightRatePerKgPer100Km * (distanceKm / 100.0f);
            float rawFare = weightKg * ratePerKg;

            if (isExpressDelivery)
            {
                rawFare *= (1.0f + ExpressParcelSurchargePercent / 100.0f);
            }

            return MathF.Max(MinimumFreightDocketChargeRupees, rawFare);
        }
    }
}
