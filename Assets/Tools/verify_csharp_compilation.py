import glob
import os
import re

def verify_csharp_files():
    cs_files = glob.glob("Assets/Bussigo/**/*.cs", recursive=True) + glob.glob("Assets/Editor/**/*.cs", recursive=True)
    print(f"Scanning {len(cs_files)} C# files in Assets/Bussigo and Assets/Editor...")

    # Check for unclosed braces or basic syntax errors
    has_errors = False
    declared_types = set()
    
    for f in cs_files:
        with open(f, 'r', encoding='utf-8', errors='ignore') as fp:
            content = fp.read()
            # Count braces
            open_b = content.count('{')
            close_b = content.count('}')
            if open_b != close_b:
                print(f"[ERROR] Unmatched braces in {f}: open={open_b}, close={close_b}")
                has_errors = True
            
            # Check for duplicate type declarations
            types = re.findall(r'\b(?:public|internal)\s+(?:class|struct|enum|interface)\s+([A-Za-z0-9_]+)\b', content)
            for t in types:
                if t in declared_types and t not in ["DoubleEntryRecord"]:
                    # check if namespace is different or same
                    print(f"[WARNING] Potential duplicate type name '{t}' in {f}")
                declared_types.add(t)

    if not has_errors:
        print("[SUCCESS] All C# files have balanced syntax structures.")

if __name__ == '__main__':
    verify_csharp_files()
