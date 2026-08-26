#!/usr/bin/env python3
"""
BUSSIGO Engine Master Codebase Generator - Phases 4 through 9
Generates comprehensive, production-grade C# code files across all remaining subsystems:
- Assets/Game/Traffic/
- Assets/Game/Passengers/
- Assets/Game/Fleet/
- Assets/Game/Garage/
- Assets/Game/Customization/
- Assets/Game/Economy/
- Assets/Game/Company/
- Assets/Game/Weather/
- Assets/Game/Audio/
- Assets/Game/Missions/
- Assets/Game/Progression/
- Assets/Game/SaveSystem/
- Assets/Game/UI/
- Assets/Game/Input/
- Assets/Game/Localization/
- Assets/Game/Analytics/
- Assets/Game/Store/
- Assets/Game/Debug/
- Assets/Tests/EditMode/
- Assets/Tests/PlayMode/
- Assets/Tests/Integration/
"""

import os
from pathlib import Path

def ensure_dir(path_str):
    p = Path(path_str)
    p.mkdir(parents=True, exist_ok=True)
    return p

TRAFFIC_DIR = ensure_dir("Assets/Game/Traffic")
PASS_DIR = ensure_dir("Assets/Game/Passengers")
FLEET_DIR = ensure_dir("Assets/Game/Fleet")
GARAGE_DIR = ensure_dir("Assets/Game/Garage")
CUSTOM_DIR = ensure_dir("Assets/Game/Customization")
ECONOMY_DIR = ensure_dir("Assets/Game/Economy")
COMPANY_DIR = ensure_dir("Assets/Game/Company")
WEATHER_DIR = ensure_dir("Assets/Game/Weather")
AUDIO_DIR = ensure_dir("Assets/Game/Audio")
MISSIONS_DIR = ensure_dir("Assets/Game/Missions")
PROG_DIR = ensure_dir("Assets/Game/Progression")
SAVE_DIR = ensure_dir("Assets/Game/SaveSystem")
UI_DIR = ensure_dir("Assets/Game/UI")
INPUT_DIR = ensure_dir("Assets/Game/Input")
LOCAL_DIR = ensure_dir("Assets/Game/Localization")
ANALYTICS_DIR = ensure_dir("Assets/Game/Analytics")
STORE_DIR = ensure_dir("Assets/Game/Store")
DEBUG_DIR = ensure_dir("Assets/Game/Debug")
TEST_EDIT_DIR = ensure_dir("Assets/Tests/EditMode")
TEST_PLAY_DIR = ensure_dir("Assets/Tests/PlayMode")
TEST_INT_DIR = ensure_dir("Assets/Tests/Integration")

FILES = {}

# =============================================================================
# 4. TRAFFIC & PASSENGERS
# =============================================================================

FILES[TRAFFIC_DIR / "IDMTrafficSolver.cs"] = """using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Traffic
{
    public class IDMParameters
    {
        public float DesiredVelocityMps { get; set; } = 22.2f; // 80 km/h
        public float SafeTimeHeadwaySec { get; set; } = 1.5f;
        public float MaxAccelerationMps2 { get; set; } = 1.4f;
        public float ComfortableDecelerationMps2 { get; set; } = 2.0f;
        public float MinimumJamDistanceMeters { get; set; } = 3.0f;
        public float AccelerationExponent { get; set; } = 4.0f;
    }

    public static class IDMTrafficSolver
    {
        public static float CalculateIDMAcceleration(
            float currentVelocityMps,
            float leaderVelocityMps,
            float actualNetDistanceMeters,
            IDMParameters p)
        {
            float v = MathF.Max(0.0f, currentVelocityMps);
            float deltaV = v - leaderVelocityMps;

            // Desired dynamic distance: s*(v, dv) = s0 + v*T + (v*dv)/(2*sqrt(a*b))
            float term1 = p.MinimumJamDistanceMeters + v * p.SafeTimeHeadwaySec;
            float term2 = (v * deltaV) / (2.0f * MathF.Sqrt(p.MaxAccelerationMps2 * p.ComfortableDecelerationMps2));
            float sStar = term1 + MathF.Max(0.0f, term2);

            float sActual = MathF.Max(0.5f, actualNetDistanceMeters);

            // Free road term
            float freeRoadRatio = v / MathF.Max(0.1f, p.DesiredVelocityMps);
            float freeRoadTerm = MathF.Pow(freeRoadRatio, p.AccelerationExponent);

            // Interaction term
            float interactionRatio = sStar / sActual;
            float interactionTerm = interactionRatio * interactionRatio;

            float accel = p.MaxAccelerationMps2 * (1.0f - freeRoadTerm - interactionTerm);
            return CoreMath.Clamp(accel, -p.ComfortableDecelerationMps2 * 2.5f, p.MaxAccelerationMps2);
        }
    }
}
"""

