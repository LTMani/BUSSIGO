using System;
using Bussigo.Game.Core;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.VehiclePhysics
{
    public class DieselPowertrain
    {
        public float CurrentRpm { get; private set; } = 650.0f;
        public float CurrentEngineTorqueNm { get; private set; } = 0.0f;
        public float TurboBoostPressureBar { get; private set; } = 0.0f;
        public int CurrentGear { get; set; } = 0; // 0 = Neutral, -1 = Reverse, 1..8 = Forward
        public float ClutchEngagementRatio { get; set; } = 1.0f; // 0 = Disengaged, 1 = Fully engaged
        public bool IsEngineRunning { get; private set; } = false;

        private readonly VehicleChassisSpec _spec;
        private float _turboSpoolRpm = 0.0f;

        public DieselPowertrain(VehicleChassisSpec spec)
        {
            _spec = spec ?? new VehicleChassisSpec();
            CurrentRpm = _spec.IdleRpm;
        }

        public void StartEngine()
        {
            IsEngineRunning = true;
            CurrentRpm = _spec.IdleRpm;
        }

        public void StopEngine()
        {
            IsEngineRunning = false;
            CurrentRpm = 0.0f;
            CurrentEngineTorqueNm = 0.0f;
            TurboBoostPressureBar = 0.0f;
        }

        public float EvaluateTorqueCurve(float rpm, float throttleInput)
        {
            if (!IsEngineRunning || rpm < 350.0f) return 0.0f;

            float rpmNorm = CoreMath.Clamp(rpm, _spec.IdleRpm, _spec.MaxEngineRpm);
            float baseTorque;

            if (rpmNorm < _spec.MaxTorqueRpmMin)
            {
                baseTorque = CoreMath.Lerp(_spec.MaxTorqueNm * 0.55f, _spec.MaxTorqueNm, (rpmNorm - _spec.IdleRpm) / (_spec.MaxTorqueRpmMin - _spec.IdleRpm));
            }
            else if (rpmNorm <= _spec.MaxTorqueRpmMax)
            {
                baseTorque = _spec.MaxTorqueNm; // Flat peak torque plateau
            }
            else
            {
                baseTorque = CoreMath.Lerp(_spec.MaxTorqueNm, _spec.MaxTorqueNm * 0.60f, (rpmNorm - _spec.MaxTorqueRpmMax) / (_spec.MaxEngineRpm - _spec.MaxTorqueRpmMax));
            }

            // Turbo boost lag integration
            float targetTurboBoost = throttleInput * 2.2f; // Max 2.2 bar boost
            _turboSpoolRpm = CoreMath.MoveTowards(_turboSpoolRpm, targetTurboBoost, 1.5f * 0.02f);
            TurboBoostPressureBar = _turboSpoolRpm;

            float boostMultiplier = 0.65f + (TurboBoostPressureBar / 2.2f) * 0.35f;
            return baseTorque * CoreMath.Clamp01(throttleInput) * boostMultiplier;
        }

        public float CalculateWheelDriveTorque(float throttleInput, float wheelAngularSpeedRadSec, float deltaTime)
        {
            if (!IsEngineRunning || CurrentGear == 0)
            {
                CurrentEngineTorqueNm = 0.0f;
                if (IsEngineRunning)
                {
                    float freeRevTargetRpm = CoreMath.Lerp(_spec.IdleRpm, _spec.MaxEngineRpm, throttleInput);
                    CurrentRpm = CoreMath.MoveTowards(CurrentRpm, freeRevTargetRpm, deltaTime * 2500.0f);
                }
                return 0.0f;
            }

            float gearRatio = 0.0f;
            if (CurrentGear == -1)
            {
                gearRatio = -_spec.ReverseGearRatio;
            }
            else if (CurrentGear > 0 && CurrentGear <= _spec.ForwardGearRatios.Length)
            {
                gearRatio = _spec.ForwardGearRatios[CurrentGear - 1];
            }

            float totalRatio = gearRatio * _spec.FinalDriveDifferentialRatio;
            float targetEngineRpm = MathF.Abs(wheelAngularSpeedRadSec * totalRatio * 60.0f / (2.0f * MathF.PI));
            targetEngineRpm = MathF.Max(_spec.IdleRpm, targetEngineRpm);

            CurrentRpm = CoreMath.Lerp(CurrentRpm, targetEngineRpm, ClutchEngagementRatio);
            CurrentEngineTorqueNm = EvaluateTorqueCurve(CurrentRpm, throttleInput);

            float driveshaftTorque = CurrentEngineTorqueNm * totalRatio * _spec.DrivetrainEfficiency * ClutchEngagementRatio;
            return driveshaftTorque;
        }
    }
}
