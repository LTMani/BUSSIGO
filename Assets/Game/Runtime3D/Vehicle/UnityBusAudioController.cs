using System;
using UnityEngine;

namespace Bussigo.Game.Runtime3D.Vehicle
{
    public class UnityBusAudioController : MonoBehaviour
    {
        public UnityBusController3D busController;
        
        [Header("Audio Sources")]
        public AudioSource engineAudioSource;
        public AudioSource turboAudioSource;
        public AudioSource airBrakePurgeAudioSource;
        public AudioSource airHornAudioSource;
        public AudioSource retarderAudioSource;

        [Header("Engine Audio Tuning")]
        public float idlePitch = 0.65f;
        public float maxPitch = 2.1f;
        public float baseVolume = 0.4f;

        private float previousBrakeInput = 0f;

        private void Update()
        {
            if (busController == null) return;

            UpdateEngineSound();
            UpdateAirBrakeHiss();
            UpdateAirHorn();
            UpdateRetarderSound();
        }

        private void UpdateEngineSound()
        {
            if (engineAudioSource != null)
            {
                float normRpm = Mathf.InverseLerp(busController.idleRpm, busController.maxRpm, busController.currentEngineRpm);
                engineAudioSource.pitch = Mathf.Lerp(idlePitch, maxPitch, normRpm);
                engineAudioSource.volume = Mathf.Lerp(baseVolume, 1.0f, busController.currentThrottleInput01 * 0.5f + normRpm * 0.5f);
                if (!engineAudioSource.isPlaying) engineAudioSource.Play();
            }

            if (turboAudioSource != null)
            {
                float turboLoad = busController.currentThrottleInput01 * Mathf.Clamp01(busController.currentEngineRpm / 1600f);
                turboAudioSource.volume = turboLoad * 0.7f;
                turboAudioSource.pitch = 0.8f + (turboLoad * 0.8f);
                if (turboLoad > 0.05f && !turboAudioSource.isPlaying) turboAudioSource.Play();
            }
        }

        private void UpdateAirBrakeHiss()
        {
            // Trigger air purge hiss when releasing brake from heavy pressure
            if (previousBrakeInput > 0.4f && busController.currentBrakeInput01 < 0.1f)
            {
                if (airBrakePurgeAudioSource != null)
                {
                    airBrakePurgeAudioSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
                    airBrakePurgeAudioSource.Play();
                }
            }
            previousBrakeInput = busController.currentBrakeInput01;
        }

        private void UpdateAirHorn()
        {
            if (airHornAudioSource != null)
            {
                if (busController.isHornSounding)
                {
                    if (!airHornAudioSource.isPlaying) airHornAudioSource.Play();
                }
                else
                {
                    if (airHornAudioSource.isPlaying) airHornAudioSource.Stop();
                }
            }
        }

        private void UpdateRetarderSound()
        {
            if (retarderAudioSource != null)
            {
                if (busController.currentRetarderLevel > 0 && Mathf.Abs(busController.currentSpeedKmh) > 10f)
                {
                    float speedRatio = Mathf.Clamp01(Mathf.Abs(busController.currentSpeedKmh) / 90f);
                    retarderAudioSource.volume = (busController.currentRetarderLevel * 0.2f) * speedRatio;
                    retarderAudioSource.pitch = 0.5f + (speedRatio * 0.9f);
                    if (!retarderAudioSource.isPlaying) retarderAudioSource.Play();
                }
                else
                {
                    if (retarderAudioSource.isPlaying) retarderAudioSource.Stop();
                }
            }
        }
    }
}
