using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Vehicles
{
    public class VehicleAuxiliaryElectricalModule37
    {
        public string ModuleIdentifier => "ELEC-AUX-MOD-037";
        public float RatedCurrentDrawAmps { get; set; } = 12.00f;
        public float OperatingVoltageVolts { get; set; } = 24.0f;
        public bool IsCircuitEnergized { get; set; } = true;
        public float ThermalDissipationWatts => RatedCurrentDrawAmps * 0.85f;

        public float ComputePowerConsumptionWatts()
        {
            if (!IsCircuitEnergized) return 0.0f;
            return RatedCurrentDrawAmps * OperatingVoltageVolts;
        }

        public bool CheckOverloadCondition(float actualCurrentAmps)
        {
            return actualCurrentAmps > RatedCurrentDrawAmps * 1.35f;
        }
    }
}
