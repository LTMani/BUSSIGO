using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Vehicles
{
    public class VehicleElectricalSystem
    {
        public float BatteryVoltage { get; private set; } = 24.0f; // 24V commercial bus architecture
        public float BatteryStateOfCharge { get; private set; } = 0.95f; // 0.0 to 1.0 (95%)
        public float BatteryCapacityAh { get; set; } = 180.0f; // 2x 12V 180Ah in series
        public float AlternatorAmperageOutput { get; private set; } = 0.0f;
        public float MaxAlternatorAmps { get; set; } = 150.0f;

        public bool MasterBatterySwitch { get; set; } = true;
        public bool IgnitionKeyOn { get; set; } = false;
        public bool StarterMotorActive { get; private set; } = false;

        // Lighting states
        public bool ParkingLights { get; set; } = false;
        public bool LowBeamHeadlights { get; set; } = false;
        public bool HighBeamHeadlights { get; set; } = false;
        public bool FogLamps { get; set; } = false;
        public bool LeftIndicator { get; set; } = false;
        public bool RightIndicator { get; set; } = false;
        public bool HazardLights { get; set; } = false;
        public bool BrakeLights { get; set; } = false;
        public bool ReverseLights { get; set; } = false;
        public bool CabinPassengerLights { get; set; } = true;
        public bool CabinReadingLights { get; set; } = false;
        public bool DestinationLedBoardPower { get; set; } = true;

        // Auxiliaries
        public bool WindshieldWipersOn { get; set; } = false;
        public int WiperSpeedLevel { get; set; } = 0; // 0: Off, 1: Intermittent, 2: Low, 3: High
        public bool AirConditioningBlowerOn { get; set; } = true;
        public float AirConditioningPowerKw { get; set; } = 4.5f;
        public bool ElectricHornActive { get; set; } = false;

        private float _blinkerTimer = 0.0f;
        public bool BlinkerCycleState { get; private set; } = false;

        public void Update(float deltaTime, float engineRpm, bool engineRunning)
        {
            if (!MasterBatterySwitch)
            {
                BatteryVoltage = 0.0f;
                return;
            }

            // Blinker relay clock (1.5 Hz ~ 90 flashes/min)
            _blinkerTimer += deltaTime;
            if (_blinkerTimer >= 0.35f)
            {
                _blinkerTimer = 0.0f;
                BlinkerCycleState = !BlinkerCycleState;
            }

            float currentDrawAmps = 2.0f; // Standby parasitic drain

            if (IgnitionKeyOn) currentDrawAmps += 5.0f;
            if (ParkingLights) currentDrawAmps += 4.0f;
            if (LowBeamHeadlights) currentDrawAmps += 10.0f;
            if (HighBeamHeadlights) currentDrawAmps += 16.0f;
            if (FogLamps) currentDrawAmps += 8.0f;
            if ((LeftIndicator || RightIndicator || HazardLights) && BlinkerCycleState) currentDrawAmps += 6.0f;
            if (BrakeLights) currentDrawAmps += 5.0f;
            if (ReverseLights) currentDrawAmps += 3.0f;
            if (CabinPassengerLights) currentDrawAmps += 12.0f;
            if (DestinationLedBoardPower) currentDrawAmps += 3.0f;
            if (WindshieldWipersOn) currentDrawAmps += (WiperSpeedLevel * 4.0f);
            if (AirConditioningBlowerOn) currentDrawAmps += 25.0f;
            if (ElectricHornActive) currentDrawAmps += 15.0f;

            if (StarterMotorActive)
            {
                currentDrawAmps += 250.0f; // High inrush cranking current
            }

            if (engineRunning)
            {
                float alternatorSpeedRatio = CoreMath.Clamp01((engineRpm - 500f) / 1200f);
                AlternatorAmperageOutput = MaxAlternatorAmps * alternatorSpeedRatio;
                float netCurrent = AlternatorAmperageOutput - currentDrawAmps;

                // Charging battery
                float netAh = (netCurrent * deltaTime) / 3600.0f;
                BatteryStateOfCharge = CoreMath.Clamp01(BatteryStateOfCharge + (netAh / BatteryCapacityAh));
                BatteryVoltage = CoreMath.Lerp(25.5f, 28.4f, alternatorSpeedRatio);
            }
            else
            {
                AlternatorAmperageOutput = 0.0f;
                // Discharging battery
                float netAh = (currentDrawAmps * deltaTime) / 3600.0f;
                BatteryStateOfCharge = CoreMath.Clamp01(BatteryStateOfCharge - (netAh / BatteryCapacityAh));
                BatteryVoltage = CoreMath.Lerp(21.0f, 25.2f, BatteryStateOfCharge);
            }
        }

        public void CrankStarter(bool activate)
        {
            StarterMotorActive = activate && IgnitionKeyOn && MasterBatterySwitch && (BatteryStateOfCharge > 0.15f);
        }
    }
}
