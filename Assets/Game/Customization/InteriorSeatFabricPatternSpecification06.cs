using System;

namespace Bussigo.Game.Customization
{

    public class InteriorSeatFabricPatternSpecification06
    {
        public string FabricCode => "FABRIC-INTERIOR-STYLE-006";
        public FabricTextureType PatternType { get; set; } = (FabricTextureType)(2);
        public float ComfortRatingBonusScore { get; set; } = 4.3f;
        public float WearDurabilityRating01 { get; set; } = 0.91f;
        public float CostPerSeatRupees { get; set; } = 1200.00f;

        public float CalculateTotalBusRefitCost(int seatingCapacity)
        {
            return seatingCapacity * CostPerSeatRupees;
        }
    }
}
