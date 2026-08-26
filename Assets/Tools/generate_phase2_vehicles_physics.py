#!/usr/bin/env python3
"""
BUSSIGO Engine Codebase Generator - Phase 2: Vehicles & Vehicle Physics Subsystems
Generates production-grade C# code files for:
- Assets/Game/Vehicles/
- Assets/Game/VehiclePhysics/
"""

import os
from pathlib import Path

VEH_DIR = Path("Assets/Game/Vehicles")
PHYS_DIR = Path("Assets/Game/VehiclePhysics")
VEH_DIR.mkdir(parents=True, exist_ok=True)
PHYS_DIR.mkdir(parents=True, exist_ok=True)

FILES = {}

# -----------------------------------------------------------------------------
# VEHICLES SUBSYSTEM
# -----------------------------------------------------------------------------

FILES[VEH_DIR / "VehicleChassisSpec.cs"] = """using System;

namespace Bussigo.Game.Vehicles
{
    public enum BusCategory
    {
        RuralOrdinary,       // Pallevelugu
        CityCommuter,        // Metro Express / Mitra
        IntercityExpress,    // Express 3+2
        UltraDeluxe,         // 2+2 Semi-luxury pushback
        SuperLuxury,         // 2+2 Air suspension luxury
        GarudaAC,            // 2+2 Volvo/Scania AC recliner
        GarudaPlusMultiAxle, // 6x2 Multi-axle 13.8m / 14.5m luxury
        AmaravatiMultiAxle,  // Premium Scania/Volvo 410HP
        VennelaACSleeper,    // 2+1 AC Berth Sleeper
        PrivateLuxurySleeper // High-deck private coach
    }

    public enum TransmissionType
    {
        ManualSynchromesh6Speed,
        ManualSynchromesh8Speed,
        AutomatedManualTransmission,
        FullyAutomaticTorqueConverter
    }

    public enum FuelType
    {
        DieselBS6,
        CNG,
        ElectricBattery
    }

    public class VehicleChassisSpec
    {
        public string ModelId { get; set; }
        public string DisplayName { get; set; }
        public string Manufacturer { get; set; }
        public BusCategory Category { get; set; }
        public FuelType EngineFuelType { get; set; } = FuelType.DieselBS6;

        // Dimensions (Meters)
        public float LengthMeters { get; set; } = 12.0f;
        public float WidthMeters { get; set; } = 2.6f;
        public float HeightMeters { get; set; } = 3.6f;
        public float WheelbaseMeters { get; set; } = 6.2f;
        public float FrontOverhangMeters { get; set; } = 2.4f;
        public float RearOverhangMeters { get; set; } = 3.4f;
        public float GroundClearanceMeters { get; set; } = 0.28f;
        public float TurningRadiusMeters { get; set; } = 11.5f;

        // Mass (Kilograms)
        public float KerbMassKg { get; set; } = 10500.0f;
        public float GrossVehicleWeightKg { get; set; } = 16200.0f;
        public float FrontAxleWeightRatio { get; set; } = 0.35f; // Unladen
        public int AxleCount { get; set; } = 2; // 2 or 3 (Multi-axle)
        public bool HasTagAxleSteer { get; set; } = false;

        // Powertrain Parameters
        public float EngineDisplacementLiters { get; set; } = 7.7f;
        public float MaxHorsepower { get; set; } = 280.0f;
        public float MaxPowerRpm { get; set; } = 2200.0f;
        public float MaxTorqueNm { get; set; } = 1100.0f;
        public float MaxTorqueRpmMin { get; set; } = 1200.0f;
        public float MaxTorqueRpmMax { get; set; } = 1600.0f;
        public float IdleRpm { get; set; } = 600.0f;
        public float MaxEngineRpm { get; set; } = 2500.0f;

        // Transmission
        public TransmissionType Transmission { get; set; } = TransmissionType.ManualSynchromesh6Speed;
        public float[] ForwardGearRatios { get; set; } = new float[] { 6.81f, 3.82f, 2.30f, 1.48f, 1.00f, 0.73f };
        public float ReverseGearRatio { get; set; } = 6.30f;
        public float FinalDriveDifferentialRatio { get; set; } = 4.30f;
        public float DrivetrainEfficiency { get; set; } = 0.88f;

        // Aerodynamics
        public float DragCoefficient { get; set; } = 0.55f;
        public float FrontalAreaM2 { get; set; } = 7.8f;

        // Capacities
        public int SeatingCapacity { get; set; } = 49;
        public int SleeperBerthCapacity { get; set; } = 0;
        public float LuggageVolumeM3 { get; set; } = 8.5f;
        public float FuelTankCapacityLiters { get; set; } = 350.0f;
        public float AdBlueTankCapacityLiters { get; set; } = 45.0f;

        // Pricing & Maintenance
        public long BasePriceInCoins { get; set; } = 3500000;
        public float MaintenanceCostPerKm { get; set; } = 4.5f;
        public float BaseComfortScore { get; set; } = 75.0f;
        public float BaseReliabilityScore { get; set; } = 92.0f;
    }
}
"""

