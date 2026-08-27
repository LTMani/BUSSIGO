import os
from pathlib import Path

def find_asset_processors():
    root = Path("Assets")
    keywords = [
        "AssetPostprocessor",
        "OnPostprocessAllAssets",
        "OnPreprocessAsset",
        "OnPostprocessModel",
        "OnPostprocessTexture",
        "TextureImporter",
        "TextureImporterShape",
        "generateCubemap",
        "ImportAsset",
        "SaveAssets",
        "Refresh",
        "_MainTex",
        "Cubemap",
        "TextureShape"
    ]
    
    matches = {}
    for cs_file in root.rglob("*.cs"):
        try:
            with open(cs_file, "r", encoding="utf-8", errors="ignore") as f:
                content = f.read()
                lines = content.splitlines()
                for i, line in enumerate(lines, 1):
                    for kw in keywords:
                        if kw.lower() in line.lower():
                            if cs_file not in matches:
                                matches[cs_file] = []
                            matches[cs_file].append((i, kw, line.strip()))
        except Exception as e:
            pass

    for file, hits in matches.items():
        print(f"\nFile: {file}")
        for line_no, kw, line in hits:
            print(f"  Line {line_no} [{kw}]: {line}")

if __name__ == '__main__':
    find_asset_processors()