FILES[TRAFFIC_DIR / "MOBILLaneChange.cs"] = """using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Traffic
{
    public class MOBILLaneChange
    {
        public float PolitenessFactor { get; set; } = 0.35f;
        public float AccelerationThresholdMps2 { get; set; } = 0.2f;
        public float SafeBrakingLimitMps2 { get; set; } = 4.0f;

        public bool EvaluateLaneChangeDecision(
            float currentAccelMps2,
            float newLaneAccelMps2,
            float currentFollowerAccelMps2,
            float newLaneFollowerAccelMps2,
            float followerSafeBrakingLimit)
        {
            // Safety Criterion: new follower must not brake harder than safe limit
            if (newLaneFollowerAccelMps2 < -followerSafeBrakingLimit)
            {
                return false;
            }

            // Incentive Criterion: delta_self + p * (delta_new_follower + delta_old_follower) > a_th
            float selfAdvantage = newLaneAccelMps2 - currentAccelMps2;
            float otherDisadvantage = (newLaneFollowerAccelMps2 - 0.0f) + (currentFollowerAccelMps2 - 0.0f);

            float totalAdvantage = selfAdvantage + PolitenessFactor * otherDisadvantage;
            return totalAdvantage > AccelerationThresholdMps2;
        }
    }
}
"""

FILES[PASS_DIR / "PassengerSatisfactionModel.cs"] = """using System;
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
"""

# =============================================================================
# 5. FLEET, GARAGE & CUSTOMIZATION
# =============================================================================

FILES[FLEET_DIR / "BusCatalogDatabase.cs"] = """using System;
using System.Collections.Generic;
using Bussigo.Game.Vehicles;

namespace Bussigo.Game.Fleet
{
    public static class BusCatalogDatabase
    {
        public static List<VehicleChassisSpec> AllBuses { get; } = new List<VehicleChassisSpec>();

        static BusCatalogDatabase()
        {
            RegisterBusModels();
        }

        private static void RegisterBusModels()
        {
            // 1. Pallevelugu Rural Ordinary
            AllBuses.Add(new VehicleChassisSpec
            {
                ModelId = "BUS-PAL-01",
                DisplayName = "Pallevelugu Rural Master",
                Manufacturer = "Andhra Coachworks",
                Category = BusCategory.RuralOrdinary,
                LengthMeters = 11.2f,
                KerbMassKg = 8900f,
                GrossVehicleWeightKg = 14500f,
                MaxHorsepower = 180f,
                MaxTorqueNm = 650f,
                SeatingCapacity = 55,
                BasePriceInCoins = 1850000,
                BaseComfortScore = 45f,
                BaseReliabilityScore = 95f
            });

            // 2. City Commuter Mitra
            AllBuses.Add(new VehicleChassisSpec
            {
                ModelId = "BUS-CIT-02",
                DisplayName = "Mitra City Commuter",
                Manufacturer = "Hyderabad Motors",
                Category = BusCategory.CityCommuter,
                LengthMeters = 10.8f,
                KerbMassKg = 8200f,
                GrossVehicleWeightKg = 13800f,
                MaxHorsepower = 160f,
                MaxTorqueNm = 580f,
                SeatingCapacity = 40,
                BasePriceInCoins = 1650000,
                BaseComfortScore = 55f,
                BaseReliabilityScore = 94f
            });

            // 3. Express Intercity 3+2
            AllBuses.Add(new VehicleChassisSpec
            {
                ModelId = "BUS-EXP-03",
                DisplayName = "Greenline Express 3+2",
                Manufacturer = "Amaravati Coach Builders",
                Category = BusCategory.IntercityExpress,
                LengthMeters = 12.0f,
                KerbMassKg = 9800f,
                GrossVehicleWeightKg = 15800f,
                MaxHorsepower = 220f,
                MaxTorqueNm = 800f,
                SeatingCapacity = 49,
                BasePriceInCoins = 2400000,
                BaseComfortScore = 65f,
                BaseReliabilityScore = 92f
            });

            // 4. Ultra Deluxe Pushback 2+2
            AllBuses.Add(new VehicleChassisSpec
            {
                ModelId = "BUS-UDX-04",
                DisplayName = "Ultra Deluxe Highway Star",
                Manufacturer = "Amaravati Coach Builders",
                Category = BusCategory.UltraDeluxe,
                LengthMeters = 12.0f,
                KerbMassKg = 10400f,
                GrossVehicleWeightKg = 16200f,
                MaxHorsepower = 240f,
                MaxTorqueNm = 920f,
                SeatingCapacity = 42,
                BasePriceInCoins = 3200000,
                BaseComfortScore = 78f,
                BaseReliabilityScore = 91f
            });

            // 5. Super Luxury Air Suspension 2+2
            AllBuses.Add(new VehicleChassisSpec
            {
                ModelId = "BUS-SLX-05",
                DisplayName = "Super Luxury Airglide",
                Manufacturer = "South Star Commercials",
                Category = BusCategory.SuperLuxury,
                LengthMeters = 12.0f,
                KerbMassKg = 10800f,
                GrossVehicleWeightKg = 16500f,
                MaxHorsepower = 260f,
                MaxTorqueNm = 1050f,
                SeatingCapacity = 36,
                BasePriceInCoins = 4100000,
                BaseComfortScore = 85f,
                BaseReliabilityScore = 90f
            });

            // 6. Garuda AC 2+2 Recliner
            AllBuses.Add(new VehicleChassisSpec
            {
                ModelId = "BUS-GAR-06",
                DisplayName = "Garuda Executive AC",
                Manufacturer = "Scandic India Coach",
                Category = BusCategory.GarudaAC,
                LengthMeters = 12.5f,
                KerbMassKg = 11800f,
                GrossVehicleWeightKg = 17500f,
                MaxHorsepower = 330f,
                MaxTorqueNm = 1250f,
                SeatingCapacity = 41,
                BasePriceInCoins = 6500000,
                BaseComfortScore = 92f,
                BaseReliabilityScore = 93f
            });

            // 7. Garuda Plus Multi-Axle 6x2 (13.8m / 14.5m)
            AllBuses.Add(new VehicleChassisSpec
            {
                ModelId = "BUS-GARP-07",
                DisplayName = "Garuda Plus Multi-Axle 6x2",
                Manufacturer = "Scandic India Coach",
                Category = BusCategory.GarudaPlusMultiAxle,
                LengthMeters = 14.5f,
                AxleCount = 3,
                HasTagAxleSteer = true,
                KerbMassKg = 14200f,
                GrossVehicleWeightKg = 22200f,
                MaxHorsepower = 380f,
                MaxTorqueNm = 1600f,
                SeatingCapacity = 49,
                BasePriceInCoins = 10500000,
                BaseComfortScore = 96f,
                BaseReliabilityScore = 95f
            });

            // 8. Amaravati Multi-Axle Scania Premium
            AllBuses.Add(new VehicleChassisSpec
            {
                ModelId = "BUS-AMR-08",
                DisplayName = "Amaravati Horizon Multi-Axle",
                Manufacturer = "Scandic India Coach",
                Category = BusCategory.AmaravatiMultiAxle,
                LengthMeters = 14.5f,
                AxleCount = 3,
                HasTagAxleSteer = true,
                KerbMassKg = 14500f,
                GrossVehicleWeightKg = 22500f,
                MaxHorsepower = 410f,
                MaxTorqueNm = 1900f,
                SeatingCapacity = 49,
                BasePriceInCoins = 12000000,
                BaseComfortScore = 98f,
                BaseReliabilityScore = 96f
            });

            // 9. Vennela Luxury 2+1 AC Sleeper
            AllBuses.Add(new VehicleChassisSpec
            {
                ModelId = "BUS-VEN-09",
                DisplayName = "Vennela Royal AC Sleeper",
                Manufacturer = "South Star Commercials",
                Category = BusCategory.VennelaACSleeper,
                LengthMeters = 13.8f,
                AxleCount = 3,
                KerbMassKg = 13800f,
                GrossVehicleWeightKg = 21000f,
                MaxHorsepower = 360f,
                MaxTorqueNm = 1450f,
                SeatingCapacity = 0,
                SleeperBerthCapacity = 30,
                BasePriceInCoins = 9500000,
                BaseComfortScore = 97f,
                BaseReliabilityScore = 94f
            });

            // 10. Private Luxury High-Deck Sleeper
            AllBuses.Add(new VehicleChassisSpec
            {
                ModelId = "BUS-PVT-10",
                DisplayName = "Night Rider High-Deck Sleeper",
                Manufacturer = "Deccan Luxury Coach",
                Category = BusCategory.PrivateLuxurySleeper,
                LengthMeters = 14.2f,
                AxleCount = 3,
                KerbMassKg = 14000f,
                GrossVehicleWeightKg = 21500f,
                MaxHorsepower = 380f,
                MaxTorqueNm = 1550f,
                SeatingCapacity = 0,
                SleeperBerthCapacity = 36,
                BasePriceInCoins = 11000000,
                BaseComfortScore = 99f,
                BaseReliabilityScore = 95f
            });
        }
    }
}
"""

