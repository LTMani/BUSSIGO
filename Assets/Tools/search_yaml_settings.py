import os
from pathlib import Path

def search_yaml_and_assets():
    keywords = ["TextureImporter", "m_TextureShape", "generateCubemap", "Cubemap", "_MainTex", "dynamicBatching", "DynamicBatching"]
    for root, dirs, files in os.walk("."):
        if any(ignored in root for ignored in [".git", "Library", "Temp", "obj"]):
            continue
        for file in files:
            ext = os.path.splitext(file)[1].lower()
            if ext in [".mat", ".asset", ".meta", ".prefab", ".unity", ".json"]:
                filepath = os.path.join(root, file)
                try:
                    with open(filepath, "r", encoding="utf-8", errors="ignore") as f:
                        lines = f.readlines()
                    for i, line in enumerate(lines, 1):
                        for kw in ["m_TextureShape", "generateCubemap", "m_DynamicBatching", "dynamicBatching"]:
                            if kw.lower() in line.lower():
                                print(f"{filepath}:{i} -> {line.strip()}")
                except Exception:
                    pass

if __name__ == '__main__':
    search_yaml_and_assets()
