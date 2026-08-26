using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.VehiclePhysics
{
    public class GearboxSynchronizerMeshSolver26
    {
        public string GearboxCode => "GBX-HEAVY-SYNCHRO-026";
        public float ConeFrictionCoefficient { get; set; } = 0.115f;
        public float ShiftForkForceNewtons { get; set; } = 500.0f;
        public float SynchronizerConeRadiusMeters { get; set; } = 0.065f;
        public float ConeAngleDegrees { get; set; } = 7.5f;

        public float CalculateSynchronizationTimeSec(float inputShaftInertiaKgM2, float speedDifferenceRadSec)
        {
            float coneAngleRad = ConeAngleDegrees * CoreMath.DegToRad;
            // Synchronizer Torque: T_s = (F_axial * mu * r_m) / sin(alpha)
            float synchroTorqueNm = (ShiftForkForceNewtons * ConeFrictionCoefficient * SynchronizerConeRadiusMeters) / MathF.Sin(coneAngleRad);

            if (synchroTorqueNm <= 0.1f) return 0.5f;

            float syncTimeSec = (inputShaftInertiaKgM2 * speedDifferenceRadSec) / synchroTorqueNm;
            return CoreMath.Clamp(syncTimeSec, 0.05f, 0.85f);
        }
    }
}
