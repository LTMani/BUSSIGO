#!/usr/bin/env python3
"""
BUSSIGO - Automated Android APK Build Engine
Compiles and generates the installable Android Development APK (BUSSIGO_v1.0.0_Dev.apk)
"""

import os
import sys
import subprocess
from pathlib import Path

def find_unity_editor():
    candidates = [
        r"C:\Program Files\Unity\Hub\Editor\2022.3.48f1\Editor\Unity.exe",
        r"C:\Program Files\Unity\Hub\Editor\2022.3.20f1\Editor\Unity.exe",
        r"C:\Program Files\Unity\Hub\Editor\2023.2.20f1\Editor\Unity.exe",
        r"C:\Program Files\Unity\Hub\Editor\6000.0.32f1\Editor\Unity.exe",
        r"C:\Program Files\Unity\Editor\Unity.exe"
    ]
    for c in candidates:
        if os.path.exists(c):
            return c
    return None

def build_android_apk():
    print("=" * 75)
    print("   BUSSIGO - ANDROID APK BUILD ENGINE")
    print("=" * 75)

    project_dir = Path(__file__).resolve().parent.parent.parent
    build_output_dir = project_dir / "Build" / "Android"
    build_output_dir.mkdir(parents=True, exist_ok=True)
    target_apk = build_output_dir / "BUSSIGO_v1.0.0_Dev.apk"
    log_file = build_output_dir / "android_build.log"

    unity_exe = find_unity_editor()
    if not unity_exe:
        print("\n[INFO] Unity Editor executable was not found in standard Program Files paths.")
        print("To build the Android APK directly from Unity:")
        print("1. Open Unity Hub and load project: 'T:\\Git Project\\BUSSIGO'")
        print("2. In Unity menu bar, select: 'BUSSIGO' -> 'Android' -> 'Build Development APK'")
        print("   OR go to File -> Build Settings -> Switch Platform to Android -> Click 'Build'.")
        print(f"3. The resulting APK will be saved to: {target_apk}")
        return

    print(f"Found Unity Editor: {unity_exe}")
    print(f"Building Android APK to: {target_apk}")

    cmd = [
        unity_exe,
        "-batchmode",
        "-quit",
        "-projectPath", str(project_dir),
        "-executeMethod", "Bussigo.Editor.AndroidBuildScript.BuildAndroidDevelopmentAPK",
        "-logFile", str(log_file)
    ]

    print("Running Unity Android Build pipeline...")
    res = subprocess.run(cmd)
    if res.returncode == 0 and target_apk.exists():
        print(f"\n✓ Android APK Built Successfully: {target_apk} ({target_apk.stat().st_size / (1024*1024):.2f} MB)")
        print("To install on connected Android device via ADB:")
        print(f"   adb install -r \"{target_apk}\"")
    else:
        print(f"\n[!] Build pipeline finished. Detailed logs available at: {log_file}")

if __name__ == "__main__":
    build_android_apk()
