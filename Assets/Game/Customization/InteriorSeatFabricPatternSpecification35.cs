using System;

namespace Bussigo.Game.Customization
{

    public class InteriorSeatFabricPatternSpecification35
    {
        public string FabricCode => "FABRIC-INTERIOR-STYLE-035";
        public FabricTextureType PatternType { get; set; } = (FabricTextureType)(3);
        public float ComfortRatingBonusScore { get; set; } = 3.5f;
        public float WearDurabilityRating01 { get; set; } = 0.94f;
        public float CostPerSeatRupees { get; set; } = 2950.00f;

        public float CalculateTotalBusRefitCost(int seatingCapacity)
        {
            return seatingCapacity * CostPerSeatRupees;
        }
    }
}
