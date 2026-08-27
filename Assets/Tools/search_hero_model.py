import glob
import re

def search_hero_model():
    files = glob.glob("Assets/**/*.cs", recursive=True)
    for f in files:
        with open(f, 'r', encoding='utf-8', errors='ignore') as fp:
            content = fp.read()
            if 'IndianIntercityCoach' in content or 'Hero_LOD0' in content or 'LoadAsset' in content or 'Resources.Load' in content or 'CreateRiggedCoach' in content:
                print(f"Match in: {f}")

if __name__ == '__main__':
    search_hero_model()