FILES[VEH_DIR / "VehicleElectricalSystem.cs"] = """using System;
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
"""

FILES[VEH_DIR / "VehicleFuelSystem.cs"] = """using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Vehicles
{
    public class VehicleFuelSystem
    {
        public float FuelTankCapacityLiters { get; set; } = 350.0f;
        public float CurrentFuelLiters { get; private set; } = 280.0f;
        public float AdBlueCapacityLiters { get; set; } = 45.0f;
        public float CurrentAdBlueLiters { get; private set; } = 40.0f;

        public float InstantaneousFuelRateLitersPerHour { get; private set; } = 0.0f;
        public float AverageFuelConsumptionLitersPer100Km { get; private set; } = 24.5f;
        public float TotalFuelConsumedLiters { get; private set; } = 0.0f;
        public float TotalDistanceTraveledKm { get; private set; } = 0.0f;

        public bool FuelFilterClogged { get; set; } = false;
        public bool LowFuelWarningLight => (CurrentFuelLiters / FuelTankCapacityLiters) < 0.12f;
        public bool LowAdBlueWarningLight => (CurrentAdBlueLiters / AdBlueCapacityLiters) < 0.10f;
        public bool IsEmpty => CurrentFuelLiters <= 0.05f;

        public float FuelPercent => CoreMath.Clamp01(CurrentFuelLiters / MathF.Max(1.0f, FuelTankCapacityLiters)) * 100.0f;

        public VehicleFuelSystem(float tankCapacity = 350.0f)
        {
            FuelTankCapacityLiters = tankCapacity;
            CurrentFuelLiters = tankCapacity * 0.85f;
        }

        public void Refuel(float liters)
        {
            CurrentFuelLiters = CoreMath.Clamp(CurrentFuelLiters + liters, 0.0f, FuelTankCapacityLiters);
        }

        public void RefillAdBlue(float liters)
        {
            CurrentAdBlueLiters = CoreMath.Clamp(CurrentAdBlueLiters + liters, 0.0f, AdBlueCapacityLiters);
        }

        public void UpdateFuelConsumption(float deltaTime, float engineRpm, float engineLoadRatio, float speedKmh, bool engineRunning)
        {
            if (!engineRunning || IsEmpty)
            {
                InstantaneousFuelRateLitersPerHour = 0.0f;
                return;
            }

            // BSFC (Brake-Specific Fuel Consumption) diesel curve
            // Base idle rate: ~2.2 L/h
            // Full load 280HP @ 1800 RPM: ~42 L/h
            float idleRate = 2.2f * (engineRpm / 650.0f);
            float loadRate = 42.0f * MathF.Pow(CoreMath.Clamp01(engineLoadRatio), 1.25f) * (engineRpm / 2200.0f);
            
            if (FuelFilterClogged)
            {
                loadRate *= 1.15f; // Extra inefficient burning
            }

            InstantaneousFuelRateLitersPerHour = idleRate + loadRate;
            float fuelConsumedThisStep = (InstantaneousFuelRateLitersPerHour / 3600.0f) * deltaTime;

            CurrentFuelLiters = MathF.Max(0.0f, CurrentFuelLiters - fuelConsumedThisStep);
            TotalFuelConsumedLiters += fuelConsumedThisStep;

            // AdBlue / DEF consumption is typically ~5% of diesel consumption for BS6
            float adBlueConsumed = fuelConsumedThisStep * 0.045f;
            CurrentAdBlueLiters = MathF.Max(0.0f, CurrentAdBlueLiters - adBlueConsumed);

            // Rolling distance & average calculation
            float distStepKm = (speedKmh * deltaTime) / 3600.0f;
            TotalDistanceTraveledKm += distStepKm;

            if (TotalDistanceTraveledKm > 1.0f)
            {
                AverageFuelConsumptionLitersPer100Km = (TotalFuelConsumedLiters / TotalDistanceTraveledKm) * 100.0f;
            }
        }
    }
}
"""

