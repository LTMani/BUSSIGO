using System;
using UnityEngine;
using Bussigo.Vehicle;

namespace Bussigo.Audio
{
    /// <summary>
    /// Multi-layer sample-based diesel engine audio crossfader responding dynamically to RPM and engine load.
    /// </summary>
    public class MultiLayerEngineAudio : MonoBehaviour
    {
        public BusChassisController chassisController;
        public BusAudioMixerController mixerController;

        [Header("Audio Sources")]
        public AudioSource idleSource;
        public AudioSource lowRpmSource;
        public AudioSource midRpmSource;
        public AudioSource highRpmSource;
        public AudioSource turboSource;

        [Header("RPM Band Thresholds")]
        public float idleRpm = 650f;
        public float lowRpm = 1100f;
        public float midRpm = 1600f;
        public float highRpm = 2200f;

        private void Update()
        {
            if (chassisController == null) return;

            float currentRpm = chassisController.currentEngineRpm;
            float throttle = Input.GetAxis("Vertical") > 0 ? Input.GetAxis("Vertical") : 0f;
            float perspectiveMult = mixerController != null ? mixerController.GetPerspectiveEngineMultiplier() : 1.0f;
            float baseVol = (mixerController != null ? mixerController.busEngineVolume : 0.8f) * perspectiveMult;

            // 1. Idle Layer (650 RPM)
            if (idleSource != null)
            {
                float weight = Mathf.Clamp01(1.0f - Mathf.Abs(currentRpm - idleRpm) / 450f);
                idleSource.volume = weight * baseVol * (0.8f + 0.2f * throttle);
                idleSource.pitch = Mathf.Clamp(currentRpm / idleRpm, 0.85f, 1.25f);
            }

            // 2. Low RPM Layer (1100 RPM)
            if (lowRpmSource != null)
            {
                float weight = Mathf.Clamp01(1.0f - Mathf.Abs(currentRpm - lowRpm) / 500f);
                lowRpmSource.volume = weight * baseVol * (0.7f + 0.3f * throttle);
                lowRpmSource.pitch = Mathf.Clamp(currentRpm / lowRpm, 0.85f, 1.25f);
            }

            // 3. Mid RPM Layer (1600 RPM)
            if (midRpmSource != null)
            {
                float weight = Mathf.Clamp01(1.0f - Mathf.Abs(currentRpm - midRpm) / 500f);
                midRpmSource.volume = weight * baseVol * (0.6f + 0.4f * throttle);
                midRpmSource.pitch = Mathf.Clamp(currentRpm / midRpm, 0.85f, 1.25f);
            }

            // 4. High RPM Layer (2200 RPM)
            if (highRpmSource != null)
            {
                float weight = Mathf.Clamp01(1.0f - Mathf.Abs(currentRpm - highRpm) / 600f);
                highRpmSource.volume = weight * baseVol * (0.5f + 0.5f * throttle);
                highRpmSource.pitch = Mathf.Clamp(currentRpm / highRpm, 0.85f, 1.25f);
            }

            // 5. Turbo Spool Layer
            if (turboSource != null)
            {
                float turboLoad = throttle * Mathf.Clamp01((currentRpm - 1000f) / 1200f);
                turboSource.volume = turboLoad * (mixerController != null ? mixerController.busPowertrainVolume : 0.7f) * perspectiveMult;
                turboSource.pitch = Mathf.Lerp(0.8f, 1.4f, turboLoad);
            }
        }
    }
}
