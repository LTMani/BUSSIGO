using System;
using UnityEngine;
using Bussigo.Game.Runtime3D.Vehicle;

namespace Bussigo.Game.Runtime3D.Environment
{
    public class MonsoonRainParticleController : MonoBehaviour
    {
        public UnityBusController3D playerBus;
        
        [Header("Weather Simulation")]
        public bool isMonsoonRainActive = false;
        public float rainIntensity01 = 0.0f;
        public float roadFrictionCoefficient = 0.85f; // Normal dry asphalt
        public bool areWipersActive = false;

        private float weatherChangeTimer = 0f;

        private void Update()
        {
            // Toggle Monsoon Rain with Key F8 (or dynamic timer)
            if (Input.GetKeyDown(KeyCode.F8))
            {
                isMonsoonRainActive = !isMonsoonRainActive;
                Debug.Log($"[Weather] Monsoon Rain is now: {(isMonsoonRainActive ? "ACTIVE" : "INACTIVE")}");
            }

            // Toggle Wipers with Key P
            if (Input.GetKeyDown(KeyCode.P))
            {
                areWipersActive = !areWipersActive;
            }

            float targetRain = isMonsoonRainActive ? 1.0f : 0.0f;
            rainIntensity01 = Mathf.MoveTowards(rainIntensity01, targetRain, Time.deltaTime * 0.15f);

            // Friction modulation on wet asphalt
            roadFrictionCoefficient = Mathf.Lerp(0.88f, 0.42f, rainIntensity01);

            // Atmospheric Fog modulation during monsoon
            RenderSettings.fogDensity = Mathf.Lerp(0.0012f, 0.0065f, rainIntensity01);
            RenderSettings.fogColor = Color.Lerp(new Color(0.75f, 0.82f, 0.90f), new Color(0.35f, 0.40f, 0.48f), rainIntensity01);
        }
    }
}