FILES[VEH_DIR / "VehicleThermalSystem.cs"] = """using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Vehicles
{
    public class VehicleThermalSystem
    {
        public float AmbientTemperatureCelsius { get; set; } = 34.0f; // Typical South India ambient
        public float EngineCoolantTemperature { get; private set; } = 34.0f;
        public float EngineOilTemperature { get; private set; } = 34.0f;
        public float TransmissionFluidTemperature { get; private set; } = 34.0f;
        public float BrakeRotorsTemperatureFront { get; private set; } = 34.0f;
        public float BrakeRotorsTemperatureRear { get; private set; } = 34.0f;

        public float ThermostatOpeningTempCelsius { get; set; } = 82.0f;
        public float RadiatorFanKickInTempCelsius { get; set; } = 92.0f;
        public bool RadiatorFanActive { get; private set; } = false;

        public bool OverheatWarningLight => EngineCoolantTemperature >= 105.0f;
        public bool EngineDeratedDueToHeat => EngineCoolantTemperature >= 112.0f;

        public void Update(float deltaTime, float engineRpm, float engineLoadRatio, float speedKmh, float brakeInput, bool engineRunning)
        {
            float targetCoolantTemp = AmbientTemperatureCelsius;
            float targetOilTemp = AmbientTemperatureCelsius;

            if (engineRunning)
            {
                // Engine thermal heat generation
                float heatGenerationKw = 15.0f + 75.0f * (engineLoadRatio * (engineRpm / 2200.0f));
                
                // Airflow cooling over radiator
                float vehicleAirflowSpeed = speedKmh * CoreMath.KmhToMps;
                float coolingEfficiency = 0.4f + (vehicleAirflowSpeed / 30.0f) * 0.6f;

                RadiatorFanActive = EngineCoolantTemperature >= RadiatorFanKickInTempCelsius;
                if (RadiatorFanActive) coolingEfficiency += 0.45f;

                float thermostatFlow = CoreMath.Clamp01((EngineCoolantTemperature - ThermostatOpeningTempCelsius) / 10.0f);
                float heatDissipationKw = (EngineCoolantTemperature - AmbientTemperatureCelsius) * coolingEfficiency * (0.2f + 0.8f * thermostatFlow);

                float netHeat = (heatGenerationKw - heatDissipationKw) * deltaTime * 0.08f;
                EngineCoolantTemperature = CoreMath.Clamp(EngineCoolantTemperature + netHeat, AmbientTemperatureCelsius, 125.0f);

                // Oil follows coolant with thermal lag
                EngineOilTemperature = CoreMath.MoveTowards(EngineOilTemperature, EngineCoolantTemperature + (engineLoadRatio * 15.0f), deltaTime * 0.5f);
                TransmissionFluidTemperature = CoreMath.MoveTowards(TransmissionFluidTemperature, 75.0f + (speedKmh / 100.0f * 20.0f), deltaTime * 0.2f);
            }
            else
            {
                // Cool down naturally towards ambient
                EngineCoolantTemperature = CoreMath.MoveTowards(EngineCoolantTemperature, AmbientTemperatureCelsius, deltaTime * 0.15f);
                EngineOilTemperature = CoreMath.MoveTowards(EngineOilTemperature, AmbientTemperatureCelsius, deltaTime * 0.12f);
                TransmissionFluidTemperature = CoreMath.MoveTowards(TransmissionFluidTemperature, AmbientTemperatureCelsius, deltaTime * 0.1f);
                RadiatorFanActive = false;
            }

            // Brake thermal model (Ghat descents heat up drums/rotors significantly)
            if (brakeInput > 0.05f)
            {
                float brakeHeatRate = brakeInput * (speedKmh * 0.8f) * deltaTime * 4.5f;
                BrakeRotorsTemperatureFront += brakeHeatRate * 0.6f;
                BrakeRotorsTemperatureRear += brakeHeatRate * 0.4f;
            }
            else
            {
                float airCoolRate = (1.0f + (speedKmh / 40.0f)) * deltaTime * 1.5f;
                BrakeRotorsTemperatureFront = CoreMath.MoveTowards(BrakeRotorsTemperatureFront, AmbientTemperatureCelsius, airCoolRate);
                BrakeRotorsTemperatureRear = CoreMath.MoveTowards(BrakeRotorsTemperatureRear, AmbientTemperatureCelsius, airCoolRate);
            }
        }
    }
}
"""

