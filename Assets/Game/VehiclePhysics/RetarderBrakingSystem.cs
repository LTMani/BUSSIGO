using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.VehiclePhysics
{
    public class RetarderBrakingSystem
    {
        public float MaxRetarderTorqueNm { get; set; } = 2400.0f; // Heavy Telma / Voith hydrodynamic retarder
        public int RetarderStage { get; private set; } = 0; // 0 = Off, 1 = 25%, 2 = 50%, 3 = 75%, 4 = 100%
        public float RetarderOilTempCelsius { get; private set; } = 45.0f;

        public void SetStage(int stage)
        {
            RetarderStage = CoreMath.Clamp(stage, 0, 4);
        }

        public float CalculateRetarderTorque(float driveshaftRpm, float deltaTime)
        {
            if (RetarderStage == 0 || driveshaftRpm < 150.0f)
            {
                RetarderOilTempCelsius = CoreMath.MoveTowards(RetarderOilTempCelsius, 45.0f, deltaTime * 2.0f);
                return 0.0f;
            }

            float stageRatio = RetarderStage / 4.0f;
            float speedRatio = CoreMath.Clamp01(driveshaftRpm / 1500.0f);

            // Hydrodynamic retarder torque scales with square of driveshaft speed up to saturation
            float torque = MaxRetarderTorqueNm * stageRatio * (0.3f + 0.7f * speedRatio);

            // Thermal dissipation
            RetarderOilTempCelsius += (torque / MaxRetarderTorqueNm) * deltaTime * 12.0f;

            // Thermal derating above 135°C
            if (RetarderOilTempCelsius > 135.0f)
            {
                float derate = CoreMath.Clamp01(1.0f - (RetarderOilTempCelsius - 135.0f) / 30.0f);
                torque *= derate;
            }

            return torque;
        }
    }
}
