import glob
import os
import re
from collections import defaultdict

def find_all_duplicate_types():
    files = glob.glob("Assets/**/*.cs", recursive=True)
    print(f"Scanning {len(files)} files for duplicate types declared in the same namespace...")

    # (namespace, type_name) -> list of (file_path, type_kind, line_number)
    type_declarations = defaultdict(list)

    for f in files:
        with open(f, 'r', encoding='utf-8', errors='ignore') as fp:
            content = fp.read()

        # Find namespace
        ns_match = re.search(r'namespace\s+([A-Za-z0-9_.]+)', content)
        ns = ns_match.group(1) if ns_match else "Global"

        # Find all class, struct, enum, interface declarations
        # e.g. public enum StaffRole, public struct JournalEntryLine, public class MonthlyInstallmentRow
        matches = re.finditer(r'\b(?:public|internal|protected|private)?\s*(?:static\s+|sealed\s+|abstract\s+)?(class|struct|enum|interface)\s+([A-Za-z0-9_]+)\b', content)
        for m in matches:
            kind = m.group(1)
            type_name = m.group(2)
            # calculate line number
            line_no = content[:m.start()].count('\n') + 1
            type_declarations[(ns, type_name)].append((f, kind, line_no))

    duplicates = {}
    for (ns, name), decls in type_declarations.items():
        if len(decls) > 1:
            duplicates[(ns, name)] = decls

    print(f"\n=======================================================")
    print(f"Found {len(duplicates)} duplicate type names across namespaces:")
    print(f"=======================================================\n")

    for (ns, name), decls in sorted(duplicates.items(), key=lambda x: len(x[1]), reverse=True):
        print(f"[{ns}] '{name}' declared in {len(decls)} files:")
        for file_path, kind, line_no in decls[:5]:
            print(f"   -> {file_path}:{line_no} ({kind})")
        if len(decls) > 5:
            print(f"   -> ... and {len(decls) - 5} more files")

if __name__ == '__main__':
    find_all_duplicate_types()
