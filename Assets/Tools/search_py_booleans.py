import glob
import re

def search_py_booleans():
    files = glob.glob("Assets/**/*.cs", recursive=True)
    found = []
    for f in files:
        with open(f, 'r', encoding='utf-8', errors='ignore') as fp:
            content = fp.read()
            for line_no, line in enumerate(content.splitlines(), start=1):
                clean = line.strip()
                if clean.startswith("//") or clean.startswith("/*") or clean.startswith("*"):
                    continue
                if re.search(r'\b(True|False)\b', clean):
                    found.append((f, line_no, clean))

    print(f"Found {len(found)} lines with Python True/False literals:")
    for f, line_no, line in found[:30]:
        print(f"  {f}:{line_no} -> {line}")
    if len(found) > 30:
        print(f"  ... and {len(found) - 30} more")

if __name__ == '__main__':
    search_py_booleans()
