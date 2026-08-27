import glob
import os
import re

def get_guids():
    files = glob.glob('Assets/**/*.cs.meta', recursive=True)
    all_guids = {}
    for f in files:
        name = os.path.basename(f).replace('.meta', '')
        with open(f, 'r', encoding='utf-8') as fp:
            m = re.search(r'guid:\s*([a-f0-9]+)', fp.read())
            if m:
                all_guids[name] = m.group(1)

    for k in sorted(all_guids.keys()):
        if any(x in k for x in ['Bussigo', 'Bus', 'Vehicle', 'Road', 'Route', 'Traffic', 'Weather', 'Time', 'Passenger', 'Audio', 'HUD', 'Economy', 'Company', 'Save', 'Engine', 'Pneumatic', 'Tire', 'Service', 'Event', 'State']):
            print(f'"{k}": "{all_guids[k]}",')

if __name__ == '__main__':
    get_guids()
