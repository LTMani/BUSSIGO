from pathlib import Path

def inspect_project_settings():
    ps_dir = Path("ProjectSettings")
    for f in ps_dir.iterdir():
        if f.is_file():
            print(f"File: {f.name} ({f.stat().st_size} bytes)")
            with open(f, "r", encoding="utf-8", errors="ignore") as file:
                lines = file.readlines()
            for i, line in enumerate(lines, 1):
                if any(k in line.lower() for k in ["batching", "texture", "importer", "material", "model", "cube", "shader"]):
                    print(f"  Line {i}: {line.strip()}")

if __name__ == '__main__':
    inspect_project_settings()
