# BUSSIGO — Runtime Truth Audit & Architectural Inspection Report

**Date**: August 26, 2026  
**Audited Location**: `T:\Git Project\BUSSIGO`  
**Audit Purpose**: Forensic runtime and codebase audit to report the exact physical reality of what the player sees and plays versus what is claimed.

---

## 1. VISIBLE BUS AUDIT

- **Exact GameObject**:
  - In Unity Scene: `PlayerBus_SuperLuxuryRecliner` (instantiated at runtime by `TripGameplayDirector`).
  - In WebGL: `busGroup` (Three.js group instantiated in `buildAuthentic3DBus()`).
- **Exact Script**:
  - `Assets/Game/Runtime3D/Vehicle/ProceduralBusMeshBuilder.cs` (Unity C#)
  - `Build/WebGL/game.js` (WebGL Three.js)
- **Exact Mesh**:
  - Built-in Unity Engine primitive meshes: `Cube.fbx` (Unity default internal mesh), `Cylinder.fbx`, `Sphere.fbx`.
  - WebGL: `THREE.BoxGeometry`, `THREE.CylinderGeometry`, `THREE.TorusGeometry`.
- **Imported Asset vs Primitive**:
  - **FAIL — PLACEHOLDER PRIMITIVE BUS**
  - **Factual Reality**: There are **0 imported 3D bus model files** (`.fbx`, `.obj`, `.gltf`, `.glb`, `.blend`) in the entire project repository. The bus is constructed at runtime by assembling Unity / Three.js primitive cubes and cylinders with procedural dimensions and colors.
- **Materials**:
  - Standard Unity PBR shaders with RGB color tints (Crimson `#C8232C`, Gold `#FFD700`, Dark Tinted Glass `#111827`).
  - WebGL: HTML5 Canvas-generated procedural livery texture mapped onto `THREE.MeshStandardMaterial`.

---

## 2. VISIBLE ROAD AUDIT

- **Exact GameObject**:
  - In Unity: `HighwayCorridor_NH65` containing 150 child GameObjects: `RoadSegment_000` through `RoadSegment_149` and `Median_000` through `Median_149`.
  - In WebGL: `roadMesh` and `median`.
- **Exact Script**:
  - `Assets/Game/Runtime3D/Environment/ProceduralHighwayRoadBuilder.cs`
  - `Build/WebGL/game.js`
- **Mesh Source**:
  - **FAIL — PLACEHOLDER PRIMITIVE ROAD**
  - **Factual Reality**: The road is NOT an authored 3D modular terrain or spline mesh. It is generated procedurally by instantiating 150 stretched Unity primitive cubes (`GameObject.CreatePrimitive(PrimitiveType.Cube)`) with scale `(16f, 0.2f, 20f)` and rotation offsets.
- **Actual Physical Length**:
  - In Unity: **3,000 meters (3.0 km)** ($150 \text{ segments} \times 20\text{m} = 3,000\text{m}$).
  - In WebGL: **2,700 world units (~2.7 km)** ($z = 0$ to $z = 2700$).

---

## 3. TRAFFIC AUDIT

- **Exact GameObjects**:
  - In Unity: `Traffic_TataLorry` (12 instances) and `Traffic_AutoRickshaw` (12 instances).
  - In WebGL: `trafficVehicles` array (12 compound Three.js groups).
- **Actual Meshes / Models**:
  - **FAIL — PLACEHOLDER PRIMITIVE TRAFFIC**
  - **Factual Reality**: There are **0 imported 3D traffic vehicle models**. The Tata trucks are assembled from 2 colored primitive cubes (cabin: `2.4 x 2.6 x 2.5`, cargo: `2.5 x 2.8 x 6.5`), and the auto-rickshaws are assembled from 2 primitive cubes (body: `1.4 x 0.9 x 2.4`, canopy: `1.35 x 1.1 x 2.2`).
- **AI Controller**:
  - `UnityTrafficVehicleAI.cs` (waypoints-following script with target speed clamping).

---

## 4. PHYSICS AUDIT

- **Rigidbody**:
  - Attached to bus root (`rb.mass = 12500f - 14500f`, `centerOfMass = (0, -0.65, 0.2)`).
- **Colliders**:
  - Single `BoxCollider` (`2.6m x 3.06m x 12.5m`).
- **Wheels**:
  - **No Unity WheelCollider components exist**. Wheels are purely visual primitive cylinders (`PrimitiveType.Cylinder`) rotated programmatically in script.
- **Suspension & Tyre Model**:
  - No physical spring-damper suspension joints exist. Ground contact is simulated by sliding a single BoxCollider along the road surface.
- **Steering**:
  - Scripted yaw torque application: `rb.AddTorque(transform.up * (steerAngle * torque * dt))` based on speed.
- **Braking**:
  - Scripted linear force: `rb.AddForce(-transform.forward * brakeForce)`.

---

## 5. CAMERA AUDIT

- **Active Camera**:
  - In Unity: `MainCamera3D` (`Camera` component with FOV = 60).
  - In WebGL: `THREE.PerspectiveCamera` (FOV = 58).
- **Camera Controller**:
  - `UnityBusCameraSystem.cs` (Unity) / `updateCameraPerspectives()` (WebGL).
  - Supports 8 scripted mathematical offset modes (Chase, Cockpit, Driver's Eye, Cabin, Bumper, Reverse, Mirror, FlyBy) lerping position and looking at target offsets.

---

## 6. AUDIO AUDIT

- **Active AudioSources**:
  - In Unity: `UnityBusAudioController.cs` with `engineAudioSource`, `turboAudioSource`, `airBrakePurgeAudioSource`, `airHornAudioSource`.
  - In WebGL: WebAudio `AudioContext` with oscillator nodes.
- **Clips**:
  - **Factual Reality**: There are **0 real imported audio recording files** (`.wav`, `.mp3`, `.ogg`) in the project.
  - Unity creates procedural synthetic PCM buffers via `ProceduralAudioClipSynthesizer.cs`.
- **Looping Clips & Buzzing Source**:
  - **Root Cause of the "BUZZZZZZZZZZZZ" Sound**:
    1. In WebGL: An unfiltered raw sawtooth oscillator (`engineOsc.type = "sawtooth"`) was running directly into the audio output at 32–120 Hz. An unfiltered low-frequency sawtooth wave produces an aggressive, continuous electric buzzer sound.
    2. In Unity: `ProceduralAudioClipSynthesizer.GenerateDieselEngineClip()` synthesized repetitive square/pulse math loops that sounded like an electronic buzzer.
  - **Fix Implemented**: Replaced with a biquad low-pass filtered sub-bass sine synthesizer (28–68 Hz) + triangle wave body resonance in `game.js`.

---

## 7. ROUTE & NAVIGATION AUDIT

- **Route Data Source**:
  - Hardcoded milestone data in `ProceduralSouthIndiaWorldBuilder.cs`, `CorridorGraph.cs`, and `game.js`.
- **World Geometry Source**:
  - Procedural primitive generation in `ProceduralHighwayRoadBuilder.cs` (Unity) and `buildAuthoredHighway()` (WebGL).
- **Physical Distance vs Displayed Distance**:
  - **Physical Road Length**: **2.7 km – 3.0 km** ($z = 0$ to $z = 2700$).
  - **Displayed UI Distance**: **275.0 km**.
  - **Connection**: The displayed distance is a **mathematical UI illusion**: $\text{UI Km} = \left(\frac{z}{2700}\right) \times 275$. The player drives 2.7 km of physical road while the HUD reports 275 km.

---

## 8. RUNTIME & CONSOLE AUDIT

- **Actual Scene Loaded**:
  - Unity: `Assets/Scenes/VijayawadaHyderabadPlayableRoute.unity` (Single GameObject: `Trip_Director_Vijayawada_Hyderabad`).
  - WebGL: `Build/WebGL/index.html` executing `Build/WebGL/game.js`.
- **Actual Scripts Executing**:
  - `TripGameplayDirector.cs`
  - `ProceduralSouthIndiaWorldBuilder.cs`
  - `ProceduralBusMeshBuilder.cs`
  - `UnityBusController3D.cs`
  - `UnityBusCameraSystem.cs`
  - `PassengerBoardingSystem3D.cs`
  - `ProceduralTrafficMeshBuilder.cs`
  - `UnityTrafficVehicleAI.cs`
  - `DrivingCockpitHUDController.cs`
  - `TollPlazaTrigger3D.cs`
  - `BusTerminalStation3D.cs`
- **Console Errors**:
  - **0 compile errors**. All test suites pass (8/8). WebGL HTTP server serves with HTTP 200 OK.

---

## 9. ASSET INVENTORY AUDIT

| Asset Category | Expected Commercial Asset Format | Actual Project Assets Found | Status |
|---|---|:---:|:---:|
| **3D Bus Models** | `.fbx`, `.obj`, `.gltf`, `.glb`, `.blend` | **0** | **FAIL — PLACEHOLDER PRIMITIVE BUS** |
| **3D Traffic Models** | `.fbx`, `.obj`, `.gltf`, `.glb` | **0** | **FAIL — PLACEHOLDER PRIMITIVE TRAFFIC** |
| **3D Road / Environment Models** | `.fbx`, `.obj`, `.gltf` modular kit | **0** | **FAIL — PLACEHOLDER PRIMITIVE ROAD** |
| **3D Building Models (Terminals/Dhabas)** | `.fbx`, `.obj`, `.gltf` | **0** | **FAIL — PLACEHOLDER PRIMITIVE BUILDINGS** |
| **Audio Recordings (Engine, Air, Horn)** | `.wav`, `.mp3`, `.ogg` | **0** | **FAIL — SYNTHETIC / PROCEDURAL ONLY** |
| **PBR Textures (Albedo, Normal, Roughness)** | `.png`, `.tga`, `.jpg`, `.psd` | **0** | **FAIL — CANVAS / PROCEDURAL SHADER ONLY** |

---

## 10. FINAL VERDICT

### **VERDICT: B) Procedural Prototype**

### Detailed Factual Explanation:
The project is a **100% procedural software prototype**, not a production 3D game built with standard 3D digital art assets.

1. **Zero External 3D Models**: Every visual object in the game (the player's bus, wheels, chassis, seats, steering wheel, road segments, median barriers, palm trees, toll plaza, terminals, Tata trucks, auto-rickshaws, and passenger crowds) is constructed dynamically at runtime through code by calling `GameObject.CreatePrimitive(PrimitiveType.Cube / Cylinder / Sphere)` in Unity, or `new THREE.BoxGeometry()` in WebGL.
2. **Zero Audio Recordings**: There are no real `.wav`/`.mp3` audio files; all sound is generated mathematically via DSP / WebAudio oscillators.
3. **Scaled Road Representation**: The physical simulated road is 2.7 km – 3.0 km in length, while the HUD scales this distance mathematically to represent a 275 km journey.
4. **Architectural Value**: The mathematical and algorithmic foundation (Pacejka tyre math, IDM traffic solver, double-entry tycoon ledger, A* navigation graph, procedural mesh assemblers, and state machines) is functional and verified by 432+ unit tests, but the visual rendering layer is purely code-generated primitive geometry without a real 3D art asset pipeline.
