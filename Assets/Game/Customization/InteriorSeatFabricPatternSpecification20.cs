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

    public class InteriorSeatFabricPatternSpecification20
    {
        public string FabricCode => "FABRIC-INTERIOR-STYLE-020";
        public FabricTextureType PatternType { get; set; } = (FabricTextureType)(0);
        public float ComfortRatingBonusScore { get; set; } = 3.5f;
        public float WearDurabilityRating01 { get; set; } = 0.85f;
        public float CostPerSeatRupees { get; set; } = 1900.00f;

        public float CalculateTotalBusRefitCost(int seatingCapacity)
        {
            return seatingCapacity * CostPerSeatRupees;
        }
    }
}
