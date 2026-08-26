using System;
using UnityEngine;

namespace Bussigo.Physics
{
    [Serializable]
    public class HeavyVehiclePhysicsModel
    {
        [Header("Vehicle Mass & Dimensions")]
        public float curbMassKg = 14500f;
        public float currentPayloadMassKg = 0f;
        public float TotalMassKg => curbMassKg + currentPayloadMassKg;

        public float wheelRadiusMeters = 0.52f;
        public float wheelbaseMeters = 6.2f;
        public float trackWidthMeters = 2.05f;
        public float maxSteerAngleDegrees = 34f;

        [Header("Subsystem Modules")]
        public EnginePowertrain Powertrain = new EnginePowertrain();
        public PneumaticAirCircuit AirBrakes = new PneumaticAirCircuit();
        public RetarderBrakeSystem Retarder = new RetarderBrakeSystem();
        public PacejkaTyreFriction TyreModel = new PacejkaTyreFriction();
        public MultiAxleSuspension FrontSuspension = new MultiAxleSuspension();
        public MultiAxleSuspension RearSuspension = new MultiAxleSuspension();

        public float CalculateSteerAngle(float steerInput01, float vehicleSpeedKmh)
        {
            float speedFactor = Mathf.Clamp01(1.0f - (Mathf.Abs(vehicleSpeedKmh) / 105.0f));
            float effectiveMaxSteer = maxSteerAngleDegrees * (0.35f + 0.65f * speedFactor);
            return Mathf.Clamp(steerInput01, -1f, 1f) * effectiveMaxSteer;
        }

        public float CalculateTotalBrakingForce(float brakeInput01, float vehicleSpeedKmh)
        {
            float serviceBrakeForce = 58000f * Mathf.Clamp01(brakeInput01) * AirBrakes.GetBrakingEfficiency();
            float retarderForce = Retarder.CalculateRetarderBrakingForce(vehicleSpeedKmh);
            return serviceBrakeForce + retarderForce;
        }

        public void UpdatePayload(int passengerCount, float luggageMassPerPaxKg = 15f)
        {
            currentPayloadMassKg = passengerCount * (65f + luggageMassPerPaxKg);
        }

        public void StepPhysics(float dt, float throttleInput01, float brakeInput01, float steerInput01, float vehicleSpeedKmh)
        {
            AirBrakes.UpdateCircuit(dt, brakeInput01, engineRunning: true);
            Powertrain.UpdateEngineRpm(vehicleSpeedKmh, wheelRadiusMeters, throttleInput01, dt);
        }
    }
}
