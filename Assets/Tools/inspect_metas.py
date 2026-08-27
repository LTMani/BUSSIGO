from pathlib import Path

def inspect_meta_files():
    tex_dir = Path("Assets/Bussigo/Assets/Textures")
    print("--- TEXTURE METAS ---")
    for meta in tex_dir.glob("*.png.meta"):
        with open(meta, "r", encoding="utf-8") as f:
            content = f.read()
        print(f"\n[{meta.name}]")
        for line in content.splitlines()[:25]:
            print("  ", line)

    model_dir = Path("Assets/Bussigo/Assets/Models/Bus")
    print("\n--- MODEL METAS ---")
    for meta in model_dir.glob("*.obj.meta"):
        with open(meta, "r", encoding="utf-8") as f:
            content = f.read()
        print(f"\n[{meta.name}]")
        for line in content.splitlines()[:20]:
            print("  ", line)

if __name__ == '__main__':
    inspect_meta_files()
