import glob
import os
import re

files = glob.glob('Assets/Bussigo/**/*.cs.meta', recursive=True)
for f in sorted(files):
    name = os.path.basename(f).replace('.meta', '')
    with open(f, 'r', encoding='utf-8') as fp:
        m = re.search(r'guid:\s*([a-f0-9]+)', fp.read())
        if m:
            print(f'"{name}": "{m.group(1)}",')
