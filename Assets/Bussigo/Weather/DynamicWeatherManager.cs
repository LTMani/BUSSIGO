using System;
using UnityEngine;
using Bussigo.Core;

namespace Bussigo.Weather
{
    /// <summary>
    /// Master dynamic weather orchestrator managing rain intensity, road wetness accumulation, puddles, tyre spray, and storm lightning.
    /// </summary>
    public class DynamicWeatherManager : MonoBehaviour, IService
    {
        public WeatherCondition currentCondition = WeatherCondition.Clear;
        public WeatherProfile activeProfile;

        [Header("Road Wetness & Dynamics")]
        [Range(0f, 1f)] public float roadWetness = 0f;
        public float wetAccumulationRate = 0.05f; // per second in heavy rain
        public float dryDryingRate = 0.02f; // per second in clear weather
        public float roadFrictionCoefficient = 1.0f;

        [Header("Puddle & Spray Telemetry")]
        [Range(0f, 1f)] public float puddleCoverage = 0f;
        public float currentTyreSprayIntensity = 0f;

        [Header("Storm Lightning")]
        public bool isLightningActive = false;
        private float lightningTimer = 0f;
        private float nextLightningInterval = 12f;

        public void Initialize()
        {
            activeProfile = WeatherProfile.Create(currentCondition);
            ServiceLocator.Register<DynamicWeatherManager>(this);
            Debug.Log("[BUSSIGO] DynamicWeatherManager initialized.");
        }

        public void Shutdown()
        {
            // Clean shutdown
        }

        public void SetWeather(WeatherCondition condition)
        {
            currentCondition = condition;
            activeProfile = WeatherProfile.Create(condition);
        }

        private void Update()
        {
            if (activeProfile == null) activeProfile = WeatherProfile.Create(currentCondition);

            float dt = Time.deltaTime;
            if (dt <= 0f || dt > 0.1f) dt = 0.02f;

            // 1. Dynamic Road Wetness Solver
            if (activeProfile.rainRate > 0.05f)
            {
                roadWetness = Mathf.Clamp01(roadWetness + wetAccumulationRate * activeProfile.rainRate * dt);
            }
            else
            {
                roadWetness = Mathf.Clamp01(roadWetness - dryDryingRate * dt);
            }

            // 2. Puddle Coverage (Appears in lower road depressions when wetness > 0.4)
            puddleCoverage = Mathf.Clamp01((roadWetness - 0.4f) / 0.6f);

            // 3. Dynamic Road Friction
            roadFrictionCoefficient = Mathf.Lerp(1.0f, activeProfile.roadFrictionMultiplier, roadWetness);

            // 4. Storm Lightning Solver (Throttled & Non-repetitive)
            if (activeProfile.thunderProbability > 0.1f)
            {
                lightningTimer += dt;
                if (lightningTimer >= nextLightningInterval)
                {
                    lightningTimer = 0f;
                    nextLightningInterval = UnityEngine.Random.Range(8f, 22f);
                    TriggerLightningFlash();
                }
            }
            else
            {
                isLightningActive = false;
            }
        }

        public float CalculateTyreSpray(float vehicleSpeedKmh)
        {
            if (vehicleSpeedKmh < 10f || roadWetness < 0.15f) return 0f;
            float speedFactor = Mathf.Clamp01(vehicleSpeedKmh / 90.0f);
            currentTyreSprayIntensity = speedFactor * roadWetness * (0.6f + 0.4f * activeProfile.rainRate);
            return currentTyreSprayIntensity;
        }

        private void TriggerLightningFlash()
        {
            isLightningActive = true;
            // Short 80ms flash
            Invoke(nameof(EndLightningFlash), 0.08f);
        }

        private void EndLightningFlash()
        {
            isLightningActive = false;
        }
    }
}
