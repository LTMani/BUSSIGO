using System;
using System.Collections.Generic;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.Garage
{
    public class DiagnosticScanSessionReport28
    {
        public string ReportId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public List<string> ActiveDtcCodes { get; } = new List<string>();
        public float EngineHealthPercent { get; set; } = 100.0f;
        public float TransmissionHealthPercent { get; set; } = 100.0f;
        public float BrakePneumaticsHealthPercent { get; set; } = 100.0f;
    }

    public class WorkshopOBDScannerDiagnosticService28
    {
        public string ScannerSerialNumber => "BOSCH-COMMERCIAL-SCAN-028";
        public bool IsScannerConnected { get; private set; } = false;

        public DiagnosticScanSessionReport28 PerformFullSystemDiagnostics(VehicleWearSystem wear)
        {
            IsScannerConnected = true;
            var report = new DiagnosticScanSessionReport28
            {
                ReportId = "SCAN-REP-" + Guid.NewGuid().ToString("N").Substring(0, 8),
                EngineHealthPercent = wear.EngineOilHealth * 100.0f,
                TransmissionHealthPercent = wear.ClutchPlateCondition * 100.0f,
                BrakePneumaticsHealthPercent = (wear.FrontBrakeLiningCondition + wear.RearBrakeLiningCondition) * 50.0f
            };

            if (wear.FrontBrakeLiningCondition < 0.20f) report.ActiveDtcCodes.Add("C0045 - Brake Lining Below Minimum Wear Limit");
            if (wear.EngineOilHealth < 0.15f) report.ActiveDtcCodes.Add("P0524 - Oil Degradation High Viscosity Breakdown");

            return report;
        }
    }
}
