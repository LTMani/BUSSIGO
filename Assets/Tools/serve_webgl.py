#!/usr/bin/env python3
"""
BUSSIGO - Local WebGL HTTP Server Launcher (Multi-threaded)
Serves the compiled 3D WebGL game on http://localhost:8080 with proper WASM/JS MIME types.
"""

import os
import sys
import mimetypes
from pathlib import Path
from http.server import ThreadingHTTPServer, SimpleHTTPRequestHandler

class WebGLHTTPRequestHandler(SimpleHTTPRequestHandler):
    def end_headers(self):
        # Add Cross-Origin headers for modern browser WASM & WebAudio execution
        self.send_header("Cross-Origin-Opener-Policy", "same-origin")
        self.send_header("Cross-Origin-Embedder-Policy", "require-corp")
        self.send_header("Access-Control-Allow-Origin", "*")
        self.send_header("Cache-Control", "no-cache, no-store, must-revalidate")
        super().end_headers()

    def guess_type(self, path):
        if path.endswith(".wasm"):
            return "application/wasm"
        elif path.endswith(".data"):
            return "application/octet-stream"
        elif path.endswith(".js"):
            return "application/javascript"
        elif path.endswith(".json"):
            return "application/json"
        elif path.endswith(".html"):
            return "text/html"
        return super().guess_type(path)

    def log_message(self, format, *args):
        # Clean logging
        sys.stderr.write(f"[BUSSIGO WebGL Server] {self.address_string()} - {format%args}\n")

def serve_webgl(port=8080):
    project_dir = Path(__file__).resolve().parent.parent.parent
    webgl_dir = project_dir / "Build" / "WebGL"

    if not webgl_dir.exists() or not (webgl_dir / "index.html").exists():
        print("=" * 75)
        print("   BUSSIGO - LOCAL WEBGL HTTP SERVER")
        print("=" * 75)
        print(f"\n[!] Warning: WebGL build not found at: {webgl_dir}")
        return

    os.chdir(str(webgl_dir))
    
    server_address = ("127.0.0.1", port)
    try:
        httpd = ThreadingHTTPServer(server_address, WebGLHTTPRequestHandler)
    except OSError:
        port = 8081
        server_address = ("127.0.0.1", port)
        httpd = ThreadingHTTPServer(server_address, WebGLHTTPRequestHandler)

    print("=" * 75)
    print("   BUSSIGO - LOCAL WEBGL HTTP SERVER RUNNING")
    print("=" * 75)
    print(f"Serving Directory: {webgl_dir}")
    print(f"Localhost URL:     http://localhost:{port}")
    print("Status:            Ready for desktop browser connections")
    print("=" * 75)
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
