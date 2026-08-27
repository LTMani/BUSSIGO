import glob
import re

def find_vector3d():
    files = glob.glob("Assets/**/*.cs", recursive=True)
    for f in files:
        with open(f, 'r', encoding='utf-8', errors='ignore') as fp:
            content = fp.read()
            if 'struct Vector3D' in content or 'class Vector3D' in content:
                print(f"Vector3D declared in: {f}")

if __name__ == '__main__':
    find_vector3d()