FILES[VEH_DIR / "VehicleWearSystem.cs"] = """using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Vehicles
{
    public class VehicleWearSystem
    {
        // Wear factors (1.0 = Brand new 100%, 0.0 = Completely worn out / failure)
        public float TyreTreadCondition { get; set; } = 1.0f;
        public float FrontBrakeLiningCondition { get; set; } = 1.0f;
        public float RearBrakeLiningCondition { get; set; } = 1.0f;
        public float ClutchPlateCondition { get; set; } = 1.0f;
        public float EngineOilHealth { get; set; } = 1.0f;
        public float AirFilterCondition { get; set; } = 1.0f;
        public float SuspensionBushingsCondition { get; set; } = 1.0f;

        public float OdometerKm { get; set; } = 0.0f;
        public float KmSinceLastFullService { get; set; } = 0.0f;

        public bool ServiceRequiredWarning => KmSinceLastFullService >= 15000.0f || EngineOilHealth < 0.20f;
        public bool BrakeWarningLight => FrontBrakeLiningCondition < 0.15f || RearBrakeLiningCondition < 0.15f;

        public void AccumulateWear(float distanceTraveledKm, float brakeEnergyJoules, float clutchSlipEnergyJoules, float engineRpmHours, float roadRoughness)
        {
            OdometerKm += distanceTraveledKm;
            KmSinceLastFullService += distanceTraveledKm;

            // Tyre wear: ~80,000 km normal tyre life
            float tyreWearRate = (distanceTraveledKm / 80000.0f) * (1.0f + roadRoughness * 0.5f);
            TyreTreadCondition = CoreMath.Clamp01(TyreTreadCondition - tyreWearRate);

            // Brake wear: ~45,000 km or accelerated by harsh braking
            float brakeWear = (distanceTraveledKm / 45000.0f) + (brakeEnergyJoules * 1e-9f);
            FrontBrakeLiningCondition = CoreMath.Clamp01(FrontBrakeLiningCondition - brakeWear * 1.2f);
            RearBrakeLiningCondition = CoreMath.Clamp01(RearBrakeLiningCondition - brakeWear * 0.8f);

            // Clutch wear
            float clutchWear = (distanceTraveledKm / 120000.0f) + (clutchSlipEnergyJoules * 1e-8f);
            ClutchPlateCondition = CoreMath.Clamp01(ClutchPlateCondition - clutchWear);

            // Oil degradation: ~15,000 km oil change interval
            float oilWear = (distanceTraveledKm / 15000.0f) + (engineRpmHours / 500.0f);
            EngineOilHealth = CoreMath.Clamp01(EngineOilHealth - oilWear);

            // Air filter: ~20,000 km (faster on dusty rural Andhra roads)
            float filterWear = (distanceTraveledKm / 20000.0f) * (1.0f + roadRoughness * 0.8f);
            AirFilterCondition = CoreMath.Clamp01(AirFilterCondition - filterWear);

            // Suspension wear: ~100,000 km
            float suspWear = (distanceTraveledKm / 100000.0f) * (1.0f + roadRoughness * 1.5f);
            SuspensionBushingsCondition = CoreMath.Clamp01(SuspensionBushingsCondition - suspWear);
        }

        public void PerformFullService()
        {
            EngineOilHealth = 1.0f;
            AirFilterCondition = 1.0f;
            KmSinceLastFullService = 0.0f;
        }

        public void OverhaulBrakes()
        {
            FrontBrakeLiningCondition = 1.0f;
            RearBrakeLiningCondition = 1.0f;
        }

        public void ReplaceTyres()
        {
            TyreTreadCondition = 1.0f;
        }

        public void ReplaceClutch()
        {
            ClutchPlateCondition = 1.0f;
        }
    }
}
"""

