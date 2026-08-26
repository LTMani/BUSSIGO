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

---

## Dependencies

The project utilizes standardized dependency manifests and lockfiles:

- **Python Dependencies**: Listed in [`requirements.txt`](file:///requirements.txt) with exact versions pinned in [`requirements.lock`](file:///requirements.lock).
- **Node/JavaScript Dependencies**: Listed in [`package.json`](file:///package.json) with exact versions pinned in [`package-lock.json`](file:///package-lock.json).
- **Unity C# Engine**: Configured in `ProjectSettings/ProjectVersion.txt` (Unity 2022.3 LTS / Unity 6 compatible) and `Packages/manifest.json`.

---

## Installation

### 1. Python Environment Setup
```bash
python -m venv venv
# Windows:
.\venv\Scripts\activate
# Linux/macOS:
source venv/bin/activate

pip install -r requirements.txt
```

### 2. Node & WebGL Environment Setup
```bash
npm install
```

### 3. Docker Installation (Optional)
```bash
docker build -t bussigo:latest .
```

---

## Build

To compile and verify the simulation build:

### Build WebGL Simulator
```bash
# Using Python build script:
python Assets/Tools/build_webgl_local.py

# Using npm:
npm run build

# Using Makefile:
make build
```

---

## Run

To launch the BUSSIGO 3D Simulation engine and local server:

### 1. Direct Python Launcher
```bash
python main.py 8080
# Or:
python app.py
```

### 2. Using npm
```bash
npm start
```

### 3. Using Makefile
```bash
make run
```

### 4. Using Docker
```bash
docker run -p 8080:8080 bussigo:latest
```

Once running, open your browser and navigate to:
👉 **`http://localhost:8080`**

---

## Usage

### Driving Controls (PC Keyboard)
- **`W` / `Up Arrow`**: Smooth Throttle / Accelerator
- **`S` / `Down Arrow`**: Service Pneumatic Air Brake & Reverse
- **`A` / `D`**: Continuous Smooth Steering with speed-dependent resistance
- **`E`**: Glider Passenger Doors (Open at PNBS/MGBS platforms to board/alight)
- **`H`**: Authentic Melodic Multi-Tone South Indian Air Horn
- **`C`**: Cycle Camera Perspectives (Chase, Cockpit Interior, Driver's Eye, Cabin Window, Bumper, Reverse, Mirror, Fly-by)
- **`R`**: 4-Stage Hydrodynamic Retarder Downhill Brake
- **`L`**: Headlights High/Low Beams
- **`Space`**: Emergency Air Parking Brake

### Gameplay Flow
1. Launch the game and click **`▶ 1. DRIVE: VIJAYAWADA ➔ HYDERABAD (NH65)`**.
2. At **Vijayawada PNBS Platform 4**, press **`E`** to open doors and board 45 passengers.
3. Close doors with **`E`**, accelerate onto NH65 highway, and follow the GPS minimap radar.
4. Pass through **Kanchikacherla FASTag Toll Plaza** (auto RFID payment ₹135 and barrier lift).
5. Arrive at **Hyderabad MGBS Platform 12**, open doors to drop off passengers, and collect your **+₹38,250** fare earnings and **+850 Driver XP**!

---

## Testing

Execute the automated test suites to verify spatial math, vehicle powertrain, Pacejka tyre friction, route graph connectivity, traffic solvers, and double-entry ledger economics:

```bash
# Run all subsystem test suites:
python Assets/Tools/test_runner.py

# Run LOC audit:
python Assets/Tools/loc_audit.py

# Run WebGL server asset audit:
python Assets/Tools/test_webgl_server.py
```

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
│   ├── Audio/              # Diesel engine DSP, retarder whine, pneumatic hisses, Indian horns
│   ├── UI/                 # Responsive PC/Mobile driving HUDs, fleet & financial management dashboards
│   └── Runtime3D/          # 3D Vehicle meshes, procedural textures, scene builder, camera system
├── Tests/
│   ├── EditMode/           # Core, math, vehicle physics, economy, route graph, save system tests
│   ├── PlayMode/           # Vehicle controller, traffic spawning, passenger flow, GPS navigation tests
│   └── Integration/        # End-to-end trip completion, tycoon progression, depot dispatch tests
├── Editor/                 # Route spline bakers, vehicle balance inspectors, localization tools
├── Tools/                  # LOC audit runner, build verification automation, local server
Docs/
├── ARCHITECTURE.md         # Technical design & subsystem specifications
├── ROUTES_ATLAS.md         # Geographic corridor documentation
├── VEHICLE_MANUAL.md       # 18 bus specs and chassis handbook
├── TYCOON_GUIDE.md         # Financial economics & depot operations
├── LOC-AUDIT.md            # Verified CLOC audit log
└── THIRD_PARTY_LICENSES.md # Open-source license attribution
```
