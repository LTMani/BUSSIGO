using System;
using System.Collections.Generic;
using Bussigo.Game.Core;
using Bussigo.Game.Economy;

namespace Bussigo.Game.UI
{
    public class TycoonFinanceOverviewPresenter31
    {
        public string PresenterId => "UI-FIN-CARD-031";
        public float DisplayedBankBalanceRupees { get; private set; } = 0.0f;
        public float DisplayedDailyOperatingProfitRupees { get; private set; } = 0.0f;
        public float FleetUtilizationPercentage { get; private set; } = 84.7f;

        public void BindFinancialStream(float actualBankBalance, float dailyProfit, float deltaTime)
        {
            DisplayedBankBalanceRupees = CoreMath.MoveTowards(DisplayedBankBalanceRupees, actualBankBalance, deltaTime * 250000.0f);
            DisplayedDailyOperatingProfitRupees = CoreMath.MoveTowards(DisplayedDailyOperatingProfitRupees, dailyProfit, deltaTime * 50000.0f);
        }
    }
}
