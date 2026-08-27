import glob
import re

def search_namespaces():
    files = glob.glob("Assets/**/*.cs", recursive=True)
    namespaces = set()
    for f in files:
        with open(f, 'r', encoding='utf-8', errors='ignore') as fp:
            content = fp.read()
            matches = re.findall(r'namespace\s+([A-Za-z0-9_.]+)', content)
            for m in matches:
                namespaces.add(m)

    print(f"Found {len(namespaces)} unique namespaces:")
    for ns in sorted(namespaces):
        if 'Input' in ns or 'Debug' in ns or 'UI' in ns or 'Vehicle' in ns or 'Physics' in ns:
            print(f"  -> {ns}")

if __name__ == '__main__':
    search_namespaces()
