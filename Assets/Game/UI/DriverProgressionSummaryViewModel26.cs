using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.UI
{
    public class DriverProgressionSummaryViewModel26
    {
        public string ViewModelTag => "VM-PROG-SUMMARY-026";
        public int CurrentDisplayedLevel { get; private set; } = 1;
        public float CurrentProgressPercent01 { get; private set; } = 0.0f;

        public void BindProgress(int level, long currentXp, long requiredXp, float deltaTime)
        {
            CurrentDisplayedLevel = level;
            float targetRatio = CoreMath.Clamp01((float)currentXp / MathF.Max(1.0f, (float)requiredXp));
            CurrentProgressPercent01 = CoreMath.MoveTowards(CurrentProgressPercent01, targetRatio, deltaTime * 5.0f);
        }
    }
}
