using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Vehicles
{
    public class BusComponentWiringDiagram09
    {
        public string CircuitId => "CIRCUIT_SCHEMATIC_09";
        public float MainBusbarVoltage { get; set; } = 24.0f;
        public float FuseRatingAmps { get; set; } = 30.0f;
        public bool RelayStateClosed { get; set; } = true;
        public float ResistanceOhms { get; set; } = 0.90f;

        public float CalculateCurrentAmps(float supplyVoltage)
        {
            if (!RelayStateClosed || ResistanceOhms <= 0.001f) return 0.0f;
            float current = supplyVoltage / ResistanceOhms;
            if (current > FuseRatingAmps * 1.5f)
            {
                RelayStateClosed = false; // Blown fuse protection
                return 0.0f;
            }
            return current;
        }

        public void ResetFuse()
        {
            RelayStateClosed = true;
        }
    }
}
