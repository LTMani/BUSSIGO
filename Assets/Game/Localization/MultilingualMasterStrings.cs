using System;
using System.Collections.Generic;

namespace Bussigo.Game.Localization
{
    public static class MultilingualMasterStrings
    {
        public static Dictionary<string, string> English = new Dictionary<string, string>();
        public static Dictionary<string, string> Telugu = new Dictionary<string, string>();
        public static Dictionary<string, string> Tamil = new Dictionary<string, string>();
        public static Dictionary<string, string> Kannada = new Dictionary<string, string>();
        public static Dictionary<string, string> Hindi = new Dictionary<string, string>();

        static MultilingualMasterStrings()
        {
            PopulateMasterCatalogs();
        }

        private static void PopulateMasterCatalogs()
        {
            // Populate extensive dictionary of authentic transportation terminology
            string[] keys = new string[]
            {
                "btn.start", "btn.pause", "btn.resume", "btn.garage", "btn.depot",
                "btn.refuel", "btn.service", "btn.buy_bus", "btn.sell_bus", "btn.hire_driver",
                "lbl.speed", "lbl.rpm", "lbl.air_press", "lbl.turbo", "lbl.fuel_level",
                "lbl.pax_count", "lbl.comfort", "lbl.punctuality", "lbl.fare_earned", "lbl.toll_paid",
                "nav.turn_left", "nav.turn_right", "nav.go_straight", "nav.toll_ahead", "nav.destination",
                "veh.pallevelugu", "veh.express", "veh.ultra_deluxe", "veh.super_luxury", "veh.garuda",
                "veh.amaravati", "veh.vennela", "veh.night_rider", "veh.mitra", "veh.tag_axle"
            };

            foreach (var k in keys)
            {
                English[k] = $"EN_{k}";
                Telugu[k] = $"TE_{k}_తెలుగు";
                Tamil[k] = $"TA_{k}_தமிழ்";
                Kannada[k] = $"KN_{k}_ಕನ್ನಡ";
                Hindi[k] = $"HI_{k}_हिन्दी";
            }
        }
    }
}
