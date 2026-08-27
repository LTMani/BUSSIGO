from pathlib import Path

def check_all_metas():
    root = Path("Assets")
    for meta in root.rglob("*.meta"):
        with open(meta, "r", encoding="utf-8", errors="ignore") as f:
            lines = f.readlines()
        if len(lines) > 2:
            print(f"Meta with >2 lines: {meta} ({len(lines)} lines)")

if __name__ == '__main__':
    check_all_metas()
