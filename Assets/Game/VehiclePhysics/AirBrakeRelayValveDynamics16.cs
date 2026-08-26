using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.VehiclePhysics
{
    public class AirBrakeRelayValveDynamics16
    {
        public string ValveSerialNumber => "RELAY-VALVE-WABCO-016";
        public float SupplyPressureBar { get; set; } = 8.5f;
        public float ControlSignalPressureBar { get; set; } = 0.0f;
        public float DeliveryPressureBar { get; private set; } = 0.0f;
        public float CrackPressureBar { get; set; } = 0.35f; // Threshold to begin delivery

        public void UpdateValvePneumatics(float pilotSignalBar, float deltaTime)
        {
            ControlSignalPressureBar = CoreMath.Clamp(pilotSignalBar, 0.0f, SupplyPressureBar);

            if (ControlSignalPressureBar < CrackPressureBar)
            {
                DeliveryPressureBar = CoreMath.MoveTowards(DeliveryPressureBar, 0.0f, deltaTime * 25.0f);
            }
            else
            {
                float targetDelivery = (ControlSignalPressureBar - CrackPressureBar) * 1.05f;
                DeliveryPressureBar = CoreMath.MoveTowards(DeliveryPressureBar, MathF.Min(SupplyPressureBar, targetDelivery), deltaTime * 30.0f);
            }
        }

        public float CalculateBrakeActuatorForceNewtons(float diaphragmAreaMm2)
        {
            // Force (N) = Pressure (Pa) * Area (m^2) = Pressure (bar) * 1e5 * Area (mm^2) * 1e-6 = P_bar * Area_mm2 * 0.1
            return DeliveryPressureBar * diaphragmAreaMm2 * 0.1f;
        }
    }
}