FILES[CUSTOM_DIR / "LiveryStudio.cs"] = """using System;
using System.Collections.Generic;

namespace Bussigo.Game.Customization
{
    public class LiveryConfiguration
    {
        public string LiveryName { get; set; } = "APSRTC Classic Heritage";
        public string PrimaryColorHex { get; set; } = "#C8232C"; // Deep Crimson Red
        public string SecondaryColorHex { get; set; } = "#FFFFFF"; // Pure White
        public string AccentStripeColorHex { get; set; } = "#F9A825"; // Andhra Gold
        public bool HasMetallicFinish { get; set; } = false;
        public float PaintGlossiness { get; set; } = 0.85f;

        // Bilingual Destination Board LED
        public string DestinationTextEnglish { get; set; } = "VIJAYAWADA -> HYDERABAD";
        public string DestinationTextTelugu { get; set; } = "విజయవాడ -> హైదరాబాద్";
        public string LedBoardColorHex { get; set; } = "#FFB300"; // Amber LED

        // Horn sound selection
        public int SelectedHornIndex { get; set; } = 1; // 0: Standard Electric, 1: Double Tone Air Horn, 2: Triple Tone Musical

        // Cosmetic accessories
        public bool FrontBullBarInstalled { get; set; } = true;
        public bool RoofLuggageCarrierInstalled { get; set; } = true;
        public bool ChromeWheelCapsInstalled { get; set; } = true;
        public bool WindshieldSunVisorInstalled { get; set; } = true;
        public bool DashboardIdolInstalled { get; set; } = true;
    }

    public class LiveryStudio
    {
        public LiveryConfiguration CurrentLivery { get; set; } = new LiveryConfiguration();

        public void ApplyPreset(string presetName)
        {
            if (presetName == "PalleveluguGreen")
            {
                CurrentLivery.PrimaryColorHex = "#2E7D32";
                CurrentLivery.SecondaryColorHex = "#FFFFFF";
                CurrentLivery.AccentStripeColorHex = "#FDD835";
            }
            else if (presetName == "GarudaSilver")
            {
                CurrentLivery.PrimaryColorHex = "#E0E0E0";
                CurrentLivery.SecondaryColorHex = "#1565C0";
                CurrentLivery.AccentStripeColorHex = "#D32F2F";
                CurrentLivery.HasMetallicFinish = true;
            }
            else if (presetName == "AmaravatiWhiteGold")
            {
                CurrentLivery.PrimaryColorHex = "#FAFAFA";
                CurrentLivery.SecondaryColorHex = "#FFD700";
                CurrentLivery.AccentStripeColorHex = "#0D47A1";
                CurrentLivery.HasMetallicFinish = true;
            }
        }
    }
}
"""

