#!/usr/bin/env python3
"""
BUSSIGO - Automated Unity 6.5 WebGL Build & Validation Engine
Builds the WebGL desktop browser playable build to Build/WebGL/
"""

import os
import sys
import subprocess
from pathlib import Path

def find_unity_editor():
    candidates = [
        r"C:\Program Files\Unity\Hub\Editor\6000.5.10f1\Editor\Unity.exe",
        r"C:\Program Files\Unity\Hub\Editor\6000.0.32f1\Editor\Unity.exe"
    ]
    for c in candidates:
        if os.path.exists(c):
            return c
    return None

def build_webgl():
    print("=" * 75)
    print("   BUSSIGO - UNITY 6.5 WEBGL BUILD & COMPILATION ENGINE")
    print("=" * 75)

    project_dir = Path(__file__).resolve().parent.parent.parent
    build_output_dir = project_dir / "Build" / "WebGL"
    build_output_dir.mkdir(parents=True, exist_ok=True)
    log_file = build_output_dir / "unity_webgl_build.log"

    unity_exe = find_unity_editor()
    if not unity_exe:
        print("\n[ERROR] Unity 6.5 Editor executable was not found at C:\\Program Files\\Unity\\Hub\\Editor\\6000.5.10f1\\Editor\\Unity.exe")
        sys.exit(1)

    print(f"Found Unity 6.5 Editor: {unity_exe}")
    print(f"Building WebGL Project: {project_dir}")
    print(f"Output Directory: {build_output_dir}")
    print(f"Build Log Path: {log_file}")

    cmd = [
        unity_exe,
        "-batchmode",
        "-quit",
        "-projectPath", str(project_dir),
        "-executeMethod", "Bussigo.Editor.WebGLBuildScript.BuildWebGL",
        "-logFile", str(log_file)
    ]

    print("\n[RUNNING] Executing Unity WebGL compilation pipeline (this may take 2-4 minutes)...")
    res = subprocess.run(cmd)

    index_html = build_output_dir / "index.html"
    if index_html.exists():
        print(f"\n✓ SUCCESS! WebGL Build generated successfully at: {build_output_dir}")
        print(f"  Entry Point: {index_html}")
        return 0
    else:
        print(f"\n[!] WebGL Build completed with return code: {res.returncode}.")
        print(f"Check build log for details: {log_file}")
        return res.returncode

if __name__ == "__main__":
    sys.exit(build_webgl())
