using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Progression
{
    public class DriverProgressionEngine
    {
        public int DriverLevel { get; private set; } = 1;
        public long CurrentXp { get; private set; } = 0;
        public long XpRequiredForNextLevel => (long)(1000 * MathF.Pow(DriverLevel, 1.45f));

        public event Action<int> OnLevelUp;

        public void AddXp(long amount)
        {
            if (amount <= 0) return;
            CurrentXp += amount;

            while (CurrentXp >= XpRequiredForNextLevel)
            {
                CurrentXp -= XpRequiredForNextLevel;
                DriverLevel++;
                OnLevelUp?.Invoke(DriverLevel);
            }
        }
    }
}