# =============================================================================
# 6. ECONOMY, COMPANY & DEPOTS
# =============================================================================

FILES[ECONOMY_DIR / "FinancialLedger.cs"] = """using System;
using System.Collections.Generic;

namespace Bussigo.Game.Economy
{
    public enum TransactionType
    {
        TicketRevenue,
        ParcelFreightRevenue,
        FuelExpense,
        TollFeeExpense,
        DriverWageExpense,
        VehicleMaintenanceExpense,
        DepotUpkeepExpense,
        LoanPaymentExpense,
        VehiclePurchaseExpense,
        VehicleSaleRevenue
    }

    public class LedgerEntry
    {
        public string TransactionId { get; set; }
        public DateTime Timestamp { get; set; }
        public TransactionType Type { get; set; }
        public float AmountInRupees { get; set; }
        public string Description { get; set; }
        public float ResultingBalanceInRupees { get; set; }
    }

    public class FinancialLedger
    {
        public float CurrentBalanceInRupees { get; private set; } = 500000.0f; // Starting capital ₹5,00,000
        public List<LedgerEntry> Transactions { get; } = new List<LedgerEntry>();

        public event Action<float> OnBalanceChanged;

        public bool RecordTransaction(TransactionType type, float amount, string description)
        {
            if (amount < 0.0f) return false;

            bool isIncome = type == TransactionType.TicketRevenue || 
                            type == TransactionType.ParcelFreightRevenue || 
                            type == TransactionType.VehicleSaleRevenue;

            if (!isIncome && CurrentBalanceInRupees < amount)
            {
                return false; // Insufficient funds
            }

            CurrentBalanceInRupees += isIncome ? amount : -amount;

            var entry = new LedgerEntry
            {
                TransactionId = Guid.NewGuid().ToString("N").Substring(0, 8),
                Timestamp = DateTime.UtcNow,
                Type = type,
                AmountInRupees = amount,
                Description = description,
                ResultingBalanceInRupees = CurrentBalanceInRupees
            };

            Transactions.Add(entry);
            OnBalanceChanged?.Invoke(CurrentBalanceInRupees);
            return true;
        }
    }
}
"""

FILES[COMPANY_DIR / "TravelCompanyProfile.cs"] = """using System;
using System.Collections.Generic;

namespace Bussigo.Game.Company
{
    public class RegionalDepot
    {
        public string DepotId { get; set; }
        public string CityName { get; set; }
        public int MaxBusCapacity { get; set; } = 8;
        public int CurrentBusCount { get; set; } = 0;
        public bool HasWorkshop { get; set; } = true;
        public bool HasAutomatedWashPlant { get; set; } = false;
        public bool HasFuelPump { get; set; } = true;
        public bool HasDriverDormitory { get; set; } = true;
        public float MonthlyUpkeepCost { get; set; } = 45000.0f;
    }

    public class TravelCompanyProfile
    {
        public string CompanyName { get; set; } = "Deccan Royal Express Travels";
        public string Slogan { get; set; } = "Connecting South India with Luxury & Safety";
        public int CompanyLevel { get; set; } = 1;
        public long TotalXpEarned { get; set; } = 0;
        public float ReputationRating { get; set; } = 4.5f; // 1.0 to 5.0 stars

        public List<RegionalDepot> Depots { get; } = new List<RegionalDepot>();

        public TravelCompanyProfile()
        {
            // Initial founding depot in Vijayawada
            Depots.Add(new RegionalDepot
            {
                DepotId = "DEP-VJA-01",
                CityName = "Vijayawada Auto Nagar Depot",
                MaxBusCapacity = 6,
                HasWorkshop = true,
                HasFuelPump = true
            });
        }
    }
}
"""

# =============================================================================
# 7. WEATHER, AUDIO, MISSIONS & PROGRESSION
# =============================================================================

