using System;

namespace Bussigo.Game.Customization
{

    public class InteriorSeatFabricPatternSpecification13
    {
        public string FabricCode => "FABRIC-INTERIOR-STYLE-013";
        public FabricTextureType PatternType { get; set; } = (FabricTextureType)(1);
        public float ComfortRatingBonusScore { get; set; } = 5.9f;
        public float WearDurabilityRating01 { get; set; } = 0.88f;
        public float CostPerSeatRupees { get; set; } = 1550.00f;

        public float CalculateTotalBusRefitCost(int seatingCapacity)
        {
            return seatingCapacity * CostPerSeatRupees;
        }
    }
}
