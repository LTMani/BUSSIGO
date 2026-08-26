#!/usr/bin/env python3
"""
BUSSIGO - Local WebGL HTTP Server Launcher
Serves the compiled WebGL game on http://localhost:8080 with proper WASM MIME types.
"""

import os
import sys
import mimetypes
from pathlib import Path
from http.server import HTTPServer, SimpleHTTPRequestHandler

class WebGLHTTPRequestHandler(SimpleHTTPRequestHandler):
    def end_headers(self):
        # Add Cross-Origin headers for modern browser WASM execution
        self.send_header("Cross-Origin-Opener-Policy", "same-origin")
        self.send_header("Cross-Origin-Embedder-Policy", "require-corp")
        self.send_header("Access-Control-Allow-Origin", "*")
        super().end_headers()

    def guess_type(self, path):
        # Ensure correct MIME types for WebGL binary assets
        if path.endswith(".wasm"):
            return "application/wasm"
        elif path.endswith(".data"):
            return "application/octet-stream"
        elif path.endswith(".js"):
            return "application/javascript"
        elif path.endswith(".json"):
            return "application/json"
        return super().guess_type(path)

def serve_webgl(port=8080):
    project_dir = Path(__file__).resolve().parent.parent.parent
    webgl_dir = project_dir / "Build" / "WebGL"

    if not webgl_dir.exists() or not (webgl_dir / "index.html").exists():
        print("=" * 75)
        print("   BUSSIGO - LOCAL WEBGL HTTP SERVER")
        print("=" * 75)
        print(f"\n[!] Warning: WebGL build not found at: {webgl_dir}")
        print("Run the build script first:")
        print("   python Assets/Tools/build_webgl_local.py")
        print("\nOr build inside Unity Editor via:")
        print("   BUSSIGO -> WebGL -> Build WebGL Local Playable")
        return

    os.chdir(str(webgl_dir))
    
    server_address = ("", port)
    try:
        httpd = HTTPServer(server_address, WebGLHTTPRequestHandler)
    except OSError:
        port = 8081
        server_address = ("", port)
        httpd = HTTPServer(server_address, WebGLHTTPRequestHandler)

    print("=" * 75)
    print("   BUSSIGO - LOCAL WEBGL HTTP SERVER RUNNING")
    print("=" * 75)
    print(f"\nServing directory: {webgl_dir}")
    print(f"Localhost URL:     http://localhost:{port}")
    print("\nPress Ctrl+C to stop the server.\n")

    try:
        httpd.serve_forever()
    except KeyboardInterrupt:
        print("\nServer stopped.")

if __name__ == "__main__":
    port = 8080
    if len(sys.argv) > 1 and sys.argv[1].isdigit():
        port = int(sys.argv[1])
    serve_webgl(port)
