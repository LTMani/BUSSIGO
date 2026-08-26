using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Vehicles
{
    public class EngineFuelEfficiencyBSFCMap08
    {
        public float DisplacementLiters => 9.10f;
        public float OptimalBsfcratingGramsPerKwh => 196.5f;

        public float LookupBSFC(float engineRpm, float engineBrakeTorqueNm, float maxTorqueNm)
        {
            float loadRatio = CoreMath.Clamp01(engineBrakeTorqueNm / MathF.Max(1.0f, maxTorqueNm));
            float rpmRatio = CoreMath.Clamp01((engineRpm - 600f) / 1800f);

            float rpmPenalty = MathF.Abs(engineRpm - 1400f) * 0.025f;
            float loadPenalty = MathF.Pow(1.0f - loadRatio, 1.8f) * 65f;

            float effectiveBsfc = OptimalBsfcratingGramsPerKwh + rpmPenalty + loadPenalty;
            return effectiveBsfc;
        }

        public float CalculateInstantaneousDieselFlowRateLph(float engineRpm, float engineBrakeTorqueNm, float maxTorqueNm)
        {
            float bsfc = LookupBSFC(engineRpm, engineBrakeTorqueNm, maxTorqueNm);
            float powerKw = (engineBrakeTorqueNm * engineRpm * 2.0f * MathF.PI) / 60000.0f;
            float gramsPerHour = bsfc * MathF.Max(0.0f, powerKw);
            
            float litersPerHour = gramsPerHour / 835.0f;
            return MathF.Max(1.8f, litersPerHour);
        }
    }
}
