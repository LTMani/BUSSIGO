using System;

namespace Bussigo.Game.Customization
{

    public class InteriorSeatFabricPatternSpecification21
    {
        public string FabricCode => "FABRIC-INTERIOR-STYLE-021";
        public FabricTextureType PatternType { get; set; } = (FabricTextureType)(1);
        public float ComfortRatingBonusScore { get; set; } = 4.3f;
        public float WearDurabilityRating01 { get; set; } = 0.88f;
        public float CostPerSeatRupees { get; set; } = 2250.00f;

        public float CalculateTotalBusRefitCost(int seatingCapacity)
        {
            return seatingCapacity * CostPerSeatRupees;
        }
    }
}
