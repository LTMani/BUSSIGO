using System;
using UnityEngine;
using Bussigo.Vehicle;

namespace Bussigo.Audio
{
    public class DieselEngineHarmonicDSP : MonoBehaviour
    {
        public BusChassisController busController;
        public AudioSource engineAudioSource;
        public AudioSource hornAudioSource;
        public AudioSource airPurgeAudioSource;

        [Header("Engine Pitch & Volume Range")]
        public float idlePitch = 0.65f;
        public float maxPitch = 1.65f;
        public float minVolume = 0.25f;
        public float maxVolume = 0.85f;

        private void Update()
        {
            if (busController == null) return;

            // Smooth engine pitch scaling without buzzing
            float normRpm = Mathf.Clamp01((busController.currentEngineRpm - 650f) / 1550f);
            if (engineAudioSource != null)
            {
                engineAudioSource.pitch = Mathf.Lerp(idlePitch, maxPitch, normRpm);
                engineAudioSource.volume = Mathf.Lerp(minVolume, maxVolume, normRpm);
            }

            // Air horn trigger
            if (hornAudioSource != null)
            {
                if (busController.isHornSounding && !hornAudioSource.isPlaying)
                {
                    hornAudioSource.Play();
                }
                else if (!busController.isHornSounding && hornAudioSource.isPlaying)
                {
                    hornAudioSource.Stop();
                }
            }
        }

        public void PlayAirBrakePurge()
        {
            if (airPurgeAudioSource != null)
            {
                airPurgeAudioSource.PlayOneShot(airPurgeAudioSource.clip);
            }
        }
    }
}
