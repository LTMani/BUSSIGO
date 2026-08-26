using System;
using UnityEngine;
using Bussigo.Core;

namespace Bussigo.Weather
{
    public enum DayTimePhase
    {
        Night = 0,
        Dawn = 1,
        Morning = 2,
        Noon = 3,
        Afternoon = 4,
        Sunset = 5,
        Evening = 6
    }

    /// <summary>
    /// Manages the 24-hour diurnal solar cycle, directional sun rotation, and atmospheric color grading.
    /// </summary>
    public class TimeOfDayService : MonoBehaviour, IService
    {
        [Header("Time Configuration")]
        [Range(0f, 24f)] public float currentHour = 14.5f; // 2:30 PM default
        public float timeScale = 1.0f; // 1 real sec = 1 game sec (can be scaled)
        public bool autoAdvance = true;

        [Header("Lighting Outputs")]
        public Light sunDirectionalLight;
        public DayTimePhase currentPhase = DayTimePhase.Afternoon;
        public float sunIntensity = 1.0f;
        public float ambientIntensity = 1.0f;
        public Color sunLightColor = Color.white;
        public Color ambientSkyColor = Color.gray;

        public void Initialize()
        {
            ServiceLocator.Register<TimeOfDayService>(this);
            Debug.Log("[BUSSIGO] TimeOfDayService initialized.");
        }

        public void Shutdown()
        {
            // Clean shutdown
        }

        private void Update()
        {
            if (autoAdvance)
            {
                currentHour += (Time.deltaTime * timeScale) / 3600.0f;
                if (currentHour >= 24.0f) currentHour -= 24.0f;
            }

            UpdateSolarLighting();
        }

        public void SetTime(float hour)
        {
            currentHour = Mathf.Clamp(hour, 0f, 23.99f);
            UpdateSolarLighting();
        }

        private void UpdateSolarLighting()
        {
            // Solar elevation angle: 0h = -90 deg (Nadir), 6h = 0 deg (Sunrise), 12h = 90 deg (Zenith), 18h = 0 deg (Sunset)
            float sunAngle = (currentHour / 24.0f) * 360.0f - 90.0f;
            Vector3 sunDir = Quaternion.Euler(sunAngle, 45f, 0f) * Vector3.forward;

            if (currentHour >= 5.0f && currentHour < 7.0f)
            {
                currentPhase = DayTimePhase.Dawn;
                sunIntensity = Mathf.Lerp(0.1f, 0.8f, (currentHour - 5.0f) / 2.0f);
                ambientIntensity = 0.5f;
                sunLightColor = new Color(1.0f, 0.65f, 0.4f);
                ambientSkyColor = new Color(0.4f, 0.35f, 0.5f);
            }
            else if (currentHour >= 7.0f && currentHour < 11.0f)
            {
                currentPhase = DayTimePhase.Morning;
                sunIntensity = 1.0f;
                ambientIntensity = 0.85f;
                sunLightColor = new Color(1.0f, 0.95f, 0.85f);
                ambientSkyColor = new Color(0.5f, 0.65f, 0.85f);
            }
            else if (currentHour >= 11.0f && currentHour < 15.0f)
            {
                currentPhase = DayTimePhase.Noon;
                sunIntensity = 1.25f;
                ambientIntensity = 1.0f;
                sunLightColor = Color.white;
                ambientSkyColor = new Color(0.6f, 0.75f, 0.95f);
            }
            else if (currentHour >= 15.0f && currentHour < 17.5f)
            {
                currentPhase = DayTimePhase.Afternoon;
                sunIntensity = 1.05f;
                ambientIntensity = 0.9f;
                sunLightColor = new Color(1.0f, 0.92f, 0.8f);
                ambientSkyColor = new Color(0.55f, 0.7f, 0.9f);
            }
            else if (currentHour >= 17.5f && currentHour < 19.5f)
            {
                currentPhase = DayTimePhase.Sunset;
                sunIntensity = Mathf.Lerp(0.8f, 0.1f, (currentHour - 17.5f) / 2.0f);
                ambientIntensity = 0.45f;
                sunLightColor = new Color(1.0f, 0.45f, 0.2f);
                ambientSkyColor = new Color(0.5f, 0.3f, 0.4f);
            }
            else if (currentHour >= 19.5f && currentHour < 21.5f)
            {
                currentPhase = DayTimePhase.Evening;
                sunIntensity = 0.05f;
                ambientIntensity = 0.25f;
                sunLightColor = new Color(0.3f, 0.35f, 0.5f);
                ambientSkyColor = new Color(0.15f, 0.18f, 0.28f);
            }
            else
            {
                currentPhase = DayTimePhase.Night;
                sunIntensity = 0.02f;
                ambientIntensity = 0.12f;
                sunLightColor = new Color(0.2f, 0.25f, 0.4f);
                ambientSkyColor = new Color(0.08f, 0.1f, 0.16f);
            }

            if (sunDirectionalLight != null)
            {
                sunDirectionalLight.transform.rotation = Quaternion.LookRotation(sunDir);
                sunDirectionalLight.intensity = sunIntensity;
                sunDirectionalLight.color = sunLightColor;
            }
        }
    }
}
