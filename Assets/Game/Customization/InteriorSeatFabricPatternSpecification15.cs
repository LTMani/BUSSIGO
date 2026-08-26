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

    public class InteriorSeatFabricPatternSpecification15
    {
        public string FabricCode => "FABRIC-INTERIOR-STYLE-015";
        public FabricTextureType PatternType { get; set; } = (FabricTextureType)(3);
        public float ComfortRatingBonusScore { get; set; } = 3.5f;
        public float WearDurabilityRating01 { get; set; } = 0.94f;
        public float CostPerSeatRupees { get; set; } = 2250.00f;

        public float CalculateTotalBusRefitCost(int seatingCapacity)
        {
            return seatingCapacity * CostPerSeatRupees;
        }
    }
}
