using System;
using UnityEngine;
using Bussigo.Vehicle;

namespace Bussigo.Audio
{
    /// <summary>
    /// Speed-dependent tire rolling road noise and suspension vibration controller.
    /// </summary>
    public class TireRoadNoiseController : MonoBehaviour
    {
        public BusChassisController chassisController;
        public BusAudioMixerController mixerController;
        public AudioSource tireAudioSource;

        private void Update()
        {
            if (chassisController == null || tireAudioSource == null) return;

            float speedKmh = Mathf.Abs(chassisController.currentSpeedKmh);
            float normSpeed = Mathf.Clamp01(speedKmh / 100.0f);

            if (normSpeed > 0.02f)
            {
                if (!tireAudioSource.isPlaying) tireAudioSource.Play();

                float perspectiveMult = mixerController != null ? mixerController.GetPerspectiveTyreMultiplier() : 1.0f;
                float baseVol = (mixerController != null ? mixerController.busTyresVolume : 0.7f) * perspectiveMult;

                tireAudioSource.volume = normSpeed * baseVol;
                tireAudioSource.pitch = Mathf.Lerp(0.6f, 1.25f, normSpeed);
            }
            else if (tireAudioSource.isPlaying)
            {
                tireAudioSource.Stop();
            }
        }
    }
}
