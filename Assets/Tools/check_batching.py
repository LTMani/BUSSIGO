from pathlib import Path

def check_dynamic_batching():
    ps = Path("ProjectSettings")
    for f in ps.glob("*.asset"):
        with open(f, "r", encoding="utf-8", errors="ignore") as file:
            content = file.read()
        if "batching" in content.lower():
            print(f"[{f.name}] contains batching:")
            for line in content.splitlines():
                if "batching" in line.lower():
                    print("  ", line)

if __name__ == '__main__':
    check_dynamic_batching()
