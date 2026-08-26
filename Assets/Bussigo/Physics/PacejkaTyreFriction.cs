using System;
using UnityEngine;

namespace Bussigo.Physics
{
    [Serializable]
    public class PacejkaTyreFriction
    {
        [Header("Pacejka '94 Coefficients")]
        public float B_Stiffness = 10.0f;
        public float C_Shape = 1.65f;
        public float D_PeakFriction = 1.05f;
        public float E_Curvature = -0.15f;

        public float CalculateLateralForce(float slipAngleDegrees, float normalLoadNewtons, float surfaceGripMultiplier = 1.0f)
        {
            float alpha = slipAngleDegrees;
            float mu = D_PeakFriction * Mathf.Sin(C_Shape * Mathf.Atan(B_Stiffness * alpha - E_Curvature * (B_Stiffness * alpha - Mathf.Atan(B_Stiffness * alpha))));
            return mu * normalLoadNewtons * surfaceGripMultiplier;
        }

        public float CalculateLongitudinalForce(float slipRatio01, float normalLoadNewtons, float surfaceGripMultiplier = 1.0f)
        {
            float kappa = Mathf.Clamp(slipRatio01, -1.0f, 1.0f);
            float mu = D_PeakFriction * Mathf.Sin(C_Shape * Mathf.Atan(B_Stiffness * kappa - E_Curvature * (B_Stiffness * kappa - Mathf.Atan(B_Stiffness * kappa))));
            return mu * normalLoadNewtons * surfaceGripMultiplier;
        }
    }
}
