import glob
import os
import re

def search_symbols():
    symbols = ["LedgerEntry", "GamePhase", "ServiceLocator", "EventBus", "NH65HighwayNetworkBuilder"]
    cs_files = glob.glob("Assets/**/*.cs", recursive=True)
    
    print("=== SYMBOL SEARCH ===")
    for sym in symbols:
        print(f"\n--- Searching for '{sym}' definitions ---")
        for f in cs_files:
            with open(f, 'r', encoding='utf-8', errors='ignore') as fp:
                lines = fp.readlines()
                for idx, line in enumerate(lines):
                    if re.search(rf'\b(class|struct|enum|interface)\s+{sym}\b', line):
                        print(f"  {f}:{idx+1} -> {line.strip()}")

if __name__ == '__main__':
    search_symbols()
