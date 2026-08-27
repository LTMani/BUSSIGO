import glob

def search_license_tier():
    files = glob.glob("Assets/**/*.cs", recursive=True)
    found = []
    for f in files:
        with open(f, 'r', encoding='utf-8', errors='ignore') as fp:
            content = fp.read()
            if 'LicenseTier' in content:
                found.append(f)
    print(f"Found 'LicenseTier' in {len(found)} files:")
    for f in found:
        print(f"  {f}")

if __name__ == '__main__':
    search_license_tier()
