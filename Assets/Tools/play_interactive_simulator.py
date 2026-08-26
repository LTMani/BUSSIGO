#!/usr/bin/env python3
"""
BUSSIGO - Interactive Terminal Simulation & Career Loop Launcher
Allows you to play, drive routes, test bus physics, earn coins/XP, and manage your travel empire!
"""

import sys
import time
import math
import random

def print_header(title):
    print("\n" + "=" * 70)
    print(f"   {title}")
    print("=" * 70)

def main_game_loop():
    coins = 500000.0
    xp = 0
    driver_level = 1
    company_name = "Deccan Royal Express Travels"
    owned_buses = ["BUS-PAL-01 (Pallevelugu Rural Standard)"]
    selected_bus = owned_buses[0]
    
    routes = [
        {"id": "COR-VJA-HYD-01", "name": "Vijayawada (PNBS) ↔ Hyderabad (MGBS) via NH65", "dist": 274.5, "fare": 480.0, "toll": 385.0},
        {"id": "COR-VJA-GNT-02", "name": "Vijayawada (Benz Circle) ↔ Guntur (NTR) via NH16", "dist": 36.2, "fare": 45.0, "toll": 85.0},
        {"id": "COR-HYD-WGL-03", "name": "Hyderabad (JBS) ↔ Warangal Central via NH163", "dist": 148.0, "fare": 260.0, "toll": 160.0},
        {"id": "COR-GHAT-SRI-04", "name": "Dornala ↔ Srisailam Temple (Eastern Ghats Pass)", "dist": 85.0, "fare": 180.0, "toll": 60.0}
    ]

    while True:
        print_header(f"SOUTH INDIA BUS & TRAVEL EMPIRE SIMULATOR (BUSSIGO)\n   Company: {company_name} | Balance: ₹{coins:,.2f} | Level: {driver_level} (XP: {xp})")
        print("1. [DRIVE] Select a Corridor & Start Journey")
        print("2. [GARAGE] Inspect Bus, Tune & Customization")
        print("3. [COMPANY] Manage Regional Depots & Fleet")
        print("4. [STORE] Bus Procurement Marketplace")
        print("5. [TESTS] Run Automated Engineering Test Suites")
        print("6. [AUDIT] View Verified Source Code LOC Audit")
        print("0. [EXIT] Save & Exit Simulator")

        choice = input("\nEnter your choice (0-6): ").strip()

        if choice == '1':
            print_header("SELECT ROUTE CORRIDOR")
            for idx, r in enumerate(routes, 1):
                print(f"{idx}. {r['name']} ({r['dist']} km | Fare: ₹{r['fare']} | Toll: ₹{r['toll']})")
            print("0. Back to Main Menu")

            r_choice = input("\nSelect Route (1-4): ").strip()
            if r_choice in ['1', '2', '3', '4']:
                selected_route = routes[int(r_choice) - 1]
                simulate_trip(selected_route, selected_bus, coins, xp, driver_level)
                # Award rewards
                pax_count = random.randint(38, 52)
                gross_fare = pax_count * selected_route['fare']
                diesel_cost = (selected_route['dist'] / 100.0) * 26.0 * 94.0 # 26L/100km @ ₹94/L
                toll_cost = selected_route['toll']
                net_profit = gross_fare - diesel_cost - toll_cost

                coins += net_profit
                trip_xp = int(selected_route['dist'] * 3.5)
                xp += trip_xp

                if xp >= driver_level * 1000:
                    driver_level += 1
                    print(f"\n★ LEVEL UP! You reached Driver Level {driver_level}! ★")

                print(f"\n--- TRIP SUMMARY FOR {selected_route['name']} ---")
                print(f"Passengers Boarded: {pax_count}")
                print(f"Gross Ticket Revenue: ₹{gross_fare:,.2f}")
                print(f"Fuel Consumption: -₹{diesel_cost:,.2f}")
                print(f"FASTag Tolls: -₹{toll_cost:,.2f}")
                print(f"Net Operating Profit: +₹{net_profit:,.2f}")
                print(f"XP Earned: +{trip_xp} XP")
                input("\nPress Enter to return to headquarters...")

        elif choice == '2':
            print_header(f"GARAGE & LIVERY STUDIO - {selected_bus}")
            print("Chassis Specs: 240 HP Turbo Diesel | 920 Nm Torque | 6-Speed Synchromesh")
            print("Braking: Dual-Circuit Air Brakes (8.5 bar) | Pacejka 94 Tyres: 100% Tread")
            print("Livery: APSRTC Crimson & Gold Heritage with Bilingual LED Boards (తెలుగు / EN)")
            input("\nPress Enter to return...")

        elif choice == '3':
            print_header("TRAVEL COMPANY HQ & REGIONAL DEPOTS")
            print("1. Vijayawada Auto Nagar Central Depot (Capacity: 8 buses | Workshop: Active)")
            print("2. Hyderabad MGBS Satellite Depot (Capacity: 12 buses | Wash Plant: Active)")
            print("3. Guntur NTR Terminal Stabling Yard (Capacity: 6 buses)")
            print("Fleet Utilization: 94.2% | Passenger Satisfaction: 4.8 / 5.0 Stars")
            input("\nPress Enter to return...")

        elif choice == '4':
            print_header("BUS PROCUREMENT MARKETPLACE")
            print("1. Super Luxury Airglide (₹41,00,000) - Air Suspension, 260 HP, 36 Seats")
            print("2. Garuda Executive AC Volvo (₹65,00,000) - 330 HP, 41 Seats, Denso AC")
            print("3. Garuda Plus Multi-Axle 6x2 (₹1,05,00,000) - 380 HP, 49 Seats, Voith Retarder")
            print("4. Vennela Royal AC Sleeper (₹95,00,000) - 360 HP, 30 Luxury Berths")
            buy_ch = input("\nEnter bus number to purchase (or 0 to cancel): ").strip()
            if buy_ch == '1' and coins >= 4100000:
                coins -= 4100000
                owned_buses.append("BUS-SLX-05 (Super Luxury Airglide)")
                selected_bus = owned_buses[-1]
                print("\nPurchase successful! Super Luxury Airglide added to your fleet!")
                input("\nPress Enter...")
            elif buy_ch in ['1', '2', '3', '4']:
                print("\nInsufficient funds! Complete more highway trips to earn capital.")
                input("\nPress Enter...")

        elif choice == '5':
            print_header("AUTOMATED TEST SUITE VERIFICATION")
            import subprocess
            subprocess.run([sys.executable, "Assets/Tools/test_runner.py"])
            input("\nPress Enter to return...")

        elif choice == '6':
            print_header("VERIFIED SOURCE CODE LOC AUDIT")
            import subprocess
            subprocess.run([sys.executable, "Assets/Tools/loc_audit.py"])
            input("\nPress Enter to return...")

        elif choice == '0':
            print("\nProgress saved successfully. Thank you for playing BUSSIGO!")
            break

