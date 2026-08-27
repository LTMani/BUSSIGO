import glob
import re

def check_initializers():
    files = glob.glob("Assets/Bussigo/**/*.cs", recursive=True)
    for f in files:
        with open(f, 'r', encoding='utf-8', errors='ignore') as fp:
            content = fp.read()
            if 'ServiceLocator.Register' in content:
                print(f"ServiceLocator.Register in: {f}")

if __name__ == '__main__':
    check_initializers()
