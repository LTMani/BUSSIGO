using System;
using UnityEngine;

namespace Bussigo.Game.Runtime3D.Vehicle
{
    public class ProceduralAudioClipSynthesizer : MonoBehaviour
    {
        public static AudioClip GenerateDieselEngineClip(int sampleRate = 44100, float lengthSeconds = 1.0f)
        {
            int sampleCount = (int)(sampleRate * lengthSeconds);
            float[] samples = new float[sampleCount];

            // 6-Cylinder 4-Stroke Diesel fundamental firing frequency (at ~700 RPM idle = ~35 Hz)
            float baseFreq = 35.0f;

            for (int i = 0; i < sampleCount; i++)
            {
                float t = (float)i / sampleRate;
                
                // Cylinder pressure pulses (harmonics 1x, 2x, 3x, 6x)
                float h1 = Mathf.Sin(2f * Mathf.PI * baseFreq * t) * 0.45f;
                float h2 = Mathf.Sin(2f * Mathf.PI * baseFreq * 2f * t) * 0.30f;
                float h3 = Mathf.Sin(2f * Mathf.PI * baseFreq * 3f * t) * 0.20f;
                float h6 = Mathf.Sin(2f * Mathf.PI * baseFreq * 6f * t) * 0.15f;
                
                // Diesel mechanical knock/clatter (white noise burst at firing top-dead-center)
                float cyclePhase = Mathf.Repeat(t * baseFreq * 3f, 1.0f);
                float knock = (cyclePhase < 0.15f) ? (UnityEngine.Random.value * 2f - 1f) * (1.0f - cyclePhase / 0.15f) * 0.25f : 0f;

                samples[i] = Mathf.Clamp(h1 + h2 + h3 + h6 + knock, -1.0f, 1.0f);
            }

            AudioClip clip = AudioClip.Create("Synthesized_DieselEngine", sampleCount, 1, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }

        public static AudioClip GenerateAirPurgeHissClip(int sampleRate = 44100, float lengthSeconds = 0.6f)
        {
            int sampleCount = (int)(sampleRate * lengthSeconds);
            float[] samples = new float[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                float t = (float)i / sampleRate;
                float envelope = Mathf.Exp(-t * 5.0f); // Quick exponential decay
                float whiteNoise = (UnityEngine.Random.value * 2f - 1f);
                samples[i] = whiteNoise * envelope * 0.75f;
            }

            AudioClip clip = AudioClip.Create("Synthesized_AirPurgeHiss", sampleCount, 1, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }

        public static AudioClip GenerateMelodicAirHornClip(int sampleRate = 44100, float lengthSeconds = 1.8f)
        {
            int sampleCount = (int)(sampleRate * lengthSeconds);
            float[] samples = new float[sampleCount];

            // Multi-tone Indian musical pressure air-horn chord (F4, A4, C5, D5)
            float[] freqs = new float[] { 349.23f, 440.00f, 523.25f, 587.33f };

            for (int i = 0; i < sampleCount; i++)
            {
                float t = (float)i / sampleRate;
                float sum = 0f;

                for (int f = 0; f < freqs.Length; f++)
                {
                    // Harmonic overtones for rich brassy pressure horn sound
                    sum += Mathf.Sin(2f * Mathf.PI * freqs[f] * t) * 0.30f;
                    sum += Mathf.Sin(2f * Mathf.PI * freqs[f] * 2f * t) * 0.15f;
                }

                float envelope = Mathf.Clamp01(t * 20f) * Mathf.Clamp01((lengthSeconds - t) * 10f);
                samples[i] = Mathf.Clamp(sum * envelope * 0.65f, -1.0f, 1.0f);
            }

            AudioClip clip = AudioClip.Create("Synthesized_MelodicAirHorn", sampleCount, 1, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }
    }
}