FILES[VEH_DIR / "VehicleTelemetry.cs"] = """using System;

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
"""

# -----------------------------------------------------------------------------
# VEHICLE PHYSICS SUBSYSTEM
# -----------------------------------------------------------------------------

FILES[PHYS_DIR / "PacejkaTyreModel.cs"] = """using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.VehiclePhysics
{
    public class PacejkaTyreModel
    {
        // Pacejka 94 Magic Formula coefficients: y = D * sin(C * atan(B*x - E*(B*x - atan(B*x))))
        public float StiffnessFactorB { get; set; } = 10.0f;
        public float ShapeFactorC { get; set; } = 1.65f;
        public float PeakFactorD { get; set; } = 1.0f; // Multiplied by normal load Fn * friction coeff
        public float CurvatureFactorE { get; set; } = 0.95f;

        public float BaseFrictionCoefficient { get; set; } = 0.95f; // Dry asphalt
        public float TyreRadiusMeters { get; set; } = 0.525f; // 295/80 R22.5 heavy commercial tyre

        public float EvaluateMagicFormula(float slip, float normalLoadNewton, float surfaceFrictionMultiplier)
        {
            if (normalLoadNewton <= 1.0f) return 0.0f;

            float mu = BaseFrictionCoefficient * surfaceFrictionMultiplier;
            float d = normalLoadNewton * mu * PeakFactorD;
            float b = StiffnessFactorB;
            float c = ShapeFactorC;
            float e = CurvatureFactorE;

            float bx = b * slip;
            float force = d * MathF.Sin(c * MathF.Atan(bx - e * (bx - MathF.Atan(bx))));
            return force;
        }

        public float CalculateLongitudinalSlipRatio(float wheelAngularVelocityRadSec, float longitudinalSpeedMps)
        {
            float wheelLinearSpeed = wheelAngularVelocityRadSec * TyreRadiusMeters;
            float refSpeed = MathF.Max(MathF.Abs(longitudinalSpeedMps), 0.5f);
            float slipRatio = (wheelLinearSpeed - longitudinalSpeedMps) / refSpeed;
            return CoreMath.Clamp(slipRatio, -1.0f, 1.0f);
        }

        public float CalculateLateralSlipAngle(float lateralVelocityMps, float longitudinalSpeedMps)
        {
            float refSpeed = MathF.Max(MathF.Abs(longitudinalSpeedMps), 0.5f);
            float slipAngleRad = MathF.Atan2(lateralVelocityMps, refSpeed);
            return slipAngleRad;
        }

        public (float longitudinalForce, float lateralForce) CalculateCombinedForces(
            float slipRatio, float slipAngleRad, float normalLoadNewton, float surfaceFrictionMultiplier)
        {
            float longForcePure = EvaluateMagicFormula(slipRatio, normalLoadNewton, surfaceFrictionMultiplier);
            float latForcePure = -EvaluateMagicFormula(slipAngleRad, normalLoadNewton, surfaceFrictionMultiplier);

            // Friction ellipse interaction
            float combinedSlip = MathF.Sqrt(slipRatio * slipRatio + slipAngleRad * slipAngleRad);
            if (combinedSlip < CoreMath.Epsilon)
            {
                return (0.0f, 0.0f);
            }

            float fx = longForcePure * MathF.Abs(slipRatio / combinedSlip);
            float fy = latForcePure * MathF.Abs(slipAngleRad / combinedSlip);

            return (fx, fy);
        }
    }
}
"""