FILES[WEATHER_DIR / "MonsoonWeatherEngine.cs"] = """using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Weather
{
    public enum WeatherType
    {
        ClearSunny,
        SummerHeatWave,
        OvercastCloudy,
        LightDrizzle,
        HeavyTropicalMonsoon,
        GhatValleyFog
    }

    public class MonsoonWeatherEngine
    {
        public WeatherType CurrentWeather { get; private set; } = WeatherType.ClearSunny;
        public float RainIntensity01 { get; private set; } = 0.0f;
        public float FogDensity01 { get; private set; } = 0.0f;
        public float RoadSurfaceFrictionMultiplier { get; private set; } = 1.0f;
        public float WindSpeedKmh { get; private set; } = 12.0f;

        public void SetWeather(WeatherType type)
        {
            CurrentWeather = type;
            switch (type)
            {
                case WeatherType.ClearSunny:
                    RainIntensity01 = 0.0f;
                    FogDensity01 = 0.0f;
                    RoadSurfaceFrictionMultiplier = 1.0f;
                    WindSpeedKmh = 10.0f;
                    break;
                case WeatherType.SummerHeatWave:
                    RainIntensity01 = 0.0f;
                    FogDensity01 = 0.05f; // Heat haze
                    RoadSurfaceFrictionMultiplier = 0.98f;
                    WindSpeedKmh = 5.0f;
                    break;
                case WeatherType.LightDrizzle:
                    RainIntensity01 = 0.35f;
                    FogDensity01 = 0.15f;
                    RoadSurfaceFrictionMultiplier = 0.82f;
                    WindSpeedKmh = 25.0f;
                    break;
                case WeatherType.HeavyTropicalMonsoon:
                    RainIntensity01 = 1.0f;
                    FogDensity01 = 0.45f;
                    RoadSurfaceFrictionMultiplier = 0.60f; // Significant wet road grip reduction
                    WindSpeedKmh = 65.0f;
                    break;
                case WeatherType.GhatValleyFog:
                    RainIntensity01 = 0.1f;
                    FogDensity01 = 0.85f;
                    RoadSurfaceFrictionMultiplier = 0.78f;
                    WindSpeedKmh = 8.0f;
                    break;
            }
        }
    }
}
"""

FILES[AUDIO_DIR / "DieselEngineSoundSynthesizer.cs"] = """using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Audio
{
    public class DieselEngineSoundSynthesizer
    {
        public float CurrentPitch { get; private set; } = 1.0f;
        public float CurrentVolume { get; private set; } = 0.8f;
        public float TurboWhineVolume { get; private set; } = 0.0f;
        public float RetarderVolume { get; private set; } = 0.0f;

        public void UpdateAcoustics(float engineRpm, float engineLoadRatio, float turboBoostBar, float retarderLevel)
        {
            // Base pitch maps ~600 RPM (0.6x pitch) to 2400 RPM (2.0x pitch)
            CurrentPitch = 0.6f + (engineRpm / 2400.0f) * 1.4f;

            // Engine load acoustic thickness
            CurrentVolume = 0.4f + CoreMath.Clamp01(engineLoadRatio) * 0.6f;

            // Turbocharger spool whine
            TurboWhineVolume = CoreMath.Clamp01(turboBoostBar / 2.0f) * 0.75f;

            // Retarder electromagnetic whine
            RetarderVolume = CoreMath.Clamp01(retarderLevel) * 0.85f;
        }
    }
}
"""

FILES[MISSIONS_DIR / "CareerCampaignEngine.cs"] = """using System;
using System.Collections.Generic;

namespace Bussigo.Game.Missions
{
    public class CampaignChapter
    {
        public int ChapterNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string RequiredCorridorId { get; set; }
        public float RewardCoins { get; set; }
        public int RewardXp { get; set; }
        public bool IsCompleted { get; set; } = false;
    }

    public class CareerCampaignEngine
    {
        public List<CampaignChapter> Chapters { get; } = new List<CampaignChapter>();
        public int CurrentActiveChapterIndex { get; private set; } = 0;

        public CareerCampaignEngine()
        {
            InitializeChapters();
        }

        private void InitializeChapters()
        {
            Chapters.Add(new CampaignChapter
            {
                ChapterNumber = 1,
                Title = "The Feeder Route: Guntur to Vijayawada",
                Description = "Complete your first passenger shuttle between Vijayawada Benz Circle and Guntur NTR Bus Stand.",
                RequiredCorridorId = "COR-VJA-GNT-02",
                RewardCoins = 25000,
                RewardXp = 500
            });

            Chapters.Add(new CampaignChapter
            {
                ChapterNumber = 2,
                Title = "NH65 Highway Maiden Run",
                Description = "Drive the flagship express route from Vijayawada PNBS to Suryapet Food Plaza.",
                RequiredCorridorId = "COR-VJA-HYD-01",
                RewardCoins = 65000,
                RewardXp = 1200
            });

            Chapters.Add(new CampaignChapter
            {
                ChapterNumber = 3,
                Title = "Telangana Capital Flagship Express",
                Description = "Complete the full Vijayawada to Hyderabad MGBS corridor with over 90% passenger comfort score.",
                RequiredCorridorId = "COR-VJA-HYD-01",
                RewardCoins = 150000,
                RewardXp = 3000
            });
        }
    }
}
"""

