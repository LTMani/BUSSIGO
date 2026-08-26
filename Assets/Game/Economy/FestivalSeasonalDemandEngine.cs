using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Economy
{
    public enum SouthIndianFestivalSeason
    {
        NormalRegularSeason,
        SankrantiHarvestFestival, // Mid-January: 300% travel surge from Hyderabad to Coastal AP
        MahaShivaratriSrisailam,  // February/March: Huge temple pilgrimage rush
        SummerSchoolVacations,    // May/June: Intercity tourism surge
        DasaraVijayawadaFestival, // October: Kanaka Durga Temple Vijayawada surge
        DiwaliDeepavaliHomecoming,// November: High passenger load factor
        AyyappaSwamyPilgrimageSeason // Dec/Jan: Special long distance charter runs
    }

    public class FestivalSeasonalDemandEngine
    {
        public SouthIndianFestivalSeason ActiveFestivalSeason { get; private set; } = SouthIndianFestivalSeason.NormalRegularSeason;
        public float DemandMultiplier { get; private set; } = 1.0f;
        public float MaxSurgePriceCap { get; private set; } = 1.0f;

        public void SetFestivalSeason(SouthIndianFestivalSeason season)
        {
            ActiveFestivalSeason = season;
            switch (season)
            {
                case SouthIndianFestivalSeason.SankrantiHarvestFestival:
                    DemandMultiplier = 3.2f;
                    MaxSurgePriceCap = 1.50f; // Legal dynamic pricing cap
                    break;
                case SouthIndianFestivalSeason.DasaraVijayawadaFestival:
                    DemandMultiplier = 2.4f;
                    MaxSurgePriceCap = 1.35f;
                    break;
                case SouthIndianFestivalSeason.DiwaliDeepavaliHomecoming:
                    DemandMultiplier = 2.1f;
                    MaxSurgePriceCap = 1.30f;
                    break;
                case SouthIndianFestivalSeason.MahaShivaratriSrisailam:
                    DemandMultiplier = 2.8f; // Heavy ghat road demand
                    MaxSurgePriceCap = 1.40f;
                    break;
                case SouthIndianFestivalSeason.SummerSchoolVacations:
                    DemandMultiplier = 1.65f;
                    MaxSurgePriceCap = 1.20f;
                    break;
                case SouthIndianFestivalSeason.AyyappaSwamyPilgrimageSeason:
                    DemandMultiplier = 1.9f;
                    MaxSurgePriceCap = 1.25f;
                    break;
                case SouthIndianFestivalSeason.NormalRegularSeason:
                default:
                    DemandMultiplier = 1.0f;
                    MaxSurgePriceCap = 1.0f;
                    break;
            }
        }

        public float CalculateDynamicTicketFare(float baseFareRupees, float seatOccupancyRatio)
        {
            float occupancySurge = 1.0f;
            if (seatOccupancyRatio > 0.70f)
            {
                occupancySurge = 1.0f + (seatOccupancyRatio - 0.70f) * 0.8f;
            }

            float rawFare = baseFareRupees * occupancySurge * DemandMultiplier;
            float cappedFare = MathF.Min(rawFare, baseFareRupees * MaxSurgePriceCap);
            return MathF.Max(baseFareRupees * 0.85f, cappedFare); // Never below 15% discount
        }
    }
}
