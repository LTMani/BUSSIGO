# BUSSIGO: Technical System Architecture Specification

## Overview
**BUSSIGO** (South India Bus & Travel Empire Simulator) is built with a modular, high-performance architectural design in Unity (C#). The codebase is partitioned across 22 decoupled subsystems with zero circular dependencies.

---

## High-Level Subsystem Breakdown

```
                                  ┌───────────────────────────────┐
                                  │       Bussigo.Game.Core       │
                                  │ (Math, Spatial, DI, EventBus) │
                                  └───────────────┬───────────────┘
                                                  │
         ┌─────────────────────────┬──────────────┴──────────────┬─────────────────────────┐
         ▼                         ▼                             ▼                         ▼
┌──────────────────┐     ┌──────────────────┐          ┌──────────────────┐      ┌──────────────────┐
│ Vehicles &       │     │ Routes &         │          │ Traffic AI &     │      │ Tycoon Economy & │
│ VehiclePhysics   │     │ Navigation Graph │          │ Passenger Sim    │      │ Company Depots   │
└────────┬─────────┘     └────────┬─────────┘          └────────┬─────────┘      └────────┬─────────┘
         │                        │                             │                         │
         └────────────────────────┼─────────────────────────────┼─────────────────────────┘
                                  │
                                  ▼
                    ┌───────────────────────────┐
                    │ UI, Input & Customization │
                    │ (PC + Mobile ViewModels)  │
                    └───────────────────────────┘
```

---

## Subsystem Details

### 1. `Bussigo.Game.Core`
- **Fast Spatial Math & Numerical Solvers**: `CoreMath`, `Vector2D`, `Vector3D`, `Matrix4x4D`, `SplineMath` (Catmull-Rom & Bezier), `KalmanFilter1D`, `GeoMath` (WGS84 Haversine & Geodetic projection).
- **Service Locator & Dependency Injection**: High-throughput thread-safe dependency injection and service locator.
- **Event Bus & State Machine**: Decoupled publisher-subscriber event bus with typed events and finite state machines.
- **Object Pooling & Spatial Partitioning**: Generic object pools and `QuadTree2D`/`SpatialGrid2D` spatial indexing.

### 2. `Bussigo.Game.Vehicles` & `Bussigo.Game.VehiclePhysics`
- **Pacejka 94 Magic Formula**: Lateral and longitudinal tyre friction curves with slip-angle and slip-ratio solvers.
- **Pneumatic Air-Brake System**: Dual primary/secondary air reservoir tanks, compressor governor cut-in/cut-out, treadle foot valve dynamics, spring-brake emergency locks, and ABS modulators.
- **Diesel Powertrain & Retarder**: BSFC (Brake-Specific Fuel Consumption) fuel maps, multi-cylinder torque plateau curves, turbocharger boost lag, hydrodynamic retarders, and multi-speed manual/AMT transmissions.
- **Chassis & Suspension Kinematics**: Dynamic axle load transfer, anti-roll bars, body pitch and roll angles, jounce and rebound progressive bumpers, and Ackermann speed-sensitive steering.
- **Electrical & Thermal Subsystems**: 24V commercial bus electrical network, alternator charging curves, engine block coolant/oil thermodynamics, and cabin HVAC air conditioning.

### 3. `Bussigo.Game.Routes` & `Bussigo.Game.Navigation`
- **South India Corridor Atlas**: Authentic highways across Andhra Pradesh and Telangana (NH65 Vijayawada-Hyderabad, NH16 Vijayawada-Guntur-Visakhapatnam, NH44 Hyderabad-Kurnool-Anantapur, NH163 Hyderabad-Warangal, Eastern Ghats Srisailam hairpins).
- **Directed Road Graph & A\* Pathfinder**: Hierarchical A\* pathfinder with turn penalties and traffic congestion weights.
- **Turn-by-Turn GPS Guidance**: Real-time maneuver detection with bilingual voice prompt catalogs in Telugu and English.

### 4. `Bussigo.Game.Traffic` & `Bussigo.Game.Passengers`
- **IDM & MOBIL Highway Simulation**: Intelligent Driver Model car-following acceleration with MOBIL lane changing.
- **Indian Highway Behaviors**: Heavy cargo lorries, state RTC buses, auto-rickshaws, two-wheelers, and 108 emergency ambulances.
- **Passenger Crowd & Satisfaction Utility**: Thermal comfort, driving smoothness, punctuality, and cleanliness scoring.

### 5. `Bussigo.Game.Economy`, `Bussigo.Game.Company` & `Bussigo.Game.Fleet`
- **Double-Entry Financial Ledger**: 50 general ledger accounts, P&L statements, balance sheets, and cash flow tracking.
- **Dynamic Pricing & Festival Surges**: Sankranti harvest surge (300% demand), Dasara, Diwali, and pilgrimage seasonal surges.
- **Depot Network & Staff Management**: Depots across AP/Telangana, workshop upgrade bays, driver skill trees, and fatigue shift rostering.
- **18 Bus Model Archetypes**: Pallevelugu rural standard, City commuter Mitra, Express 3+2, Ultra Deluxe, Super Luxury, Garuda AC, Garuda Plus Multi-Axle, Amaravati Scania, Vennela Sleeper, and Night Rider Luxury.

### 6. `Bussigo.Game.Weather`, `Bussigo.Game.Audio`, `Bussigo.Game.UI` & `Bussigo.Game.Input`
- **24-Hour Solar & Tropical Monsoon**: Astronomical solar position solvers for 16.5° N latitude, dynamic rainfall accumulation, and puddle hydroplaning.
- **Procedural Engine DSP**: Cylinder firing harmonic pitch shifters, turbo spool filters, air-brake purge hisses, and Telugu/English terminal announcements.
- **Unified PC & Mobile Input**: PC keyboard, gamepad, 900° force feedback wheel with H-shifter, and mobile touch/tilt virtual steering.
- **Responsive ViewModels**: 50+ ViewModels driving in-game HUD gauges, fleet management dashboards, and garage inspectors.
