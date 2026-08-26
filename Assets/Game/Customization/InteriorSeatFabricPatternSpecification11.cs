using System;

namespace Bussigo.Game.Customization
{
    public enum FabricTextureType
    {
        ClassicAPSRTCVelourPattern,
        RoyalHeritageFloralWeave,
        ExecutiveSyntheticLeatherette,
        PremiumMemoryFoamSleeper
    }

    public class InteriorSeatFabricPatternSpecification11
    {
        public string FabricCode => "FABRIC-INTERIOR-STYLE-011";
        public FabricTextureType PatternType { get; set; } = (FabricTextureType)(3);
        public float ComfortRatingBonusScore { get; set; } = 4.3f;
        public float WearDurabilityRating01 { get; set; } = 0.94f;
        public float CostPerSeatRupees { get; set; } = 2950.00f;

        public float CalculateTotalBusRefitCost(int seatingCapacity)
        {
            return seatingCapacity * CostPerSeatRupees;
        }
    }
}
