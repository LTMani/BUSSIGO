using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.VehiclePhysics
{
    public class PneumaticCircuitChamberModel11
    {
        public float ChamberVolumeLiters { get; set; } = 47.5f;
        public float CurrentPressureBar { get; private set; } = 8.5f;
        public float PortOrificeAreaMm2 { get; set; } = 75.0f;

        public void InflowFromCompressor(float massFlowKgSec, float deltaTime)
        {
            float deltaPressureBar = (massFlowKgSec * deltaTime * 287.05f * 293.15f) / (ChamberVolumeLiters * 1e-3f * 1e5f);
            CurrentPressureBar = MathF.Min(10.5f, CurrentPressureBar + deltaPressureBar);
        }

        public float DischargeAirThroughValve(float downstreamPressureBar, float valveOpenFraction, float deltaTime)
        {
            if (CurrentPressureBar <= downstreamPressureBar || valveOpenFraction <= 0.01f) return 0.0f;

            float deltaP = CurrentPressureBar - downstreamPressureBar;
            float flowRateBarPerSec = (PortOrificeAreaMm2 * 0.015f) * MathF.Sqrt(deltaP) * valveOpenFraction;
            float dischargedBar = flowRateBarPerSec * deltaTime;

            CurrentPressureBar = MathF.Max(downstreamPressureBar, CurrentPressureBar - dischargedBar);
            return dischargedBar;
        }
    }
}
