using System;

namespace Bussigo.Game.Passengers
{
    public class CommercialCargoConsignment
    {
        public string ConsignmentTrackingCode { get; set; }
        public string ConsignorName { get; set; }
        public float WeightKg { get; set; }
        public float VolumeM3 { get; set; }
        public string OriginCity { get; set; }
        public string DestinationCity { get; set; }
        public float FreightChargesRupees { get; set; }
    }
}
