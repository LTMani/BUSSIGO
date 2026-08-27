import glob
import re

def search_all_unescaped_tokens():
    files = glob.glob("Assets/**/*.cs", recursive=True)
    found = []
    
    # Common variable patterns in python generators
    pattern = re.compile(r'\b([a-zA-Z]+_idx|[a-zA-Z]+_val|[a-zA-Z]+_num)\b')

    for f in files:
        with open(f, 'r', encoding='utf-8', errors='ignore') as fp:
            for line_no, line in enumerate(fp.readlines(), start=1):
                clean = line.strip()
                if clean.startswith("//") or clean.startswith("/*") or clean.startswith("*"):
                    continue
                matches = pattern.findall(clean)
                for m in matches:
                    found.append((f, line_no, m, clean))

    print(f"Found {len(found)} unescaped python generator variables across project:")
    for f, line_no, m, clean in found:
        print(f"  {f}:{line_no} -> '{m}' in: {clean}")

if __name__ == '__main__':
    search_all_unescaped_tokens()