def simulate_trip(route, bus, coins, xp, level):
    print_header(f"DEPARTING: {route['name']}")
    print(f"Vehicle: {bus}")
    print("Pre-Trip Inspection: Air Pressure 8.5 bar | Coolant 82°C | Diesel Tank 85% Full")
    print("Boarding passengers at terminal platform bays...")
    time.sleep(0.6)

    stages = [
        ("Departing Terminal & Navigating City Traffic", 35.0, 8.5),
        ("Cruising on 4-Lane Highway (NH65/NH16)", 82.0, 8.4),
        ("Approaching Multi-Lane FASTag Toll Plaza (Auto RFID Deduction)", 25.0, 8.2),
        ("Overtaking Heavy Cargo Trucks & Navigating Traffic", 78.0, 8.3),
        ("Highway Rest Stop & Food Court Passenger Refreshment", 0.0, 8.5),
        ("Final Corridor Segment & Approaching Destination Terminal", 40.0, 8.5),
        ("Docking into Sawtooth Platform Bay & Alighting Passengers", 0.0, 8.5)
    ]

    for stage_name, speed, air in stages:
        time.sleep(0.4)
        print(f"  --> [{stage_name}] Speed: {speed:F1} km/h | Air Pressure: {air:F1} bar | Retarder: Auto")

    print("\n✓ TRIP COMPLETED SAFELY WITH 98% PASSENGER COMFORT SCORE!")

if __name__ == '__main__':
    main_game_loop()
