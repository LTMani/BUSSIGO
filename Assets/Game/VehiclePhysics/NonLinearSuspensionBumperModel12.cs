using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.VehiclePhysics
{
    public class NonLinearSuspensionBumperModel12
    {
        public string SuspensionPartNumber => "SUSP-BUSHING-AIR-012";
        public float JounceBumperEngageDisplacementMeters { get; set; } = 0.12f;
        public float JounceStiffnessProgressiveNewtonPerM2 { get; set; } = 450000.0f;

        public float CalculateProgressiveBumperForce(float compressionMeters)
        {
            if (compressionMeters <= JounceBumperEngageDisplacementMeters)
            {
                return 0.0f;
            }

            float bumpCompression = compressionMeters - JounceBumperEngageDisplacementMeters;
            // Progressive non-linear polyurethane bump stop curve
            float bumperForce = JounceStiffnessProgressiveNewtonPerM2 * bumpCompression * bumpCompression;
            return bumperForce;
        }
    }
}