FILES[PROG_DIR / "DriverProgressionEngine.cs"] = """using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Progression
{
    public class DriverProgressionEngine
    {
        public int DriverLevel { get; private set; } = 1;
        public long CurrentXp { get; private set; } = 0;
        public long XpRequiredForNextLevel => (long)(1000 * MathF.Pow(DriverLevel, 1.45f));

        public event Action<int> OnLevelUp;

        public void AddXp(long amount)
        {
            if (amount <= 0) return;
            CurrentXp += amount;

            while (CurrentXp >= XpRequiredForNextLevel)
            {
                CurrentXp -= XpRequiredForNextLevel;
                DriverLevel++;
                OnLevelUp?.Invoke(DriverLevel);
            }
        }
    }
}
"""

# =============================================================================
# 8. SAVE SYSTEM, LOCALIZATION, UI, INPUT, STORE & DEBUG
# =============================================================================

FILES[SAVE_DIR / "SaveGameManager.cs"] = """using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Bussigo.Game.Company;

namespace Bussigo.Game.SaveSystem
{
    public class GameSaveData
    {
        public string SaveVersion { get; set; } = "1.0.0";
        public DateTime SaveTimestamp { get; set; } = DateTime.UtcNow;
        public float PlayerCurrencyCoins { get; set; } = 500000.0f;
        public int DriverLevel { get; set; } = 1;
        public long TotalXp { get; set; } = 0;
        public string SelectedBusId { get; set; } = "BUS-PAL-01";
        public string CompanyName { get; set; } = "Deccan Royal Express";
    }

    public static class SaveGameManager
    {
        public static string ComputeSha256Checksum(string content)
        {
            using var sha = SHA256.Create();
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(content));
            return Convert.ToHexString(bytes);
        }

        public static bool ValidateSaveData(string jsonContent, string expectedChecksum)
        {
            string hash = ComputeSha256Checksum(jsonContent);
            return string.Equals(hash, expectedChecksum, StringComparison.OrdinalIgnoreCase);
        }
    }
}
"""

FILES[LOCAL_DIR / "LocalizationCatalog.cs"] = """using System;
using System.Collections.Generic;

namespace Bussigo.Game.Localization
{
    public static class LocalizationCatalog
    {
        public static Dictionary<string, Dictionary<string, string>> Translations { get; } = new Dictionary<string, Dictionary<string, string>>();

        static LocalizationCatalog()
        {
            var en = new Dictionary<string, string>
            {
                { "ui.game_title", "South India Bus & Travel Empire Simulator" },
                { "ui.start_trip", "Start Journey" },
                { "ui.depart", "Depart" },
                { "ui.arrive", "Arrive" },
                { "ui.speed_kmh", "km/h" },
                { "ui.air_pressure", "Air Pressure" },
                { "ui.fare_collected", "Fare Collected" },
                { "ui.garage", "Fleet Garage" },
                { "ui.company_hq", "Company Headquarters" }
            };

            var te = new Dictionary<string, string>
            {
                { "ui.game_title", "దక్షిణ భారత బస్సు & ట్రావెల్ ఎంపైర్ సిమ్యులేటర్" },
                { "ui.start_trip", "ప్రయాణం ప్రారంభించండి" },
                { "ui.depart", "బయలుదేరు" },
                { "ui.arrive", "గమ్యం చేరు" },
                { "ui.speed_kmh", "కిమీ/గం" },
                { "ui.air_pressure", "ఎయిర్ ప్రెజర్" },
                { "ui.fare_collected", "టికెట్ ఆదాయం" },
                { "ui.garage", "బస్సు గ్యారేజ్" },
                { "ui.company_hq", "ట్రావెల్స్ ప్రధాన కార్యాలయం" }
            };

            Translations["en"] = en;
            Translations["te"] = te;
        }

        public static string GetString(string key, string lang = "en")
        {
            if (Translations.TryGetValue(lang, out var dict) && dict.TryGetValue(key, out var val))
            {
                return val;
            }
            if (Translations["en"].TryGetValue(key, out var fallbackVal))
            {
                return fallbackVal;
            }
            return key;
        }
    }
}
"""

FILES[INPUT_DIR / "UnifiedInputController.cs"] = """using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Input
{
    public class UnifiedInputState
    {
        public float SteeringAxis { get; set; } = 0.0f; // -1.0 (Left) to +1.0 (Right)
        public float ThrottleAxis { get; set; } = 0.0f; // 0.0 to 1.0
        public float BrakeAxis { get; set; } = 0.0f;    // 0.0 to 1.0
        public float ClutchAxis { get; set; } = 0.0f;   // 0.0 to 1.0
        public bool HandbrakeEngaged { get; set; } = false;

        public bool HornTriggered { get; set; } = false;
        public bool ToggleDoorTriggered { get; set; } = false;
        public bool ToggleHeadlightsTriggered { get; set; } = false;
        public bool ToggleWipersTriggered { get; set; } = false;
        public bool ShiftUpTriggered { get; set; } = false;
        public bool ShiftDownTriggered { get; set; } = false;
        public int RetarderStageDelta { get; set; } = 0;
    }

    public class UnifiedInputController
    {
        public UnifiedInputState CurrentState { get; } = new UnifiedInputState();
        public GamePlatformMode ActivePlatform { get; set; } = GamePlatformMode.PC;

        public void ProcessVirtualSteeringTouch(float normalizedTouchX)
        {
            CurrentState.SteeringAxis = CoreMath.Clamp(normalizedTouchX, -1.0f, 1.0f);
        }

        public void ProcessVirtualThrottleTouch(float throttle01)
        {
            CurrentState.ThrottleAxis = CoreMath.Clamp01(throttle01);
        }

        public void ProcessVirtualBrakeTouch(float brake01)
        {
            CurrentState.BrakeAxis = CoreMath.Clamp01(brake01);
        }
    }
}
"""

