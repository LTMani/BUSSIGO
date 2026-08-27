import glob
import re

def check_physics_calls():
    files = glob.glob("Assets/**/*.cs", recursive=True)
    found = []
    for f in files:
        with open(f, 'r', encoding='utf-8', errors='ignore') as fp:
            for line_no, line in enumerate(fp.readlines(), start=1):
                clean = line.strip()
                if clean.startswith("//") or clean.startswith("/*") or clean.startswith("*"):
                    continue
                # check for bare Physics.
                if re.search(r'(?<!UnityEngine\.)\bPhysics\.(Raycast|RaycastAll|SphereCast|BoxCast|CapsuleCast|OverlapSphere|OverlapBox|Linecast|CheckSphere|CheckCapsule|CheckBox|gravity|autoSimulation|defaultContactOffset)\b', clean):
                    found.append((f, line_no, clean))

    print(f"Found {len(found)} bare Physics API calls:")
    for f, line_no, clean in found:
        print(f"  {f}:{line_no} -> {clean}")

if __name__ == '__main__':
    check_physics_calls()
