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

    public class InteriorSeatFabricPatternSpecification24
    {
        public string FabricCode => "FABRIC-INTERIOR-STYLE-024";
        public FabricTextureType PatternType { get; set; } = (FabricTextureType)(0);
        public float ComfortRatingBonusScore { get; set; } = 6.7f;
        public float WearDurabilityRating01 { get; set; } = 0.85f;
        public float CostPerSeatRupees { get; set; } = 1200.00f;

        public float CalculateTotalBusRefitCost(int seatingCapacity)
        {
            return seatingCapacity * CostPerSeatRupees;
        }
    }
}
