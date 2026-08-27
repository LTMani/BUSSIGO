using System;
using UnityEngine;
using Bussigo.Vehicle;

namespace Bussigo.Audio
{
    /// <summary>
    /// Manages pneumatic air purge, compressor charge, retarder, door, and horn audio events.
    /// </summary>
    public class PneumaticAirSoundController : MonoBehaviour
    {
        public BusChassisController chassisController;
        public BusAudioMixerController mixerController;

        [Header("Audio Sources")]
        public AudioSource airPurgeSource;
        public AudioSource compressorSource;
        public AudioSource retarderSource;
        public AudioSource doorSource;
        public AudioSource hornSource;
        public AudioSource gearShiftSource;

        private bool wasBraking = false;
        private bool wasDoorOpen = false;
        private int lastGear = 1;

        private void Update()
        {
            if (chassisController == null) return;

            float brakeInput = chassisController.BrakeInput;

            // 1. Air Brake Purge (Triggers on Brake Release)
            if (wasBraking && brakeInput < 0.05f && airPurgeSource != null)
            {
                airPurgeSource.PlayOneShot(airPurgeSource.clip, mixerController != null ? mixerController.busAirVolume : 0.8f);
            }
            wasBraking = (brakeInput > 0.1f);

            // 2. Air Compressor (Runs only when charging below 8.5 bar)
            if (compressorSource != null)
            {
                bool isCharging = chassisController.primaryAirPressureBar < 8.5f;
                if (isCharging && !compressorSource.isPlaying)
                {
                    compressorSource.volume = (mixerController != null ? mixerController.busAirVolume : 0.7f) * 0.6f;
                    compressorSource.Play();
                }
                else if (!isCharging && compressorSource.isPlaying)
                {
                    compressorSource.Stop();
                }
            }

            // 3. Retarder Sound (Proportional to Retarder Stage & Speed)
            if (retarderSource != null)
            {
                int stage = chassisController.retarderLevel;
                float speed = Mathf.Abs(chassisController.currentSpeedKmh);
                if (stage > 0 && speed > 5.0f)
                {
                    if (!retarderSource.isPlaying) retarderSource.Play();
                    float load = (stage / 4.0f) * Mathf.Clamp01(speed / 75.0f);
                    retarderSource.volume = load * (mixerController != null ? mixerController.busBrakesVolume : 0.8f);
                    retarderSource.pitch = Mathf.Lerp(0.7f, 1.35f, speed / 90.0f);
                }
                else if (retarderSource.isPlaying)
                {
                    retarderSource.Stop();
                }
            }

            // 4. Glider Door Open/Close Audio
            if (chassisController.isDoorOpen != wasDoorOpen)
            {
                wasDoorOpen = chassisController.isDoorOpen;
                if (doorSource != null && doorSource.clip != null)
                {
                    doorSource.PlayOneShot(doorSource.clip, mixerController != null ? mixerController.busCabinVolume : 0.75f);
                }
            }

            // 5. Musical Air Horn
            if (hornSource != null)
            {
                bool hornPressed = Input.GetKey(KeyCode.H);
                if (hornPressed && !hornSource.isPlaying)
                {
                    hornSource.volume = mixerController != null ? mixerController.busHornVolume : 0.95f;
                    hornSource.Play();
                }
                else if (!hornPressed && hornSource.isPlaying)
                {
                    hornSource.Stop();
                }
            }

            // 6. Gear Shift Transient Clunk
            if (chassisController.currentGear != lastGear)
            {
                lastGear = chassisController.currentGear;
                if (gearShiftSource != null && gearShiftSource.clip != null)
                {
                    gearShiftSource.PlayOneShot(gearShiftSource.clip, mixerController != null ? mixerController.busPowertrainVolume : 0.7f);
                }
            }
        }
    }
}
