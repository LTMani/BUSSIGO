using System;

namespace Bussigo.Game.Customization
{

    public class InteriorSeatFabricPatternSpecification27
    {
        public string FabricCode => "FABRIC-INTERIOR-STYLE-027";
        public FabricTextureType PatternType { get; set; } = (FabricTextureType)(3);
        public float ComfortRatingBonusScore { get; set; } = 5.1f;
        public float WearDurabilityRating01 { get; set; } = 0.94f;
        public float CostPerSeatRupees { get; set; } = 2250.00f;

        public float CalculateTotalBusRefitCost(int seatingCapacity)
        {
            return seatingCapacity * CostPerSeatRupees;
        }
    }
}
