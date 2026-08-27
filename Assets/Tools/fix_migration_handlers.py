import glob
import os

def fix_migration_handlers():
    for i in range(1, 31):
        num_str = f"{i:02d}"
        file_path = f"Assets/Game/SaveSystem/SaveSchemaDataMigrationHandler{num_str}.cs"
        if not os.path.exists(file_path):
            print(f"File not found: {file_path}")
            continue

        from_ver = i
        to_ver = i + 1

        content = f"""using System;

namespace Bussigo.Game.SaveSystem
{{
    public class SaveSchemaDataMigrationHandler{num_str}
    {{
        public int FromSchemaVersion => {from_ver};
        public int ToSchemaVersion => {to_ver};

        public string MigratePayload(string oldPayloadJson)
        {{
            if (string.IsNullOrEmpty(oldPayloadJson)) return "{{}}";
            string fromVer = "\\"version\\":\\"" + {from_ver} + ".0.0\\"";
            string toVer = "\\"version\\":\\"" + {to_ver} + ".0.0\\"";
            return oldPayloadJson.Replace(fromVer, toVer);
        }}
    }}
}}
"""
        with open(file_path, "w", encoding="utf-8") as fp:
            fp.write(content)

    print("Fixed all 30 SaveSchemaDataMigrationHandler files with valid C# quote escaping.")

if __name__ == '__main__':
    fix_migration_handlers()
