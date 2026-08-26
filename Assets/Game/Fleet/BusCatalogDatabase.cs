using System;
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
