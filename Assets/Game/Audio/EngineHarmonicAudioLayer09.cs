using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Audio
{
    public class EngineHarmonicAudioLayer09
    {
        public int HarmonicOrder => 9;
        public float FundamentalCylinderFiringFrequencyHz(float engineRpm, int cylinderCount = 6)
        {
            float fundamentalHz = (engineRpm / 60.0f) * (cylinderCount / 2.0f);
            return fundamentalHz * HarmonicOrder;
        }

        public float CalculateHarmonicGain(float engineLoad01)
        {
            float baseGain = 1.0f / (HarmonicOrder * 0.8f);
            float loadBoost = engineLoad01 * 0.35f;
            return MathF.Min(1.0f, baseGain + loadBoost);
        }
    }
}
