using System;
using UnityEngine;

namespace Bussigo.Physics
{
    [Serializable]
    public class RetarderBrakeSystem
    {
        [Header("Hydrodynamic Retarder Stages")]
        public int currentStage = 0; // 0 = Off, 1 = 25%, 2 = 50%, 3 = 75%, 4 = 100%
        public float[] stageBrakingTorqueNm = new float[] { 0f, 600f, 1300f, 2100f, 3000f };

        public void CycleStage()
        {
            currentStage = (currentStage + 1) % stageBrakingTorqueNm.Length;
        }

        public void SetStage(int stage)
        {
            currentStage = Mathf.Clamp(stage, 0, stageBrakingTorqueNm.Length - 1);
        }

        public float CalculateRetarderBrakingForce(float vehicleSpeedKmh)
        {
            if (currentStage == 0 || Mathf.Abs(vehicleSpeedKmh) < 3.0f)
            {
                return 0f;
            }

            // Retarder braking efficiency is proportional to driveline rotational speed
            float speedFactor = Mathf.Clamp01(Mathf.Abs(vehicleSpeedKmh) / 60.0f);
            float baseTorque = stageBrakingTorqueNm[currentStage];

            return baseTorque * (0.4f + 0.6f * speedFactor);
        }
    }
}
