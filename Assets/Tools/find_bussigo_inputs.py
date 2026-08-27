import glob
import re

def find_bussigo_input_calls():
    files = glob.glob("Assets/Bussigo/**/*.cs", recursive=True)
    found = []
    for f in files:
        with open(f, 'r', encoding='utf-8', errors='ignore') as fp:
            for line_no, line in enumerate(fp.readlines(), start=1):
                clean = line.strip()
                if clean.startswith("//") or clean.startswith("/*") or clean.startswith("*"):
                    continue
                if re.search(r'\bInput\.(GetKey|GetKeyDown|GetKeyUp|GetAxis|GetAxisRaw|GetButton|GetButtonDown)\b', clean):
                    found.append((f, line_no, clean))

    print(f"Found {len(found)} Input calls in Assets/Bussigo:")
    for f, line_no, clean in found:
        print(f"  {f}:{line_no} -> {clean}")

if __name__ == '__main__':
    find_bussigo_input_calls()
