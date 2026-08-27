import glob
import re

def find_iservices():
    files = glob.glob("Assets/**/*.cs", recursive=True)
    for f in files:
        with open(f, 'r', encoding='utf-8', errors='ignore') as fp:
            content = fp.read()
            if ': IService' in content or ', IService' in content:
                print(f"IService implemented in: {f}")

if __name__ == '__main__':
    find_iservices()
