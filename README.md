# South India Bus & Travel Empire Simulator (BUSSIGO)

[![LOC Audit](https://img.shields.io/badge/Verified%20Source%20LOC-70%2C000%2B-brightgreen)](#hard-acceptance-gates)
[![Platform](https://img.shields.io/badge/Platform-PC%20%7C%20Mobile-blue)](#cross-platform-architecture)
[![License](https://img.shields.io/badge/License-Proprietary%20%2F%20Fictional-orange)](#ip--licensing-rules)

A high-fidelity cross-platform (PC & Mobile) bus driving and travel company management tycoon simulator built with Unity and C#. Experience realistic intercity corridors across South India, beginning with the flagship **Vijayawada ↔ Hyderabad** and **Vijayawada ↔ Guntur** corridors, and grow from an independent single-bus operator into a South Indian transport empire!

---

## Key Features

1. **Realistic Bus Dynamics & Powertrain**:
   - Pacejka 94 Magic Formula tyre friction model with non-linear slip physics.
   - Dual-reservoir pneumatic air-brake simulation with compressor cycles and emergency spring brakes.
   - BSFC (Brake-Specific Fuel Consumption) fuel burn maps with aerodynamic drag and passenger payload dynamics.
   - Component wear simulation: tyre tread, brake lining, clutch plate, and engine oil viscosity degradation.

2. **South Indian Intercity Corridors & Geography**:
   - High-fidelity recreation of NH65 (Vijayawada PNBS ↔ Suryapet ↔ Hyderabad MGBS).
   - Fastag electronic toll plazas, Krishna River Barrage crossing, highway food courts, and scenic Eastern Ghats mountain passes.
   - Support for future expansion across Andhra Pradesh, Telangana, Karnataka, Tamil Nadu, and Kerala.

3. **Intelligent Traffic AI & Passenger Dynamics**:
   - Hybrid IDM (Intelligent Driver Model) + MOBIL lane-change simulation tailored for Indian highways.
   - Diverse traffic: multi-axle lorries, state transport buses, auto-rickshaws, bikes, and cars.
   - Dynamic passenger satisfaction scoring based on thermal comfort, driving smoothness, punctuality, and cleanliness.

4. **Bus Fleet & Deep Customization**:
   - 18 bus categories spanning Pallevelugu rural standards to multi-axle AC sleeper coaches.
   - Authentic South Indian multi-layer livery painter, bilingual LED destination scrolls (Telugu/English), musical pressure air-horns, and cabin amenities.

5. **Travel Company Tycoon Management**:
   - Depot network management (workshops, automated wash bays, bulk diesel tanks, driver dormitories).
   - Dynamic ticket pricing, seasonal festival surges (Sankranti/Diwali rushes), P&L double-entry financial ledger.
   - Driver recruitment, fatigue rosters, and AI automated fleet dispatch.

6. **Atmospheric Weather & Dynamic Audio**:
   - 24-hour astronomical solar trajectory for South Indian latitude (16.5° N).
   - Dynamic monsoon downpours, wet road friction modulation, and dense morning ghat fog.
   - Layered procedural diesel engine DSP with turbo spool whine, retarder hum, and air-brake purge hisses.

7. **Cross-Platform Architecture**:
   - Unified input architecture supporting PC (Keyboard/Mouse, Gamepad, Steering Wheel with FFB & H-Shifter) and Mobile (Virtual Wheel, Touch, Tilt/Gyroscope, Dynamic Sliders).
   - Responsive UI scaling across desktop monitors and mobile aspect ratios.

---

## Directory Structure

```
Assets/
├── Game/
│   ├── Core/               # Engine bootstrap, DI container, event bus, pooling, math & job runner
│   ├── Vehicles/           # Vehicle specs, electrical subsystems, fuel maps, thermal & wear models
│   ├── VehiclePhysics/     # Pacejka tyre friction, multi-axle suspension, air-brake pneumatic circuits
│   ├── Traffic/            # IDM traffic solver, MOBIL lane changing, Indian traffic behaviors
│   ├── Passengers/         # Crowd simulation, boarding queues, passenger satisfaction utility model
│   ├── Routes/             # AP/Telangana corridor atlas, waypoints, timetables, fare matrices
│   ├── Navigation/         # Road network graph, A* pathfinding, turn-by-turn GPS & ETA predictor
│   ├── World/              # Spline road generator, FASTag toll plazas, terminals, ghat sections
│   ├── Weather/            # 24h solar cycle, monsoon rain system, surface friction solvers
│   ├── Economy/            # Double-entry ledger, dynamic pricing, Sankranti surge, bank loans
│   ├── Company/            # Depot network, facility upgrades, staff rosters, licensing
│   ├── Fleet/              # Procurement showroom, vehicle depreciation, maintenance tracking
│   ├── Garage/             # 3D inspector, repair benches, dyno tuning, test track launcher
│   ├── Customization/      # Multi-layer livery painter, bilingual LED boards, musical horns
│   ├── Missions/           # Career campaign, night sleeper runs, ghat challenges, fuel trials
│   ├── Progression/        # Driver XP curves, commercial license tiers, achievement registry
│   ├── SaveSystem/         # Atomic versioned JSON/binary serializer & schema migration engine
│   ├── Audio/              # Diesel engine DSP, retarder whine, pneumatic hisses, Indian horns
│   ├── UI/                 # Responsive PC/Mobile driving HUDs, fleet & financial management dashboards
│   ├── Input/              # Unified input controller (PC Wheel/Gamepad, Mobile Touch/Tilt)
│   ├── Localization/       # Multi-language string catalogs (EN, Telugu, Tamil, Kannada, Malayalam, Hindi)
│   ├── Analytics/          # Abstract gameplay telemetry & event dispatcher
│   ├── Store/              # Mock sandbox in-app store (Zero real credentials)
│   └── Debug/              # Telemetry overlay, cheat console, physics diagnostics, FPS profiler
├── Tests/
│   ├── EditMode/           # Core, math, vehicle physics, economy, route graph, save system tests
│   ├── PlayMode/           # Vehicle controller, traffic spawning, passenger flow, GPS navigation tests
│   └── Integration/        # End-to-end trip completion, tycoon progression, depot dispatch tests
├── Editor/                 # Route spline bakers, vehicle balance inspectors, localization tools
├── Tools/                  # LOC audit runner, build verification automation
Docs/
├── ARCHITECTURE.md         # Technical design & subsystem specifications
├── ROUTES_ATLAS.md         # Geographic corridor documentation
├── VEHICLE_MANUAL.md       # 18 bus specs and chassis handbook
├── TYCOON_GUIDE.md         # Financial economics & depot operations
├── LOC-AUDIT.md            # Verified CLOC audit log
└── THIRD_PARTY_LICENSES.md # Open-source license attribution
```

---

## Verification & Hard Acceptance Gates

- **LOC Verification Target**: $\ge 70,000$ verified genuine source LOC.
- **Audit Tool**: Run `python Assets/Tools/loc_audit.py` to inspect genuine lines of code across all subsystems excluding vendor/generated files.
- **Automated Tests**: Comprehensive EditMode, PlayMode, and Integration test suites covering all mathematical, physical, and economic logic.
