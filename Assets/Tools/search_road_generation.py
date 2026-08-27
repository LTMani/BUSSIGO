import glob
import re

def search_road_generation():
    files = glob.glob("Assets/**/*.cs", recursive=True)
    for f in files:
        with open(f, 'r', encoding='utf-8', errors='ignore') as fp:
            content = fp.read()
            if 'MeshFilter' in content or 'CreatePrimitive' in content or 'CombineMeshes' in content or 'RoadSegment' in content:
                print(f"Match in: {f}")

if __name__ == '__main__':
    search_road_generation()
