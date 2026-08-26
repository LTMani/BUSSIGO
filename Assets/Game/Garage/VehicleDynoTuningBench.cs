using System;
using System.Collections.Generic;
using Bussigo.Game.Core;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.Garage
{
    public struct DynoDataPoint
    {
        public float EngineRpm;
        public float BrakeTorqueNm;
        public float Horsepower;
        public float FuelFlowGramsPerKwh;
    }

    public class VehicleDynoTuningBench
    {
        public List<DynoDataPoint> RunDynoSweep(VehicleChassisSpec spec)
        {
            var results = new List<DynoDataPoint>();
            float rpmStep = 100.0f;

            for (float rpm = spec.IdleRpm; rpm <= spec.MaxEngineRpm; rpm += rpmStep)
            {
                float baseTorque;
                if (rpm < spec.MaxTorqueRpmMin)
                {
                    baseTorque = CoreMath.Lerp(spec.MaxTorqueNm * 0.55f, spec.MaxTorqueNm, (rpm - spec.IdleRpm) / (spec.MaxTorqueRpmMin - spec.IdleRpm));
                }
                else if (rpm <= spec.MaxTorqueRpmMax)
                {
                    baseTorque = spec.MaxTorqueNm;
                }
                else
                {
                    baseTorque = CoreMath.Lerp(spec.MaxTorqueNm, spec.MaxTorqueNm * 0.60f, (rpm - spec.MaxTorqueRpmMax) / (spec.MaxEngineRpm - spec.MaxTorqueRpmMax));
                }

                // Power (kW) = (Torque (Nm) * RPM * 2*pi) / 60000
                float powerKw = (baseTorque * rpm * 2.0f * MathF.PI) / 60000.0f;
                float hp = powerKw * 1.34102f;

                // BSFC estimate (g/kWh)
                float bsfc = 195.0f + MathF.Abs(rpm - 1400.0f) * 0.045f;

                results.Add(new DynoDataPoint
                {
                    EngineRpm = rpm,
                    BrakeTorqueNm = baseTorque,
                    Horsepower = hp,
                    FuelFlowGramsPerKwh = bsfc
                });
            }

            return results;
        }
    }
}
