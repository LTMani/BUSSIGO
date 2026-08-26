using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Vehicles
{
    public class CabinAirConditioningThermalModel01
    {
        public string HVACSystemId => "HVAC-SYS-DENSO-001";
        public float CoolingCapacityKilowatts { get; set; } = 32.0f; // Commercial bus AC unit 28kW to 48kW
        public float TargetCabinTemperatureCelsius { get; set; } = 22.5f;
        public float CurrentCabinTemperatureCelsius { get; private set; } = 36.0f;
        public float BlowerAirflowCfm { get; set; } = 2050.0f;
        public bool CompressorClutchEngaged { get; private set; } = true;
        public float CompressorPowerDrawEngineHp => (CoolingCapacityKilowatts * 0.42f);

        public void UpdateThermalCycle(float ambientTempCelsius, int passengerCount, float solarRadiationWattsM2, float deltaTime)
        {
            // Thermal loads:
            // 1. Solar transmission through windows (approx 18 m^2 bus glass)
            float solarHeatLoadKw = (solarRadiationWattsM2 * 18.0f * 0.65f) / 1000.0f;

            // 2. Passenger metabolic heat (approx 120W sensible + latent per passenger)
            float passengerHeatLoadKw = (passengerCount * 120.0f) / 1000.0f;

            // 3. Conduction through body panels (U * A * deltaT)
            float deltaTAmbient = MathF.Max(0.0f, ambientTempCelsius - CurrentCabinTemperatureCelsius);
            float conductionHeatLoadKw = 1.2f * 65.0f * deltaTAmbient / 1000.0f;

            float totalHeatGainKw = solarHeatLoadKw + passengerHeatLoadKw + conductionHeatLoadKw;

            // AC cooling capacity modulation
            CompressorClutchEngaged = CurrentCabinTemperatureCelsius > TargetCabinTemperatureCelsius;
            float netCoolingKw = CompressorClutchEngaged ? CoolingCapacityKilowatts : 0.0f;

            // Cabin thermal mass (approx 45 m^3 air + interior furnishings ~ 75 kJ/K)
            float cabinThermalMassKjPerK = 75.0f;
            float netEnergyKw = totalHeatGainKw - netCoolingKw;
            float tempDelta = (netEnergyKw / cabinThermalMassKjPerK) * deltaTime;

            CurrentCabinTemperatureCelsius = CoreMath.Clamp(CurrentCabinTemperatureCelsius + tempDelta, 18.0f, ambientTempCelsius);
        }
    }
}
