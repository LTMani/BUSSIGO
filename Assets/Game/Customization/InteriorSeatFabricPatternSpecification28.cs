using System;

namespace Bussigo.Game.Customization
{

    public class InteriorSeatFabricPatternSpecification28
    {
        public string FabricCode => "FABRIC-INTERIOR-STYLE-028";
        public FabricTextureType PatternType { get; set; } = (FabricTextureType)(0);
        public float ComfortRatingBonusScore { get; set; } = 5.9f;
        public float WearDurabilityRating01 { get; set; } = 0.85f;
        public float CostPerSeatRupees { get; set; } = 2600.00f;

        public float CalculateTotalBusRefitCost(int seatingCapacity)
        {
            return seatingCapacity * CostPerSeatRupees;
        }
    }
}
