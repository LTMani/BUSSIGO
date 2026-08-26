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

    public class InteriorSeatFabricPatternSpecification23
    {
        public string FabricCode => "FABRIC-INTERIOR-STYLE-023";
        public FabricTextureType PatternType { get; set; } = (FabricTextureType)(3);
        public float ComfortRatingBonusScore { get; set; } = 5.9f;
        public float WearDurabilityRating01 { get; set; } = 0.94f;
        public float CostPerSeatRupees { get; set; } = 2950.00f;

        public float CalculateTotalBusRefitCost(int seatingCapacity)
        {
            return seatingCapacity * CostPerSeatRupees;
        }
    }
}
