using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.VehiclePhysics
{
    public class PacejkaTyreModel
    {
        // Pacejka 94 Magic Formula coefficients: y = D * sin(C * atan(B*x - E*(B*x - atan(B*x))))
        public float StiffnessFactorB { get; set; } = 10.0f;
        public float ShapeFactorC { get; set; } = 1.65f;
        public float PeakFactorD { get; set; } = 1.0f; // Multiplied by normal load Fn * friction coeff
        public float CurvatureFactorE { get; set; } = 0.95f;

        public float BaseFrictionCoefficient { get; set; } = 0.95f; // Dry asphalt
        public float TyreRadiusMeters { get; set; } = 0.525f; // 295/80 R22.5 heavy commercial tyre

        public float EvaluateMagicFormula(float slip, float normalLoadNewton, float surfaceFrictionMultiplier)
        {
            if (normalLoadNewton <= 1.0f) return 0.0f;

            float mu = BaseFrictionCoefficient * surfaceFrictionMultiplier;
            float d = normalLoadNewton * mu * PeakFactorD;
            float b = StiffnessFactorB;
            float c = ShapeFactorC;
            float e = CurvatureFactorE;

            float bx = b * slip;
            float force = d * MathF.Sin(c * MathF.Atan(bx - e * (bx - MathF.Atan(bx))));
            return force;
        }

        public float CalculateLongitudinalSlipRatio(float wheelAngularVelocityRadSec, float longitudinalSpeedMps)
        {
            float wheelLinearSpeed = wheelAngularVelocityRadSec * TyreRadiusMeters;
            float refSpeed = MathF.Max(MathF.Abs(longitudinalSpeedMps), 0.5f);
            float slipRatio = (wheelLinearSpeed - longitudinalSpeedMps) / refSpeed;
            return CoreMath.Clamp(slipRatio, -1.0f, 1.0f);
        }

        public float CalculateLateralSlipAngle(float lateralVelocityMps, float longitudinalSpeedMps)
        {
            float refSpeed = MathF.Max(MathF.Abs(longitudinalSpeedMps), 0.5f);
            float slipAngleRad = MathF.Atan2(lateralVelocityMps, refSpeed);
            return slipAngleRad;
        }

        public (float longitudinalForce, float lateralForce) CalculateCombinedForces(
            float slipRatio, float slipAngleRad, float normalLoadNewton, float surfaceFrictionMultiplier)
        {
            float longForcePure = EvaluateMagicFormula(slipRatio, normalLoadNewton, surfaceFrictionMultiplier);
            float latForcePure = -EvaluateMagicFormula(slipAngleRad, normalLoadNewton, surfaceFrictionMultiplier);

            // Friction ellipse interaction
            float combinedSlip = MathF.Sqrt(slipRatio * slipRatio + slipAngleRad * slipAngleRad);
            if (combinedSlip < CoreMath.Epsilon)
            {
                return (0.0f, 0.0f);
            }

            float fx = longForcePure * MathF.Abs(slipRatio / combinedSlip);
            float fy = latForcePure * MathF.Abs(slipAngleRad / combinedSlip);

            return (fx, fy);
        }
    }
}
