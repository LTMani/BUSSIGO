using System;
using Bussigo.Game.Core;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.VehiclePhysics
{
    public class ChassisRigidBody
    {
        public Vector3D Position { get; set; } = Vector3D.Zero;
        public Vector3D VelocityMps { get; set; } = Vector3D.Zero;
        public float YawAngleDegrees { get; set; } = 0.0f;
        public float YawRateRadSec { get; set; } = 0.0f;
        public float PitchAngleDegrees { get; private set; } = 0.0f;
        public float RollAngleDegrees { get; private set; } = 0.0f;

        public float SpeedKmh => VelocityMps.Length * CoreMath.MpsToKmh;
        public float ForwardSpeedMps => VelocityMps.Z;
        public float LateralSpeedMps => VelocityMps.X;

        private readonly VehicleChassisSpec _spec;

        public ChassisRigidBody(VehicleChassisSpec spec)
        {
            _spec = spec ?? new VehicleChassisSpec();
        }

        public void IntegratePhysics(float longitudinalDriveForceN, float totalBrakingForceN, float frontLateralForceN, float rearLateralForceN, float steeringAngleRad, float surfaceSlopeAngleRad, float deltaTime)
        {
            float totalMass = _spec.KerbMassKg + 3500.0f; // Include passenger payload

            // Aerodynamic drag: Fd = 0.5 * rho * Cd * A * v^2
            float airDensity = 1.205f; // kg/m^3
            float vForward = ForwardSpeedMps;
            float dragForceN = 0.5f * airDensity * _spec.DragCoefficient * _spec.FrontalAreaM2 * vForward * MathF.Abs(vForward);

            // Rolling resistance: Fr = Crr * m * g
            float rollResistCoeff = 0.012f;
            float rollingResistanceForceN = rollResistCoeff * totalMass * CoreMath.Gravity * MathF.Sign(vForward);

            // Slope gravity component
            float slopeGravityForceN = totalMass * CoreMath.Gravity * MathF.Sin(surfaceSlopeAngleRad);

            // Net longitudinal force
            float netLongForceN = longitudinalDriveForceN - totalBrakingForceN - dragForceN - rollingResistanceForceN - slopeGravityForceN;
            float longAccelMps2 = netLongForceN / totalMass;

            // Lateral dynamics and Yaw moment
            float frontAxleDist = _spec.WheelbaseMeters * (1.0f - _spec.FrontAxleWeightRatio);
            float rearAxleDist = _spec.WheelbaseMeters * _spec.FrontAxleWeightRatio;
            float yawInertia = (1.0f / 12.0f) * totalMass * (_spec.LengthMeters * _spec.LengthMeters + _spec.WidthMeters * _spec.WidthMeters);

            float totalLateralForceN = frontLateralForceN * MathF.Cos(steeringAngleRad) + rearLateralForceN;
            float latAccelMps2 = totalLateralForceN / totalMass;

            float yawTorqueNm = (frontLateralForceN * MathF.Cos(steeringAngleRad) * frontAxleDist) - (rearLateralForceN * rearAxleDist);
            float yawAngularAccelRadSec2 = yawTorqueNm / yawInertia;

            // Integration
            YawRateRadSec += yawAngularAccelRadSec2 * deltaTime;
            YawRateRadSec *= 0.96f; // Yaw damping
            YawAngleDegrees += (YawRateRadSec * CoreMath.RadToDeg) * deltaTime;
            YawAngleDegrees = CoreMath.NormalizeAngleDegrees(YawAngleDegrees);

            float newForwardSpeed = ForwardSpeedMps + longAccelMps2 * deltaTime;
            if (MathF.Abs(newForwardSpeed) < 0.05f && MathF.Abs(longitudinalDriveForceN) < 50.0f)
            {
                newForwardSpeed = 0.0f;
            }

            float newLateralSpeed = LateralSpeedMps + latAccelMps2 * deltaTime;
            VelocityMps = new Vector3D(newLateralSpeed, 0.0f, newForwardSpeed);

            // Dynamic pitch and roll angles based on accelerations
            PitchAngleDegrees = CoreMath.Clamp(-longAccelMps2 * 0.45f, -6.0f, 6.0f);
            RollAngleDegrees = CoreMath.Clamp(latAccelMps2 * 0.65f, -8.0f, 8.0f);

            // World position update
            float yawRad = YawAngleDegrees * CoreMath.DegToRad;
            float cosY = MathF.Cos(yawRad);
            float sinY = MathF.Sin(yawRad);

            float worldVx = VelocityMps.X * cosY + VelocityMps.Z * sinY;
            float worldVz = -VelocityMps.X * sinY + VelocityMps.Z * cosY;

            Position = new Vector3D(
                Position.X + worldVx * deltaTime,
                Position.Y,
                Position.Z + worldVz * deltaTime
            );
        }
    }
}
