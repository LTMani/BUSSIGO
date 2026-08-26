using System;

namespace Bussigo.Game.Vehicles
{
    public class VehicleTelemetry
    {
        public float SpeedKmh { get; set; }
        public float EngineRpm { get; set; }
        public int CurrentGear { get; set; }
        public float PrimaryAirPressureBar { get; set; }
        public float SecondaryAirPressureBar { get; set; }
        public float TurboBoostBar { get; set; }
        public float ThrottleInput { get; set; }
        public float BrakeInput { get; set; }
        public float ClutchInput { get; set; }
        public float SteeringAngleDeg { get; set; }
        public float RetarderLevel { get; set; }
        public float LateralGForce { get; set; }
        public float LongitudinalGForce { get; set; }

        public bool AbsActive { get; set; }
        public bool ParkingBrakeEngaged { get; set; }
        public bool CruiseControlActive { get; set; }
        public float SetCruiseSpeedKmh { get; set; }
        public bool CheckEngineLight { get; set; }

        public string DiagnosticSummary => $"Spd: {SpeedKmh:F1} km/h | RPM: {EngineRpm:F0} | G: {CurrentGear} | Air: {PrimaryAirPressureBar:F1} bar | LatG: {LateralGForce:F2}";
    }
}
