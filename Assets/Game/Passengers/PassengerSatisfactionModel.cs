using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Passengers
{
    public class PassengerSatisfactionMetrics
    {
        public float ThermalComfortScore { get; set; } = 100.0f;
        public float DrivingSmoothnessScore { get; set; } = 100.0f;
        public float PunctualityScore { get; set; } = 100.0f;
        public float SeatCleanlinessScore { get; set; } = 100.0f;
        public float OverallSatisfactionScore { get; set; } = 100.0f;

        public int HarshBrakingEventsCount { get; set; } = 0;
        public int ExcessiveCorneringEventsCount { get; set; } = 0;
        public int SpeedingViolationsCount { get; set; } = 0;
    }

    public class PassengerSatisfactionModel
    {
        private readonly PassengerSatisfactionMetrics _metrics = new PassengerSatisfactionMetrics();
        public PassengerSatisfactionMetrics Metrics => _metrics;

        public void EvaluateDrivingDynamics(float lateralGForce, float longitudinalGForce, float speedKmh, float speedLimitKmh, float deltaTime)
        {
            if (MathF.Abs(longitudinalGForce) > 0.45f) // Harsh brake/accel
            {
                _metrics.HarshBrakingEventsCount++;
                _metrics.DrivingSmoothnessScore = MathF.Max(0.0f, _metrics.DrivingSmoothnessScore - 4.5f);
            }

            if (MathF.Abs(lateralGForce) > 0.38f) // Harsh cornering / swerve
            {
                _metrics.ExcessiveCorneringEventsCount++;
                _metrics.DrivingSmoothnessScore = MathF.Max(0.0f, _metrics.DrivingSmoothnessScore - 3.5f);
            }

            if (speedKmh > speedLimitKmh + 5.0f)
            {
                _metrics.SpeedingViolationsCount++;
                _metrics.DrivingSmoothnessScore = MathF.Max(0.0f, _metrics.DrivingSmoothnessScore - 1.5f * deltaTime);
            }

            // Natural recovery over steady smooth driving
            if (MathF.Abs(lateralGForce) < 0.15f && MathF.Abs(longitudinalGForce) < 0.15f)
            {
                _metrics.DrivingSmoothnessScore = CoreMath.MoveTowards(_metrics.DrivingSmoothnessScore, 100.0f, deltaTime * 0.25f);
            }

            CalculateOverallSatisfaction();
        }

        public void EvaluateThermalComfort(float cabinTempCelsius, float targetTempCelsius = 23.0f)
        {
            float tempDelta = MathF.Abs(cabinTempCelsius - targetTempCelsius);
            if (tempDelta < 2.0f)
            {
                _metrics.ThermalComfortScore = 100.0f;
            }
            else
            {
                _metrics.ThermalComfortScore = CoreMath.Clamp01(1.0f - (tempDelta - 2.0f) / 12.0f) * 100.0f;
            }
            CalculateOverallSatisfaction();
        }

        public void EvaluatePunctuality(float scheduledArrivalMinutes, float actualArrivalMinutes)
        {
            float delayMinutes = actualArrivalMinutes - scheduledArrivalMinutes;
            if (delayMinutes <= 0.0f)
            {
                _metrics.PunctualityScore = 100.0f; // On time or early
            }
            else
            {
                _metrics.PunctualityScore = CoreMath.Clamp01(1.0f - (delayMinutes / 45.0f)) * 100.0f;
            }
            CalculateOverallSatisfaction();
        }

        private void CalculateOverallSatisfaction()
        {
            _metrics.OverallSatisfactionScore = 
                _metrics.DrivingSmoothnessScore * 0.40f +
                _metrics.PunctualityScore * 0.30f +
                _metrics.ThermalComfortScore * 0.20f +
                _metrics.SeatCleanlinessScore * 0.10f;
        }
    }
}
