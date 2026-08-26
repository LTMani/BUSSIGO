#!/usr/bin/env python3
"""
BUSSIGO - Automated Standalone Windows Executable (.exe) Build Script
Builds the standalone 3D playable game using Unity Editor command-line batchmode.
"""

import os
import sys
import subprocess
from pathlib import Path

def find_unity_editor():
    # Common standard Unity Editor installation paths on Windows
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

def build_standalone():
    print("=" * 70)
    print("   BUSSIGO - STANDALONE 3D EXECUTABLE BUILD HELPER")
    print("=" * 70)

    project_dir = Path(__file__).resolve().parent.parent.parent
    build_output_dir = project_dir / "Build"
    build_output_dir.mkdir(parents=True, exist_ok=True)
    build_target_exe = build_output_dir / "BUSSIGO_SouthIndiaSimulator.exe"

    unity_exe = find_unity_editor()
    if not unity_exe:
        print("\n[INFO] Unity Editor executable was not found at standard Program Files paths.")
        print("To build a standalone Windows .exe, you can:")
        print("1. Open Unity Hub and load 't:\\Git Project\\BUSSIGO'.")
        print("2. In Unity, go to: File -> Build Settings -> Select PC, Mac & Linux Standalone -> Click 'Build'.")
        return

    print(f"Found Unity Editor: {unity_exe}")
    print(f"Building project to: {build_target_exe}")

    cmd = [
        unity_exe,
        "-batchmode",
        "-quit",
        "-projectPath", str(project_dir),
        "-buildWindows64Player", str(build_target_exe),
        "-logFile", str(build_output_dir / "build.log")
    ]

    print("Executing Unity batch build...")
    res = subprocess.run(cmd)
    if res.returncode == 0:
        print(f"\n✓ Standalone 3D Game Built Successfully at: {build_target_exe}")
    else:
        print(f"\n[!] Build completed with return code: {res.returncode}. Check Build/build.log for details.")

if __name__ == "__main__":
    build_standalone()
