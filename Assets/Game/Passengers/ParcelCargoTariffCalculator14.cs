using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Passengers
{
    public class CommercialCargoConsignment
    {
        public string ConsignmentTrackingCode { get; set; }
        public string ConsignorName { get; set; }
        public float WeightKg { get; set; }
        public float VolumeM3 { get; set; }
        public string OriginCity { get; set; }
        public string DestinationCity { get; set; }
        public float FreightChargesRupees { get; set; }
    }

    public class ParcelCargoTariffCalculator14
    {
        public float BaseFreightRatePerKgPer100Km { get; set; } = 6.10f;
        public float MinimumFreightDocketChargeRupees { get; set; } = 150.0f;
        public float ExpressParcelSurchargePercent { get; set; } = 25.0f;

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
