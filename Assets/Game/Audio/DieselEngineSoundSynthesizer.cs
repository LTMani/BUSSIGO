using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Audio
{
    public class DieselEngineSoundSynthesizer
    {
        public float CurrentPitch { get; private set; } = 1.0f;
        public float CurrentVolume { get; private set; } = 0.8f;
        public float TurboWhineVolume { get; private set; } = 0.0f;
        public float RetarderVolume { get; private set; } = 0.0f;

        public void UpdateAcoustics(float engineRpm, float engineLoadRatio, float turboBoostBar, float retarderLevel)
        {
            // Base pitch maps ~600 RPM (0.6x pitch) to 2400 RPM (2.0x pitch)
            CurrentPitch = 0.6f + (engineRpm / 2400.0f) * 1.4f;

            // Engine load acoustic thickness
            CurrentVolume = 0.4f + CoreMath.Clamp01(engineLoadRatio) * 0.6f;

            // Turbocharger spool whine
            TurboWhineVolume = CoreMath.Clamp01(turboBoostBar / 2.0f) * 0.75f;

            // Retarder electromagnetic whine
            RetarderVolume = CoreMath.Clamp01(retarderLevel) * 0.85f;
        }
    }
}
