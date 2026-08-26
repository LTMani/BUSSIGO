#!/usr/bin/env python3
"""
BUSSIGO: South India Bus & Travel Empire Simulator
Main Executable Entry Point
"""

import sys
import os
from pathlib import Path

def main():
    print("=" * 75)
    print("   BUSSIGO V2 — SOUTH INDIA BUS & TRAVEL EMPIRE SIMULATOR")
    print("=" * 75)
    print("Starting BUSSIGO 3D WebGL Server & Simulation Engine...")

    project_root = Path(__file__).resolve().parent
    serve_script = project_root / "Assets" / "Tools" / "serve_webgl.py"

    if not serve_script.exists():
        print(f"Error: Server script not found at {serve_script}")
        sys.exit(1)

    port = 8080
    if len(sys.argv) > 1 and sys.argv[1].isdigit():
        port = int(sys.argv[1])

    # Import and run server
    sys.path.insert(0, str(project_root / "Assets" / "Tools"))
    from serve_webgl import serve_webgl
    serve_webgl(port)

if __name__ == "__main__":
    main()