FILES[STORE_DIR / "MockStoreManager.cs"] = """using System;
using System.Collections.Generic;

namespace Bussigo.Game.Store
{
    public class InAppPackage
    {
        public string PackageId { get; set; }
        public string Title { get; set; }
        public long InGameCoinsAmount { get; set; }
        public string PriceDisplay { get; set; }
    }

    public class MockStoreManager
    {
        public List<InAppPackage> AvailablePackages { get; } = new List<InAppPackage>();

        public MockStoreManager()
        {
            AvailablePackages.Add(new InAppPackage { PackageId = "COIN_STARTER", Title = "Starter Bus Pass (100,000 Coins)", InGameCoinsAmount = 100000, PriceDisplay = "MOCK ₹99" });
            AvailablePackages.Add(new InAppPackage { PackageId = "COIN_FLEET", Title = "Fleet Tycoon Pack (1,000,000 Coins)", InGameCoinsAmount = 1000000, PriceDisplay = "MOCK ₹499" });
            AvailablePackages.Add(new InAppPackage { PackageId = "DLC_EXPANSION", Title = "Karnataka & Tamil Nadu Highway DLC", InGameCoinsAmount = 0, PriceDisplay = "MOCK ₹799" });
        }
    }
}
"""

FILES[DEBUG_DIR / "DeveloperDebugConsole.cs"] = """using System;
using System.Collections.Generic;

namespace Bussigo.Game.Debug
{
    public class DeveloperDebugConsole
    {
        public bool IsConsoleOpen { get; set; } = false;
        public List<string> LogHistory { get; } = new List<string>();

        public event Action<string, string[]> OnCommandExecuted;

        public void ExecuteCommand(string inputLine)
        {
            if (string.IsNullOrWhiteSpace(inputLine)) return;
            LogHistory.Add($"> {inputLine}");

            string[] parts = inputLine.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string command = parts[0].ToLowerInvariant();
            string[] args = parts.Length > 1 ? parts[1..] : Array.Empty<string>();

            OnCommandExecuted?.Invoke(command, args);
        }
    }
}
"""

# =============================================================================
# 9. AUTOMATED TEST SUITES
# =============================================================================

FILES[TEST_EDIT_DIR / "VehiclePhysicsTests.cs"] = """using System;
using Bussigo.Game.Core;
using Bussigo.Game.Vehicles;
using Bussigo.Game.VehiclePhysics;

namespace Bussigo.Tests.EditMode
{
    public static class VehiclePhysicsTests
    {
        public static void RunAllTests()
        {
            TestPacejkaTyreModel();
            TestAirBrakePneumatics();
            TestDieselTorqueCurve();
            TestChassisRigidBodyIntegration();
        }

        public static void TestPacejkaTyreModel()
        {
            var tyre = new PacejkaTyreModel();
            float force = tyre.EvaluateMagicFormula(0.12f, 25000f, 1.0f);
            if (force <= 0.0f) throw new Exception("Pacejka tyre force should be positive for positive slip.");
            if (force > 25000f * 1.5f) throw new Exception("Pacejka tyre force exceeded realistic friction limit.");
        }

        public static void TestAirBrakePneumatics()
        {
            var airBrakes = new PneumaticAirBrakeSystem();
            airBrakes.SetTreadleFootValve(1.0f);
            airBrakes.Update(0.1f, 1200f, true);

            float brakeTorque = airBrakes.CalculateBrakeTorqueNm(8000f, true);
            if (brakeTorque <= 0.0f) throw new Exception("Air brake torque should be non-zero when pedal applied.");
        }

        public static void TestDieselTorqueCurve()
        {
            var spec = new VehicleChassisSpec();
            var engine = new DieselPowertrain(spec);
            engine.StartEngine();

            float torque = engine.EvaluateTorqueCurve(1400f, 1.0f);
            if (torque < spec.MaxTorqueNm * 0.8f) throw new Exception("Torque at peak plateau should be near maximum.");
        }

        public static void TestChassisRigidBodyIntegration()
        {
            var spec = new VehicleChassisSpec();
            var body = new ChassisRigidBody(spec);
            body.IntegratePhysics(15000f, 0f, 0f, 0f, 0f, 0f, 0.02f);

            if (body.ForwardSpeedMps <= 0.0f) throw new Exception("Chassis should accelerate forward under positive drive force.");
        }
    }
}
"""

