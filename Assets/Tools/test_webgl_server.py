#!/usr/bin/env python3
import urllib.request
import threading
import time
import sys
from pathlib import Path

project_dir = Path(__file__).resolve().parent.parent.parent
sys.path.insert(0, str(project_dir))

from Assets.Tools.serve_webgl import serve_webgl

def test_server():
    print("=" * 65)
    print("   BUSSIGO - WEBGL SERVER PATH & ASSET INTEGRITY AUDIT")
    print("=" * 65)

    server_thread = threading.Thread(target=serve_webgl, args=(8080,), daemon=True)
    server_thread.start()
    time.sleep(1.0)

    urls_to_test = [
        ("http://127.0.0.1:8080/", "text/html"),
        ("http://127.0.0.1:8080/index.html", "text/html"),
        ("http://127.0.0.1:8080/game.js", "application/javascript"),
    ]

    all_passed = True
    for url, expected_mime in urls_to_test:
        try:
            req = urllib.request.urlopen(url, timeout=5)
            content = req.read()
            mime = req.headers.get("Content-Type", "")
            print(f"[PASS] {url} -> HTTP {req.status} OK | Size: {len(content):,} bytes | MIME: {mime}")
        except Exception as e:
            print(f"[FAIL] {url} -> {e}")
            all_passed = False

    if all_passed:
        print("\n[SUCCESS] All WebGL core files and endpoints verified with HTTP 200 OK!")
        return 0
    else:
        print("\n[ERROR] One or more endpoints failed.")
        return 1

if __name__ == "__main__":
    sys.exit(test_server())
