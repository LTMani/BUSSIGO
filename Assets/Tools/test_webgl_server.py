#!/usr/bin/env python3
import urllib.request
import threading
import time
import sys
from pathlib import Path

# Add project root to path
project_dir = Path(__file__).resolve().parent.parent.parent
sys.path.insert(0, str(project_dir))

from Assets.Tools.serve_webgl import serve_webgl

def test_server():
    server_thread = threading.Thread(target=serve_webgl, args=(8080,), daemon=True)
    server_thread.start()
    time.sleep(1.2)

    try:
        req = urllib.request.urlopen("http://localhost:8080/index.html")
        print("=" * 60)
        print("   BUSSIGO WEBGL SERVER TEST RESULTS")
        print("=" * 60)
        print(f"HTTP Status:  {req.status} OK")
        print(f"Content-Type: {req.headers.get('Content-Type')}")
        print(f"COOP Header:  {req.headers.get('Cross-Origin-Opener-Policy')}")
        print(f"COEP Header:  {req.headers.get('Cross-Origin-Embedder-Policy')}")
        print("[PASS] Local WebGL HTTP Server Verified Successfully!")
        return 0
    except Exception as e:
        print(f"Server test failed: {e}")
        return 1

if __name__ == "__main__":
    sys.exit(test_server())