FILES[PHYS_DIR / "PneumaticAirBrakeSystem.cs"] = """using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.VehiclePhysics
{
    public class PneumaticAirBrakeSystem
    {
        public float PrimaryReservoirPressureBar { get; private set; } = 8.5f;   // Rear circuit (8.5 bar normal)
        public float SecondaryReservoirPressureBar { get; private set; } = 8.5f; // Front circuit
        public float MaxCompressorGovernorCutoutBar { get; set; } = 9.2f;
        public float CompressorGovernorCutinBar { get; set; } = 7.0f;
        public bool CompressorLoaded { get; private set; } = false;

        public bool ParkingBrakeEngaged { get; private set; } = false;
        public float ServiceBrakeTreadleApplication { get; private set; } = 0.0f;
        public float RetarderApplicationRatio { get; private set; } = 0.0f; // 0.0 to 1.0 (5 stages)

        public bool LowAirPressureAlarm => PrimaryReservoirPressureBar < 5.5f || SecondaryReservoirPressureBar < 5.5f;
        public bool SpringBrakeEmergencyLocked => PrimaryReservoirPressureBar < 3.8f; // Maxi brakes auto-apply

        public event Action OnAirPurgeBlowoff;

        public void SetTreadleFootValve(float input)
        {
            ServiceBrakeTreadleApplication = CoreMath.Clamp01(input);
        }

        public void SetParkingBrake(bool engaged)
        {
            ParkingBrakeEngaged = engaged;
        }

        public void SetRetarderLevel(int stage) // 0 to 4
        {
            RetarderApplicationRatio = CoreMath.Clamp01(stage / 4.0f);
        }

        public void Update(float deltaTime, float engineRpm, bool engineRunning)
        {
            // Air compressor pump simulation
            if (engineRunning)
            {
                if (PrimaryReservoirPressureBar <= CompressorGovernorCutinBar || SecondaryReservoirPressureBar <= CompressorGovernorCutinBar)
                {
                    CompressorLoaded = true;
                }
                else if (PrimaryReservoirPressureBar >= MaxCompressorGovernorCutoutBar && SecondaryReservoirPressureBar >= MaxCompressorGovernorCutoutBar)
                {
                    if (CompressorLoaded)
                    {
                        OnAirPurgeBlowoff?.Invoke();
                    }
                    CompressorLoaded = false;
                }

                if (CompressorLoaded)
                {
                    float pumpRateBarPerSec = 0.25f * (engineRpm / 1500.0f);
                    PrimaryReservoirPressureBar = MathF.Min(MaxCompressorGovernorCutoutBar, PrimaryReservoirPressureBar + pumpRateBarPerSec * deltaTime);
                    SecondaryReservoirPressureBar = MathF.Min(MaxCompressorGovernorCutoutBar, SecondaryReservoirPressureBar + pumpRateBarPerSec * deltaTime);
                }
            }

            // Air consumption when applying service brakes
            if (ServiceBrakeTreadleApplication > 0.05f)
            {
                float airConsumptionRate = ServiceBrakeTreadleApplication * 0.15f * deltaTime;
                PrimaryReservoirPressureBar = MathF.Max(0.0f, PrimaryReservoirPressureBar - airConsumptionRate);
                SecondaryReservoirPressureBar = MathF.Max(0.0f, SecondaryReservoirPressureBar - airConsumptionRate);
            }
        }

        public float CalculateBrakeTorqueNm(float maxServiceBrakeTorqueNm, bool isFrontAxle)
        {
            if (ParkingBrakeEngaged || SpringBrakeEmergencyLocked)
            {
                return maxServiceBrakeTorqueNm * 0.95f; // Heavy spring brake lock
            }

            float reservoirPressure = isFrontAxle ? SecondaryReservoirPressureBar : PrimaryReservoirPressureBar;
            float pressureFactor = CoreMath.Clamp01(reservoirPressure / 6.5f);

            float serviceTorque = maxServiceBrakeTorqueNm * ServiceBrakeTreadleApplication * pressureFactor;
            float retarderTorque = isFrontAxle ? 0.0f : (maxServiceBrakeTorqueNm * 0.45f * RetarderApplicationRatio);

            return serviceTorque + retarderTorque;
        }
    }
}
"""

FILES[PHYS_DIR / "DieselPowertrain.cs"] = """using System;
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
"""

