import os
from pathlib import Path

def audit_models():
    model_dir = Path("Assets/Bussigo/Assets/Models/Bus")
    for obj_file in model_dir.glob("*.obj"):
        with open(obj_file, "r", encoding="utf-8") as f:
            lines = f.readlines()
        v_count = sum(1 for l in lines if l.startswith("v "))
        vn_count = sum(1 for l in lines if l.startswith("vn "))
        vt_count = sum(1 for l in lines if l.startswith("vt "))
        f_count = sum(1 for l in lines if l.startswith("f "))
        groups = set(l.split()[1] for l in lines if l.startswith("g ") or l.startswith("o "))
        size_kb = obj_file.stat().st_size / 1024
        print(f"[{obj_file.name}] Size: {size_kb:.1f} KB | Verts: {v_count}, Normals: {vn_count}, UVs: {vt_count}, Faces: {f_count}")
        print(f"   Groups: {groups}")

def audit_textures():
    tex_dir = Path("Assets/Bussigo/Assets/Textures")
    print("\n--- TEXTURES AUDIT ---")
    for tex in tex_dir.rglob("*.png"):
        size_kb = tex.stat().st_size / 1024
        print(f"[{tex.relative_to(tex_dir)}] Size: {size_kb:.1f} KB")

if __name__ == '__main__':
    audit_models()
    audit_textures()
