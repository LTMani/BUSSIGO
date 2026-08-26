using System;
using UnityEngine;
using Bussigo.Vehicle;

namespace Bussigo.Audio
{
    public enum AudioPerspective
    {
        Exterior = 0,
        DriverCockpit = 1,
        PassengerCabin = 2
    }

    /// <summary>
    /// Master bus audio controller managing 8 mixer sub-groups and spatial cabin acoustics.
    /// </summary>
    public class BusAudioMixerController : MonoBehaviour
    {
        [Header("Master & Sub-group Volumes (0.0 to 1.0)")]
        public float masterVolume = 0.90f;
        public float busEngineVolume = 0.85f;
        public float busPowertrainVolume = 0.70f;
        public float busBrakesVolume = 0.80f;
        public float busAirVolume = 0.75f;
        public float busCabinVolume = 0.65f;
        public float busHornVolume = 0.95f;
        public float busTyresVolume = 0.70f;
        public float busEnvironmentVolume = 0.60f;

        [Header("Active Perspective")]
        public AudioPerspective currentPerspective = AudioPerspective.Exterior;

        [Header("Low-Pass Filter Settings")]
        public float exteriorCutoffHz = 22000f;
        public float cockpitCutoffHz = 1600f;
        public float cabinCutoffHz = 900f;

        public float GetPerspectiveEngineMultiplier()
        {
            switch (currentPerspective)
            {
                case AudioPerspective.Exterior: return 1.0f;
                case AudioPerspective.DriverCockpit: return 0.55f;
                case AudioPerspective.PassengerCabin: return 0.35f;
                default: return 1.0f;
            }
        }

        public float GetPerspectiveTyreMultiplier()
        {
            switch (currentPerspective)
            {
                case AudioPerspective.Exterior: return 1.0f;
                case AudioPerspective.DriverCockpit: return 0.60f;
                case AudioPerspective.PassengerCabin: return 0.85f;
                default: return 1.0f;
            }
        }

        public void UpdatePerspectiveFromCameraMode(BusCameraMode mode)
        {
            if (mode == BusCameraMode.DriverCockpit)
            {
                currentPerspective = AudioPerspective.DriverCockpit;
            }
            else if (mode == BusCameraMode.PassengerCabin)
            {
                currentPerspective = AudioPerspective.PassengerCabin;
            }
            else
            {
                currentPerspective = AudioPerspective.Exterior;
            }
        }
    }
}
