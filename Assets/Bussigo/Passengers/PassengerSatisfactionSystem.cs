using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bussigo.Passengers
{
    /// <summary>
    /// Computes realistic passenger satisfaction ratings based on driving smoothness, punctuality, and comfort.
    /// </summary>
    public class PassengerSatisfactionSystem
    {
        public float CalculateAggregateSatisfaction(IReadOnlyList<PassengerProfile> activePassengers)
        {
            if (activePassengers == null || activePassengers.Count == 0) return 100.0f;

            float total = 0f;
            for (int i = 0; i < activePassengers.Count; i++)
            {
                total += activePassengers[i].satisfactionScore;
            }
            return total / activePassengers.Count;
        }

        public void ApplyDrivingTelemetryEvent(IReadOnlyList<PassengerProfile> passengers, float longitudinalAccelMss, float lateralAccelMss, float deltaTime)
        {
            if (passengers == null) return;

            // Harsh Braking Penalty (Decel > 3.0 m/s^2)
            if (longitudinalAccelMss < -3.0f)
            {
                float penalty = Mathf.Abs(longitudinalAccelMss + 3.0f) * 1.5f * deltaTime;
                for (int i = 0; i < passengers.Count; i++)
                {
                    passengers[i].satisfactionScore = Mathf.Clamp(passengers[i].satisfactionScore - penalty, 0f, 100f);
                }
            }

            // Severe Cornering Lateral G Penalty (Lateral Accel > 2.5 m/s^2)
            if (Mathf.Abs(lateralAccelMss) > 2.5f)
            {
                float penalty = (Mathf.Abs(lateralAccelMss) - 2.5f) * 1.2f * deltaTime;
                for (int i = 0; i < passengers.Count; i++)
                {
                    passengers[i].satisfactionScore = Mathf.Clamp(passengers[i].satisfactionScore - penalty, 0f, 100f);
                }
            }
        }
    }
}
