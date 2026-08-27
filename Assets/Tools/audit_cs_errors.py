import glob
import os
import re

def audit_all_cs():
    files = glob.glob("Assets/**/*.cs", recursive=True)
    print(f"Total C# files found in project: {len(files)}")
    
    corrupted_files = []
    
    for f in files:
        with open(f, 'r', encoding='utf-8', errors='ignore') as fp:
            content = fp.read()
            
            # Check for "version":""
            if '"version":""' in content or '"version": "' in content and ':' in content:
                # check if malformed
                for line_no, line in enumerate(content.splitlines(), start=1):
                    if '"version":""' in line or re.search(r'string\s+\w+\s*=\s*"[^"]*":""', line):
                        corrupted_files.append((f, line_no, line.strip()))

    print(f"Found {len(corrupted_files)} files with string escaping corruption:")
    for f, line_no, line in corrupted_files:
        print(f"  {f}:{line_no} -> {line}")

if __name__ == '__main__':
    audit_all_cs()
