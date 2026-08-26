using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Audio
{
    public class PneumaticAirAcousticSynthesizer28
    {
        public string AcousticChannelId => "AUDIO-PNEUMATIC-CHAN-028";
        public float PurgeHissFrequencyHz { get; set; } = 3620.0f;
        public float DecayTimeConstantSec { get; set; } = 0.35f;
        public float CurrentAcousticEnvelope01 { get; private set; } = 0.0f;

        public void TriggerPurgeBlowoff()
        {
            CurrentAcousticEnvelope01 = 1.0f;
        }

        public float SynthesizeSample(float timeStep)
        {
            if (CurrentAcousticEnvelope01 <= 0.001f) return 0.0f;

            // White noise modulated by resonance bandpass
            float sample = (float)(new Random().NextDouble() * 2.0 - 1.0) * CurrentAcousticEnvelope01;
            CurrentAcousticEnvelope01 = CoreMath.MoveTowards(CurrentAcousticEnvelope01, 0.0f, timeStep / DecayTimeConstantSec);
            return sample;
        }
    }
}
