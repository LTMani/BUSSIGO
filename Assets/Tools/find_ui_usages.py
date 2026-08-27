import glob

def find_ui_usages():
    files = glob.glob("Assets/**/*.cs", recursive=True)
    ui_files = []
    for f in files:
        with open(f, 'r', encoding='utf-8', errors='ignore') as fp:
            content = fp.read()
            if 'using UnityEngine.UI;' in content or 'UnityEngine.UI.Text' in content:
                ui_files.append(f)
    print(f"Found {len(ui_files)} files using UnityEngine.UI:")
    for f in ui_files:
        print(f"  {f}")

if __name__ == '__main__':
    find_ui_usages()
