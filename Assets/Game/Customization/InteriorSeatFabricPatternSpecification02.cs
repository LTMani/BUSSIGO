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

    public class InteriorSeatFabricPatternSpecification02
    {
        public string FabricCode => "FABRIC-INTERIOR-STYLE-002";
        public FabricTextureType PatternType { get; set; } = (FabricTextureType)(2);
        public float ComfortRatingBonusScore { get; set; } = 5.1f;
        public float WearDurabilityRating01 { get; set; } = 0.91f;
        public float CostPerSeatRupees { get; set; } = 1900.00f;

        public float CalculateTotalBusRefitCost(int seatingCapacity)
        {
            return seatingCapacity * CostPerSeatRupees;
        }
    }
}