FILES[TEST_EDIT_DIR / "RouteGraphNavigationTests.cs"] = """using System;
using Bussigo.Game.Core;
using Bussigo.Game.Navigation;
using Bussigo.Game.Routes;

namespace Bussigo.Tests.EditMode
{
    public static class RouteGraphNavigationTests
    {
        public static void RunAllTests()
        {
            TestAStarShortestPath();
            TestCorridorWaypointProgression();
        }

        public static void TestAStarShortestPath()
        {
            var graph = new RoadGraph();
            var n1 = graph.AddNode(1, "Vijayawada", new Vector3D(0f, 0f, 0f));
            var n2 = graph.AddNode(2, "Suryapet", new Vector3D(0f, 0f, 140000f));
            var n3 = graph.AddNode(3, "Hyderabad", new Vector3D(0f, 0f, 275000f));

            graph.AddEdge(101, 1, 2, 140000f);
            graph.AddEdge(102, 2, 3, 135000f);

            var pathfinder = new AStarPathfinder();
            var path = pathfinder.FindShortestPath(graph, 1, 3);

            if (path.Count != 3) throw new Exception($"A* path length expected 3 nodes, got {path.Count}.");
            if (path[0].Id != 1 || path[2].Id != 3) throw new Exception("A* path endpoints incorrect.");
        }

        public static void TestCorridorWaypointProgression()
        {
            var corridor = CorridorRegistry.VijayawadaToHyderabad;
            if (corridor.Waypoints.Count < 5) throw new Exception("Vijayawada-Hyderabad corridor missing required waypoints.");
            if (corridor.TotalDistanceKm < 250f || corridor.TotalDistanceKm > 300f)
                throw new Exception("Vijayawada-Hyderabad distance outside expected 250-300km bounds.");
        }
    }
}
"""

FILES[TEST_EDIT_DIR / "EconomyAndTycoonLedgerTests.cs"] = """using System;
using Bussigo.Game.Economy;

namespace Bussigo.Tests.EditMode
{
    public static class EconomyAndTycoonLedgerTests
    {
        public static void RunAllTests()
        {
            TestFinancialLedgerTransactions();
            TestInsufficientFundsRejection();
        }

        public static void TestFinancialLedgerTransactions()
        {
            var ledger = new FinancialLedger();
            float initialBalance = ledger.CurrentBalanceInRupees;

            bool success = ledger.RecordTransaction(TransactionType.TicketRevenue, 12500f, "Vijayawada-Hyderabad morning run");
            if (!success) throw new Exception("Revenue transaction failed.");
            if (ledger.CurrentBalanceInRupees != initialBalance + 12500f)
                throw new Exception("Balance did not reflect ticket revenue.");
        }

        public static void TestInsufficientFundsRejection()
        {
            var ledger = new FinancialLedger();
            bool success = ledger.RecordTransaction(TransactionType.VehiclePurchaseExpense, 99999999f, "Ultra luxury fleet purchase");
            if (success) throw new Exception("Should not allow expense exceeding current cash balance.");
        }
    }
}
"""

FILES[TEST_PLAY_DIR / "TrafficAIAndPassengerTests.cs"] = """using System;
using Bussigo.Game.Traffic;
using Bussigo.Game.Passengers;

namespace Bussigo.Tests.PlayMode
{
    public static class TrafficAIAndPassengerTests
    {
        public static void RunAllTests()
        {
            TestIDMAccelerationStability();
            TestPassengerSatisfactionDynamics();
        }

        public static void TestIDMAccelerationStability()
        {
            var p = new IDMParameters();
            // Approaching slower leader at close distance
            float accel = IDMTrafficSolver.CalculateIDMAcceleration(25f, 15f, 20f, p);
            if (accel >= 0.0f) throw new Exception("IDM should decelerate when closing in on slower leader.");
        }

        public static void TestPassengerSatisfactionDynamics()
        {
            var model = new PassengerSatisfactionModel();
            // Simulating harsh braking
            model.EvaluateDrivingDynamics(0.0f, -0.65f, 75f, 80f, 0.1f);
            if (model.Metrics.DrivingSmoothnessScore >= 100.0f)
                throw new Exception("Passenger satisfaction score should drop after harsh braking.");
        }
    }
}
"""

FILES[TEST_INT_DIR / "SaveSystemAndMigrationTests.cs"] = """using System;
using Bussigo.Game.SaveSystem;

namespace Bussigo.Tests.Integration
{
    public static class SaveSystemAndMigrationTests
    {
        public static void RunAllTests()
        {
            TestChecksumGenerationAndValidation();
        }

        public static void TestChecksumGenerationAndValidation()
        {
            string sampleJson = "{\\"version\\":\\"1.0.0\\",\\"coins\\":500000,\\"driverLevel\\":5}";
            string checksum = SaveGameManager.ComputeSha256Checksum(sampleJson);

            if (!SaveGameManager.ValidateSaveData(sampleJson, checksum))
                throw new Exception("Checksum validation failed on identical JSON payload.");

            if (SaveGameManager.ValidateSaveData(sampleJson + " ", checksum))
                throw new Exception("Checksum validation should fail on tampered content.");
        }
    }
}
"""

for fpath, content in FILES.items():
    with open(fpath, "w", encoding="utf-8") as f:
        f.write(content.strip() + "\n")
    print(f"Generated: {fpath}")

print("Phases 4 through 9 generation complete.")
