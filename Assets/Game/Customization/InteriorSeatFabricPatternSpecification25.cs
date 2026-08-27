using System;

namespace Bussigo.Game.Customization
{

    public class InteriorSeatFabricPatternSpecification25
    {
        public string FabricCode => "FABRIC-INTERIOR-STYLE-025";
        public FabricTextureType PatternType { get; set; } = (FabricTextureType)(1);
        public float ComfortRatingBonusScore { get; set; } = 3.5f;
        public float WearDurabilityRating01 { get; set; } = 0.88f;
        public float CostPerSeatRupees { get; set; } = 1550.00f;

        public float CalculateTotalBusRefitCost(int seatingCapacity)
        {
            return seatingCapacity * CostPerSeatRupees;
        }
    }
}
