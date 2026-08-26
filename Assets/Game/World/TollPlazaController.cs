using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.World
{
    public enum TollPaymentType
    {
        FASTagRFID,
        CashBoothManual,
        VIPExempt
    }

    public class TollLane
    {
        public int LaneNumber { get; set; }
        public TollPaymentType PaymentType { get; set; } = TollPaymentType.FASTagRFID;
        public bool BarrierArmRaised { get; set; } = false;
        public bool VehicleDetected { get; set; } = false;
        public float TollFeeInRupees { get; set; } = 110.0f;
    }

    public class TollPlazaController
    {
        public string PlazaName { get; set; } = "Kaza Toll Plaza (NH16)";
        public List<TollLane> Lanes { get; } = new List<TollLane>();
        public float StandardBusTollFee { get; set; } = 110.0f;

        public event Action<string, float> OnTollPaid;

        public TollPlazaController(string name, int numLanes = 6)
        {
            PlazaName = name;
            for (int i = 1; i <= numLanes; i++)
            {
                Lanes.Add(new TollLane
                {
                    LaneNumber = i,
                    PaymentType = (i <= 4) ? TollPaymentType.FASTagRFID : TollPaymentType.CashBoothManual,
                    TollFeeInRupees = StandardBusTollFee
                });
            }
        }

        public bool ProcessFASTagDeduction(int laneIndex, float fastagBalance)
        {
            if (laneIndex < 0 || laneIndex >= Lanes.Count) return false;
            var lane = Lanes[laneIndex];

            if (fastagBalance >= lane.TollFeeInRupees)
            {
                lane.BarrierArmRaised = true;
                OnTollPaid?.Invoke(PlazaName, lane.TollFeeInRupees);
                return true;
            }

            lane.BarrierArmRaised = false;
            return false;
        }
    }
}