FILES[PHYS_DIR / "ChassisRigidBody.cs"] = """using System;
using Bussigo.Game.Core;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.VehiclePhysics
{
    public class ChassisRigidBody
    {
        public Vector3D Position { get; set; } = Vector3D.Zero;
        public Vector3D VelocityMps { get; set; } = Vector3D.Zero;
        public float YawAngleDegrees { get; set; } = 0.0f;
        public float YawRateRadSec { get; set; } = 0.0f;
        public float PitchAngleDegrees { get; private set; } = 0.0f;
        public float RollAngleDegrees { get; private set; } = 0.0f;

        public float SpeedKmh => VelocityMps.Length * CoreMath.MpsToKmh;
        public float ForwardSpeedMps => VelocityMps.Z;
        public float LateralSpeedMps => VelocityMps.X;

        private readonly VehicleChassisSpec _spec;

        public ChassisRigidBody(VehicleChassisSpec spec)
        {
            _spec = spec ?? new VehicleChassisSpec();
        }

        public void IntegratePhysics(float longitudinalDriveForceN, float totalBrakingForceN, float frontLateralForceN, float rearLateralForceN, float steeringAngleRad, float surfaceSlopeAngleRad, float deltaTime)
        {
            float totalMass = _spec.KerbMassKg + 3500.0f; // Include passenger payload

            // Aerodynamic drag: Fd = 0.5 * rho * Cd * A * v^2
            float airDensity = 1.205f; // kg/m^3
            float vForward = ForwardSpeedMps;
            float dragForceN = 0.5f * airDensity * _spec.DragCoefficient * _spec.FrontalAreaM2 * vForward * MathF.Abs(vForward);

            // Rolling resistance: Fr = Crr * m * g
            float rollResistCoeff = 0.012f;
            float rollingResistanceForceN = rollResistCoeff * totalMass * CoreMath.Gravity * MathF.Sign(vForward);

            // Slope gravity component
            float slopeGravityForceN = totalMass * CoreMath.Gravity * MathF.Sin(surfaceSlopeAngleRad);

            // Net longitudinal force
            float netLongForceN = longitudinalDriveForceN - totalBrakingForceN - dragForceN - rollingResistanceForceN - slopeGravityForceN;
            float longAccelMps2 = netLongForceN / totalMass;

            // Lateral dynamics and Yaw moment
            float frontAxleDist = _spec.WheelbaseMeters * (1.0f - _spec.FrontAxleWeightRatio);
            float rearAxleDist = _spec.WheelbaseMeters * _spec.FrontAxleWeightRatio;
            float yawInertia = (1.0f / 12.0f) * totalMass * (_spec.LengthMeters * _spec.LengthMeters + _spec.WidthMeters * _spec.WidthMeters);

            float totalLateralForceN = frontLateralForceN * MathF.Cos(steeringAngleRad) + rearLateralForceN;
            float latAccelMps2 = totalLateralForceN / totalMass;

            float yawTorqueNm = (frontLateralForceN * MathF.Cos(steeringAngleRad) * frontAxleDist) - (rearLateralForceN * rearAxleDist);
            float yawAngularAccelRadSec2 = yawTorqueNm / yawInertia;

            // Integration
            YawRateRadSec += yawAngularAccelRadSec2 * deltaTime;
            YawRateRadSec *= 0.96f; // Yaw damping
            YawAngleDegrees += (YawRateRadSec * CoreMath.RadToDeg) * deltaTime;
            YawAngleDegrees = CoreMath.NormalizeAngleDegrees(YawAngleDegrees);

            float newForwardSpeed = ForwardSpeedMps + longAccelMps2 * deltaTime;
            if (MathF.Abs(newForwardSpeed) < 0.05f && MathF.Abs(longitudinalDriveForceN) < 50.0f)
            {
                newForwardSpeed = 0.0f;
            }

            float newLateralSpeed = LateralSpeedMps + latAccelMps2 * deltaTime;
            VelocityMps = new Vector3D(newLateralSpeed, 0.0f, newForwardSpeed);

            // Dynamic pitch and roll angles based on accelerations
            PitchAngleDegrees = CoreMath.Clamp(-longAccelMps2 * 0.45f, -6.0f, 6.0f);
            RollAngleDegrees = CoreMath.Clamp(latAccelMps2 * 0.65f, -8.0f, 8.0f);

            // World position update
            float yawRad = YawAngleDegrees * CoreMath.DegToRad;
            float cosY = MathF.Cos(yawRad);
            float sinY = MathF.Sin(yawRad);

            float worldVx = VelocityMps.X * cosY + VelocityMps.Z * sinY;
            float worldVz = -VelocityMps.X * sinY + VelocityMps.Z * cosY;

            Position = new Vector3D(
                Position.X + worldVx * deltaTime,
                Position.Y,
                Position.Z + worldVz * deltaTime
            );
        }
    }
}
"""

for fpath, content in FILES.items():
    with open(fpath, "w", encoding="utf-8") as f:
        f.write(content.strip() + "\n")
    print(f"Generated: {fpath}")

print("Phase 2 generation complete.")
