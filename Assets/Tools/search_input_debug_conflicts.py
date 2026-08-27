import glob
import re

def search_input_debug_conflicts():
    files = glob.glob("Assets/**/*.cs", recursive=True)
    conflicts = []
    for f in files:
        with open(f, 'r', encoding='utf-8', errors='ignore') as fp:
            content = fp.read()
            # check if file declares class Input or class Debug or namespace Input or namespace Debug
            if re.search(r'\b(class|struct|enum|namespace)\s+(Input|Debug)\b', content):
                conflicts.append(f)

    print(f"Found {len(conflicts)} files declaring Input or Debug:")
    for f in conflicts:
        print(f"  {f}")

if __name__ == '__main__':
    search_input_debug_conflicts()
