using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.VehiclePhysics
{
    public class AckermannSteering
    {
        public float WheelbaseMeters { get; set; } = 6.2f;
        public float TrackWidthMeters { get; set; } = 2.1f;
        public float MaxInsideWheelAngleDeg { get; set; } = 48.0f;
        public float SpeedSensitivityKmh { get; set; } = 80.0f;

        public (float leftAngleRad, float rightAngleRad) CalculateWheelAngles(float steeringInput01, float currentSpeedKmh)
        {
            steeringInput01 = CoreMath.Clamp(steeringInput01, -1.0f, 1.0f);

            // Speed-sensitive steering reduction for high-speed highway stability
            float speedFactor = 1.0f / (1.0f + MathF.Max(0.0f, currentSpeedKmh) / SpeedSensitivityKmh);
            float targetAngleDeg = steeringInput01 * MaxInsideWheelAngleDeg * speedFactor;

            if (MathF.Abs(targetAngleDeg) < 0.1f)
            {
                return (0.0f, 0.0f);
            }

            float angleRad = targetAngleDeg * CoreMath.DegToRad;
            float turningRadius = WheelbaseMeters / MathF.Tan(MathF.Abs(angleRad));

            float innerAngleRad = MathF.Atan(WheelbaseMeters / (turningRadius - TrackWidthMeters * 0.5f));
            float outerAngleRad = MathF.Atan(WheelbaseMeters / (turningRadius + TrackWidthMeters * 0.5f));

            if (steeringInput01 > 0.0f) // Turning Right
            {
                return (outerAngleRad, innerAngleRad);
            }
            else // Turning Left
            {
                return (-innerAngleRad, -outerAngleRad);
            }
        }
    }
}
