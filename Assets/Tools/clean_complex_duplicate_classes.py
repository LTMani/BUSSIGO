import glob
import os

def clean_cargo_calculators():
    for i in range(1, 31):
        num_str = f"{i:02d}"
        path = f"Assets/Game/Passengers/ParcelCargoTariffCalculator{num_str}.cs"
        if not os.path.exists(path):
            continue

        base_rate = 4.50 + (i * 0.40)
        min_charge = 120.0 + (i * 30.0)
        express_pct = 20.0 + (i * 5.0)

        code = f"""using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Passengers
{{
    public class ParcelCargoTariffCalculator{num_str}
    {{
        public float BaseFreightRatePerKgPer100Km {{ get; set; }} = {base_rate:.2f}f;
        public float MinimumFreightDocketChargeRupees {{ get; set; }} = {min_charge:.1f}f;
        public float ExpressParcelSurchargePercent {{ get; set; }} = {express_pct:.1f}f;

        public float CalculateFreightFare(float weightKg, float distanceKm, bool isExpressDelivery)
        {{
            float ratePerKg = BaseFreightRatePerKgPer100Km * (distanceKm / 100.0f);
            float rawFare = weightKg * ratePerKg;

            if (isExpressDelivery)
            {{
                rawFare *= (1.0f + ExpressParcelSurchargePercent / 100.0f);
            }}

            return MathF.Max(MinimumFreightDocketChargeRupees, rawFare);
        }}
    }}
}}
"""
        with open(path, "w", encoding="utf-8") as fp:
            fp.write(code)

    print("Cleaned all ParcelCargoTariffCalculator files.")

def clean_seat_reservation_matrices():
    for i in range(1, 31):
        num_str = f"{i:02d}"
        path = f"Assets/Game/Passengers/PassengerSeatReservationMatrix{num_str}.cs"
        if not os.path.exists(path):
            continue

        total_seats = 36 + (i % 12)
        base_fare = 650.0 + (i * 25.0)

        code = f"""using System;
using System.Collections.Generic;

namespace Bussigo.Game.Passengers
{{
    public class PassengerSeatReservationMatrix{num_str}
    {{
        public string BusLayoutCode => "LAYOUT-CONFIG-{num_str}";
        public int TotalSeatsCount {{ get; set; }} = {total_seats};
        public List<SeatSlot> Seats {{ get; }} = new List<SeatSlot>();

        public PassengerSeatReservationMatrix{num_str}()
        {{
            for (int s = 1; s <= TotalSeatsCount; s++)
            {{
                Seats.Add(new SeatSlot
                {{
                    SeatNumber = s,
                    Type = (s % 4 == 0 || s % 4 == 1) ? SeatType.WindowSeat : SeatType.AisleSeat,
                    IsBooked = false,
                    PassengerName = string.Empty,
                    SeatFareRupees = {base_fare:.2f}f
                }});
            }}
        }}

        public bool ReserveSeat(int seatNumber, string passengerName)
        {{
            var slot = Seats.Find(x => x.SeatNumber == seatNumber);
            if (slot == null || slot.IsBooked) return false;

            slot.IsBooked = true;
            slot.PassengerName = passengerName;
            return true;
        }}
    }}
}}
"""
        with open(path, "w", encoding="utf-8") as fp:
            fp.write(code)

    print("Cleaned all PassengerSeatReservationMatrix files.")

def clean_bus_terminal_layouts():
    for i in range(1, 21):
        num_str = f"{i:02d}"
        path = f"Assets/Game/World/BusTerminalLayoutModel{num_str}.cs"
        if not os.path.exists(path):
            continue

        total_bays = 16 + (i * 2)

        code = f"""using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.World
{{
    public class BusTerminalLayoutModel{num_str}
    {{
        public string TerminalCode => "TERM-SOUTH-{num_str}";
        public string TerminalNameEnglish => "Major South Bus Station Hub {num_str}";
        public string TerminalNameTelugu => "ప్రధాన బస్ స్టేషన్ కాంప్లెక్స్ {num_str}";
        public int TotalPlatformBays {{ get; set; }} = {total_bays};
        public List<BusPlatformBay> Platforms {{ get; }} = new List<BusPlatformBay>();

        public BusTerminalLayoutModel{num_str}()
        {{
            for (int b = 1; b <= TotalPlatformBays; b++)
            {{
                Platforms.Add(new BusPlatformBay
                {{
                    BayNumber = b,
                    DestinationSignboardEnglish = $"Platform Bay {{b}} Intercity Corridor",
                    DestinationSignboardTelugu = $"ప్లాట్‌ఫారమ్ {{b}} అంతర్రాష్ట్ర సర్వీస్",
                    IsOccupiedByBus = false,
                    DockPosition = new Vector3D(b * 12.0, 0.0, 0.0)
                }});
            }}
        }}
    }}
}}
"""
        with open(path, "w", encoding="utf-8") as fp:
            fp.write(code)

    print("Cleaned all BusTerminalLayoutModel files.")

if __name__ == '__main__':
    clean_cargo_calculators()
    clean_seat_reservation_matrices()
    clean_bus_terminal_layouts()
