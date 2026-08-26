using System;

namespace Bussigo.Game.Core
{
    public class GameClock
    {
        public float TimeOfDaySeconds { get; private set; } // 0 to 86400 (24h)
        public float TimeScale { get; set; } = 1.0f;
        public bool IsPaused { get; set; } = false;
        public int DayCount { get; private set; } = 1;

        public const float SecondsInDay = 86400.0f;

        public int Hours => (int)(TimeOfDaySeconds / 3600.0f) % 24;
        public int Minutes => (int)((TimeOfDaySeconds % 3600.0f) / 60.0f);
        public int Seconds => (int)(TimeOfDaySeconds % 60.0f);

        public string FormattedTime => $"{Hours:D2}:{Minutes:D2}:{Seconds:D2}";

        public GameClock(float initialHour = 6.0f)
        {
            TimeOfDaySeconds = initialHour * 3600.0f;
        }

        public void Advance(float deltaRealSeconds)
        {
            if (IsPaused) return;

            TimeOfDaySeconds += deltaRealSeconds * TimeScale;
            while (TimeOfDaySeconds >= SecondsInDay)
            {
                TimeOfDaySeconds -= SecondsInDay;
                DayCount++;
            }
            while (TimeOfDaySeconds < 0.0f)
            {
                TimeOfDaySeconds += SecondsInDay;
                DayCount = Math.Max(1, DayCount - 1);
            }
        }

        public void SetTime(float hour, float minute = 0f)
        {
            TimeOfDaySeconds = CoreMath.Clamp(hour * 3600f + minute * 60f, 0f, SecondsInDay);
        }
    }
}
