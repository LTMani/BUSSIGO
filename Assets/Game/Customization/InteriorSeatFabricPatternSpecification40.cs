using System;

namespace Bussigo.Game.Customization
{

    public class InteriorSeatFabricPatternSpecification40
    {
        public string FabricCode => "FABRIC-INTERIOR-STYLE-040";
        public FabricTextureType PatternType { get; set; } = (FabricTextureType)(0);
        public float ComfortRatingBonusScore { get; set; } = 3.5f;
        public float WearDurabilityRating01 { get; set; } = 0.85f;
        public float CostPerSeatRupees { get; set; } = 2600.00f;

        public float CalculateTotalBusRefitCost(int seatingCapacity)
        {
            return seatingCapacity * CostPerSeatRupees;
        }
    }
}
