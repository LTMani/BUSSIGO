#!/usr/bin/env python3
"""
BUSSIGO - Local WebGL HTTP Server Launcher (Multi-threaded)
Serves the compiled 3D WebGL game on http://localhost:8080.
"""

import os
import sys
from pathlib import Path
from http.server import ThreadingHTTPServer, SimpleHTTPRequestHandler

class WebGLHTTPRequestHandler(SimpleHTTPRequestHandler):
    def end_headers(self):
        self.send_header("Access-Control-Allow-Origin", "*")
        self.send_header("Cache-Control", "no-cache, no-store, must-revalidate")
        super().end_headers()

    def guess_type(self, path):
        if str(path).endswith(".wasm"):
            return "application/wasm"
        elif str(path).endswith(".data"):
            return "application/octet-stream"
        elif str(path).endswith(".js"):
            return "application/javascript"
        elif str(path).endswith(".json"):
            return "application/json"
        elif str(path).endswith(".html"):
            return "text/html"
        return super().guess_type(path)

def serve_webgl(port=8080):
    project_dir = Path(__file__).resolve().parent.parent.parent
    webgl_dir = project_dir / "Build" / "WebGL"

    if not webgl_dir.exists() or not (webgl_dir / "index.html").exists():
        print(f"[!] Warning: WebGL build not found at: {webgl_dir}")
        return

    os.chdir(str(webgl_dir))
    
    ThreadingHTTPServer.allow_reuse_address = True
    server_address = ("127.0.0.1", port)
    try:
        httpd = ThreadingHTTPServer(server_address, WebGLHTTPRequestHandler)
    except OSError:
        port = 8085
        server_address = ("127.0.0.1", port)
        httpd = ThreadingHTTPServer(server_address, WebGLHTTPRequestHandler)

    print("=" * 75)
    print("   BUSSIGO - LOCAL WEBGL HTTP SERVER RUNNING")
    print("=" * 75)
    print(f"Serving Directory: {webgl_dir}")
    print(f"Localhost URL:     http://localhost:{port}")
    print(f"Direct IP URL:     http://127.0.0.1:{port}")
    print("Status:            Ready for desktop browser connections")
    print("=" * 75)

    try:
        httpd.serve_forever()
    except KeyboardInterrupt:
        print("\nServer stopped.")

if __name__ == "__main__":
    port = 8080
    if len(sys.argv) > 1 and sys.argv[1].isdigit():
        port = int(sys.argv[1])
    serve_webgl(port)
