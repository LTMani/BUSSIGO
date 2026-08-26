using System;
using System.Collections.Generic;

namespace Bussigo.Game.Vehicles
{
    public enum DiagnosticSeverity
    {
        Information,
        Warning,
        CriticalStopEngine
    }

    public class DiagnosticTroubleCode
    {
        public string Code { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }
        public DiagnosticSeverity Severity { get; set; }
        public bool IsActive { get; set; } = false;
        public DateTime TimestampOccurred { get; set; }
    }

    public class OBD2DiagnosticsRegistry
    {
        public Dictionary<string, DiagnosticTroubleCode> RegisteredCodes { get; } = new Dictionary<string, DiagnosticTroubleCode>();

        public OBD2DiagnosticsRegistry()
        {
            RegisterCode("P0101", "Air Intake", "Mass Airflow Sensor Circuit Range/Performance Fault", DiagnosticSeverity.Warning);
            RegisterCode("P0217", "Cooling", "Engine Coolant Over-Temperature Condition Detected", DiagnosticSeverity.CriticalStopEngine);
            RegisterCode("P0524", "Lubrication", "Engine Oil Pressure Too Low (< 1.2 bar)", DiagnosticSeverity.CriticalStopEngine);
            RegisterCode("C0035", "Brakes/ABS", "Left Front Wheel Speed Sensor Signal Erratic", DiagnosticSeverity.Warning);
            RegisterCode("C1095", "Pneumatics", "Primary Air Pressure Reservoir Loss of Pressure (< 5.5 bar)", DiagnosticSeverity.CriticalStopEngine);
            RegisterCode("P20EE", "SCR/AdBlue", "SCR NOx Catalyst Efficiency Below Threshold (Refill DEF)", DiagnosticSeverity.Warning);
        }

        public void RegisterCode(string code, string sys, string desc, DiagnosticSeverity sev)
        {
            RegisteredCodes[code] = new DiagnosticTroubleCode
            {
                Code = code,
                SystemName = sys,
                Description = desc,
                Severity = sev
            };
        }

        public void TriggerDTC(string code)
        {
            if (RegisteredCodes.TryGetValue(code, out var dtc))
            {
                dtc.IsActive = true;
                dtc.TimestampOccurred = DateTime.UtcNow;
            }
        }

        public void ClearDTC(string code)
        {
            if (RegisteredCodes.TryGetValue(code, out var dtc))
            {
                dtc.IsActive = false;
            }
        }
    }
}
