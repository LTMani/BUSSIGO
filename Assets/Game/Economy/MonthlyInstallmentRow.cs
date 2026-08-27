using System;

namespace Bussigo.Game.Economy
{
    public struct MonthlyInstallmentRow
    {
        public int MonthIndex;
        public float MonthlyPaymentEmi;
        public float PrincipalPortion;
        public float InterestPortion;
        public float RemainingPrincipalBalance;
    }
}
