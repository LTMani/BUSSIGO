from pathlib import Path

def find_all_editor_scripts():
    root = Path("Assets")
    for p in root.rglob("*.cs"):
        if "Editor" in p.parts:
            print(p)

if __name__ == '__main__':
    find_all_editor_scripts()
