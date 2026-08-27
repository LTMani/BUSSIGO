import glob
import re

def find_input_debug_files():
    files = glob.glob("Assets/**/*.cs", recursive=True)
    for f in files:
        with open(f, 'r', encoding='utf-8', errors='ignore') as fp:
            content = fp.read()
            if 'namespace Bussigo.Game.Input' in content:
                print(f"File in Bussigo.Game.Input: {f}")
            if 'namespace Bussigo.Game.Debug' in content:
                print(f"File in Bussigo.Game.Debug: {f}")

if __name__ == '__main__':
    find_input_debug_files()
