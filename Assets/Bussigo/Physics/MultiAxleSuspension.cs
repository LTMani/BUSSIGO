using System;
using UnityEngine;

namespace Bussigo.Physics
{
    [Serializable]
    public class MultiAxleSuspension
    {
        [Header("Axle Specs")]
        public float springStiffnessNPerM = 85000f;
        public float damperRateNsPerM = 6500f;
        public float suspensionTravelMeters = 0.22f;

        public float CalculateSuspensionForce(float currentCompressionMeters, float compressionVelocityMps)
        {
            float clampedCompression = Mathf.Clamp(currentCompressionMeters, -suspensionTravelMeters, suspensionTravelMeters);
            float springForce = clampedCompression * springStiffnessNPerM;
            float damperForce = compressionVelocityMps * damperRateNsPerM;
            return Mathf.Max(0f, springForce + damperForce);
        }
    }
}
