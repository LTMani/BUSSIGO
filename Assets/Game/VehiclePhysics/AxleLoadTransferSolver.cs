using System;
using Bussigo.Game.Core;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.VehiclePhysics
{
    public class AxleLoadTransferSolver
    {
        public float CenterOfGravityHeightMeters { get; set; } = 1.35f;
        public float StaticFrontAxleWeightFraction { get; set; } = 0.35f;
        public float WheelbaseMeters { get; set; } = 6.20f;
        public float TrackWidthMeters { get; set; } = 2.15f;

        public (float frontAxleLoadN, float rearAxleLoadN) CalculateLongitudinalLoadTransfer(
            float totalMassKg, float longitudinalAccelerationMps2, float roadGradientAngleRad)
        {
            float totalWeightN = totalMassKg * CoreMath.Gravity;
            float staticFrontN = totalWeightN * StaticFrontAxleWeightFraction;
            float staticRearN = totalWeightN * (1.0f - StaticFrontAxleWeightFraction);

            float dynamicTransferN = (totalMassKg * longitudinalAccelerationMps2 * CenterOfGravityHeightMeters) / WheelbaseMeters;
            float slopeTransferN = (totalWeightN * MathF.Sin(roadGradientAngleRad) * CenterOfGravityHeightMeters) / WheelbaseMeters;

            float dynamicFrontN = MathF.Max(0.0f, staticFrontN - dynamicTransferN - slopeTransferN);
            float dynamicRearN = MathF.Max(0.0f, staticRearN + dynamicTransferN + slopeTransferN);

            return (dynamicFrontN, dynamicRearN);
        }

        public (float leftSideLoadN, float rightSideLoadN) CalculateLateralLoadTransfer(
            float totalAxleLoadN, float lateralAccelerationMps2, float axleRollStiffnessFraction)
        {
            float staticSideN = totalAxleLoadN * 0.5f;
            float lateralTransferN = (totalAxleLoadN / CoreMath.Gravity) * lateralAccelerationMps2 * (CenterOfGravityHeightMeters / TrackWidthMeters) * axleRollStiffnessFraction;

            float leftN = MathF.Max(0.0f, staticSideN - lateralTransferN);
            float rightN = MathF.Max(0.0f, staticSideN + lateralTransferN);

            return (leftN, rightN);
        }
    }
}
