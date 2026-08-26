# BUSSIGO: Android Mobile Build & Physical Device Testing Guide

## 1. Android Application Specifications
- **Product Name**: `BUSSIGO - South India Bus & Travel Empire Simulator`
- **Application ID / Package Name**: `com.bussigo.southindiatravels`
- **Version**: `1.0.0` (Bundle Version Code: `1`)
- **Supported Architecture**: `ARM64 (64-bit)` and `ARMv7 (32-bit)`
- **Minimum OS Version**: Android 7.0 Nougat (API Level 24)
- **Target OS Version**: Android 14.0 (API Level 34)
- **Screen Orientation**: Fixed Landscape (`LandscapeLeft` / `LandscapeRight` auto-rotation)
- **Rendering API**: OpenGLES 3.0 / Vulkan with mobile GPU instancing and dynamic batching

---

## 2. Touchscreen Control Architecture (`MobileTouchInputController.cs`)

The game features an integrated, responsive on-screen mobile control canvas that automatically activates on Android devices:

```
┌────────────────────────────────────────────────────────────────────────┐
│ [GPS Minimap Radar]                                      [Gear Shifter]│
│                                                              ▲ SHIFT UP│
│                                                            [ D / N / R]│
│                                                            ▼ SHIFT DN  │
│                                                                        │
│                                                                        │
│                                                               [ACCEL]  │
│  [◀ STEER]    [STEER ▶]                                        ▲ GAS   │
│   (Left)       (Right)                                                 │
│                                                               [BRAKE]  │
│          [HORN]   [DOORS]   [CAMERA]   [LIGHTS]   [RETARDER]   ▼ STOP  │
└────────────────────────────────────────────────────────────────────────┘
```

### Controls Mapping:
- **Steering**: Left button (`◀ STEER`) and Right button (`STEER ▶`) with smooth steering wheel interpolation and spring return-to-center.
- **Acceleration**: Large responsive throttle pedal (`▲ ACCEL`).
- **Braking & Reverse**: Progressive service brake pedal (`▼ BRAKE`); holds brakes and shifts to reverse when stationary.
- **Gear Shifter**: Tap `SHIFT ▲` / `SHIFT ▼` to change transmission gears.
- **Quick Action Toolbar**:
  - `HORN`: Melodic South Indian pressure air horn.
  - `DOORS`: Pneumatic glider passenger doors (operates safely when bus is stopped at terminal platforms).
  - `CAMERA`: Cycles between 3rd-person chase, 1st-person cockpit interior, front bumper cam, and passenger cabin view.
  - `LIGHTS`: Headlights low/high beam toggle.
  - `RETARDER`: 4-stage hydrodynamic auxiliary brake lever.
  - `PARK`: Spring emergency handbrake.

---

## 3. How to Build the Android Development APK

### Method A: One-Click Build Inside Unity Editor
1. Open **Unity Hub** and load the project: `T:\Git Project\BUSSIGO`.
2. In the top Unity menu bar, click:
   ```text
   BUSSIGO ➔ Android ➔ Build Development APK
   ```
3. Unity will compile all C# scripts, package the 3D assets, and generate:
   ```text
   T:\Git Project\BUSSIGO\Build\Android\BUSSIGO_v1.0.0_Dev.apk
   ```

### Method B: Automated Command Line (Batchmode)
Run the automated build script in PowerShell:
```powershell
python Assets/Tools/build_android_apk.py
```

---

## 4. How to Install and Play on a Physical Android Phone

### Step 1: Transfer the APK to Your Phone
- **Option 1 (Fastest via USB / ADB)**:
  Connect your Android phone via USB with USB Debugging enabled, then run:
  ```powershell
  adb install -r "Build\Android\BUSSIGO_v1.0.0_Dev.apk"
  ```
- **Option 2 (Direct File Transfer)**:
  Connect your phone via USB in File Transfer mode and copy `BUSSIGO_v1.0.0_Dev.apk` to your phone's `Downloads` folder.
- **Option 3 (Cloud / Drive)**:
  Upload `BUSSIGO_v1.0.0_Dev.apk` to Google Drive or local file share and download it directly on your Android device.

### Step 2: Install and Launch
1. On your phone, tap `BUSSIGO_v1.0.0_Dev.apk` in your File Manager.
2. If prompted, select **Allow from this source** (Enable installing unknown apps).
3. Tap **Install** and then **Open**.

### Step 3: Play the Vijayawada ➔ Hyderabad Route
1. On the Main Menu, tap **1. DRIVE: VIJAYAWADA ➔ HYDERABAD (NH65)**.
2. At **Vijayawada PNBS Platform 4**, tap **DOOR** to open the glider doors and board 45 passengers.
3. Tap **DOOR** to close, press **ACCEL** to drive out of the terminal onto NH65.
4. Steer with on-screen buttons, overtake traffic (Tata trucks and autos), and pass through the **Kanchikacherla FASTag Toll Plaza**.
5. Pull into **Hyderabad MGBS**, open doors to drop off passengers, and collect your fare revenue and XP rewards!
