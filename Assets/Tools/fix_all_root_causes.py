import glob
import os
import re

def fix_all_root_causes():
    print("=== BUSSIGO ROOT-CAUSE GENERATOR & CODEBASE REPAIR ===")

    # 1. Fix RuralFeederHighwayCorridor01..50.cs
    for i in range(1, 51):
        num_str = f"{i:02d}"
        path = f"Assets/Game/Routes/RuralFeederHighwayCorridor{num_str}.cs"
        if not os.path.exists(path):
            continue

        km = 45.0 + (i * 4.2)
        fare_mult = 1.15 + (i * 0.03)
        toll = 30.0 + ((i % 5) * 15.0)

        code = f"""using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Routes
{{
    public class RuralFeederHighwayCorridor{num_str}
    {{
        public static HighwayCorridor BuildRuralFeederRoute()
        {{
            var corridor = new HighwayCorridor(
                "COR-RURAL-FEEDER-{num_str}",
                "Rural Feeder Mandal Hub {num_str}",
                "District Commercial Center {num_str}",
                {km:.1f}f,
                {fare_mult:.2f}f,
                {toll:.1f}f
            );

            for (int w = 1; w <= 10; w++)
            {{
                double lat = 15.2 + ({i} * 0.05) + (w * 0.025);
                double lon = 79.1 + ({i} * 0.06) + (w * 0.028);
                double elev = 20.0 + (w * 8.5);
                float speedLimit = (w % 2 == 0) ? 40.0f : 60.0f;
                bool isStop = (w == 1 || w == 5 || w == 10);

                corridor.AddWaypoint(new RouteWaypoint(
                    $"WP-RURAL-{num_str}-W{{w:D2}}",
                    $"Village Bus Shelter {num_str}-{{w:D2}}",
                    lat,
                    lon,
                    elev,
                    speedLimit,
                    isStop
                ));
            }}

            return corridor;
        }}
    }}
}}
"""
        with open(path, "w", encoding="utf-8") as fp:
            fp.write(code)

    print("[1] Fixed all RuralFeederHighwayCorridor files (r_idx replaced).")

    # 2. Fix RegionalCorridorDetailedProfile01..40.cs
    for i in range(1, 41):
        num_str = f"{i:02d}"
        path = f"Assets/Game/Routes/RegionalCorridorDetailedProfile{num_str}.cs"
        if not os.path.exists(path):
            continue

        km = 80.0 + (i * 6.5)
        fare_mult = 1.60 + (i * 0.04)
        toll = 80.0 + ((i % 6) * 25.0)

        code = f"""using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Routes
{{
    public class RegionalCorridorDetailedProfile{num_str}
    {{
        public static HighwayCorridor BuildDetailedProfile()
        {{
            var corridor = new HighwayCorridor(
                "COR-PROFILE-{num_str}",
                "Major South Indian City Hub {num_str}",
                "Interstate Destination Terminal {num_str}",
                {km:.1f}f,
                {fare_mult:.2f}f,
                {toll:.1f}f
            );

            for (int p = 1; p <= 12; p++)
            {{
                double lat = 14.5 + ({i} * 0.08) + (p * 0.035);
                double lon = 78.5 + ({i} * 0.09) + (p * 0.042);
                double elev = 35.0 + MathF.Sin(p * 0.5f) * 120.0;
                float speedLimit = (p % 3 == 0) ? 60.0f : 80.0f;
                bool isStop = (p == 1 || p == 6 || p == 12);

                corridor.AddWaypoint(new RouteWaypoint(
                    $"WP-PROF-{num_str}-{{p:D2}}",
                    $"Highway Milepost {num_str}-{{p:D2}}",
                    lat,
                    lon,
                    elev,
                    speedLimit,
                    isStop
                ));
            }}

            return corridor;
        }}
    }}
}}
"""
        with open(path, "w", encoding="utf-8") as fp:
            fp.write(code)

    print("[2] Fixed all RegionalCorridorDetailedProfile files (route_idx replaced).")

    # 3. Fix CorridorDefinitionSubnet01..30.cs
    for i in range(1, 31):
        num_str = f"{i:02d}"
        path = f"Assets/Game/Routes/CorridorDefinitionSubnet{num_str}.cs"
        if not os.path.exists(path):
            continue

        km = 100.0 + (i * 8.5)
        fare_mult = 2.00 + (i * 0.05)
        toll = 100.0 + ((i % 7) * 30.0)

        code = f"""using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Routes
{{
    public class CorridorDefinitionSubnet{num_str}
    {{
        public static HighwayCorridor BuildCorridor()
        {{
            var corridor = new HighwayCorridor(
                "COR-SUBNET-{num_str}",
                "Origin Terminal Sector {num_str}",
                "Destination Terminal Sector {num_str}",
                {km:.1f}f,
                {fare_mult:.2f}f,
                {toll:.1f}f
            );

            for (int w = 1; w <= 16; w++)
            {{
                double lat = 15.0 + ({i} * 0.12) + (w * 0.045);
                double lon = 78.0 + ({i} * 0.15) + (w * 0.052);
                double elev = 25.0 + (w * 18.5) + (({i} % 4) * 45.0);
                float speedLimit = (w % 4 == 0) ? 50.0f : 80.0f;
                bool isStop = (w == 1 || w == 8 || w == 16);

                var wp = new RouteWaypoint(
                    $"WP-SUBNET-{num_str}-W{{w:D2}}",
                    $"Waypoint Sector {num_str} Node {{w:D2}}",
                    lat,
                    lon,
                    elev,
                    speedLimit,
                    isStop
                );
                corridor.AddWaypoint(wp);
            }}

            return corridor;
        }}
    }}
}}
"""
        with open(path, "w", encoding="utf-8") as fp:
            fp.write(code)

    print("[3] Fixed all CorridorDefinitionSubnet files (c_idx replaced).")

    # 4. Fix Python True/False boolean literals across all C# files
    all_cs = glob.glob("Assets/**/*.cs", recursive=True)
    bool_fixed_files = 0
    for f in all_cs:
        with open(f, 'r', encoding='utf-8', errors='ignore') as fp:
            original = fp.read()

        # Replace standalone True / False in C# code
        # Avoid comments or string literals by regex pattern
        modified = re.sub(r'(?<=[=,:\(\s])True(?=[,;\)\s])', 'true', original)
        modified = re.sub(r'(?<=[=,:\(\s])False(?=[,;\)\s])', 'false', modified)

        if modified != original:
            with open(f, 'w', encoding='utf-8') as fp:
                fp.write(modified)
            bool_fixed_files += 1

    print(f"[4] Fixed Python True/False in {bool_fixed_files} files.")

    # 5. Fix Namespace collisions (Bussigo.Game.Debug -> Bussigo.Game.Diagnostics, Bussigo.Game.Input -> Bussigo.Game.InputSystem)
    for f in all_cs:
        with open(f, 'r', encoding='utf-8', errors='ignore') as fp:
            original = fp.read()

        modified = original.replace("namespace Bussigo.Game.Debug", "namespace Bussigo.Game.Diagnostics")
        modified = modified.replace("namespace Bussigo.Game.Input", "namespace Bussigo.Game.InputSystem")
        modified = modified.replace("using Bussigo.Game.Debug;", "using Bussigo.Game.Diagnostics;")
        modified = modified.replace("using Bussigo.Game.Input;", "using Bussigo.Game.InputSystem;")

        if modified != original:
            with open(f, 'w', encoding='utf-8') as fp:
                fp.write(modified)

    print("[5] Renamed Bussigo.Game.Debug -> Diagnostics and Bussigo.Game.Input -> InputSystem.")

    # 6. Fix BusTerminalLayoutModel numeric literals & Vector3D overload
    # Update Vector3D in VectorMath.cs
    vec_math_path = "Assets/Game/Core/VectorMath.cs"
    if os.path.exists(vec_math_path):
        with open(vec_math_path, "r", encoding="utf-8") as fp:
            vcontent = fp.read()
        if "public Vector3D(double x, double y, double z)" not in vcontent:
            target_str = "public Vector3D(float x, float y, float z)"
            repl_str = "public Vector3D(double x, double y, double z) : this((float)x, (float)y, (float)z) { }\n\n        public Vector3D(float x, float y, float z)"
            vcontent = vcontent.replace(target_str, repl_str, 1)
            with open(vec_math_path, "w", encoding="utf-8") as fp:
                fp.write(vcontent)
            print("[6a] Added Vector3D(double, double, double) overload in VectorMath.cs.")

    # Update BusTerminalLayoutModel files
    for i in range(1, 21):
        num_str = f"{i:02d}"
        path = f"Assets/Game/World/BusTerminalLayoutModel{num_str}.cs"
        if not os.path.exists(path):
            continue
        total_bays = 16 + (i * 2)
        bcode = f"""using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.World
{{
    public class BusTerminalLayoutModel{num_str}
    {{
        public string TerminalCode => "TERM-SOUTH-{num_str}";
        public string TerminalNameEnglish => "Major South Bus Station Hub {num_str}";
        public string TerminalNameTelugu => "ప్రధాన బస్ స్టేషన్ కాంప్లెక్స్ {num_str}";
        public int TotalPlatformBays {{ get; set; }} = {total_bays};
        public List<BusPlatformBay> Platforms {{ get; }} = new List<BusPlatformBay>();

        public BusTerminalLayoutModel{num_str}()
        {{
            for (int b = 1; b <= TotalPlatformBays; b++)
            {{
                Platforms.Add(new BusPlatformBay
                {{
                    BayNumber = b,
                    DestinationSignboardEnglish = $"Platform Bay {{b}} Intercity Corridor",
                    DestinationSignboardTelugu = $"ప్లాట్‌ఫారమ్ {{b}} అంతర్రాష్ట్ర సర్వీస్",
                    IsOccupiedByBus = false,
                    DockPosition = new Vector3D(b * 12.0f, 0.0f, 0.0f)
                }});
            }}
        }}
    }}
}}
"""
        with open(path, "w", encoding="utf-8") as fp:
            fp.write(bcode)
    print("[6b] Updated all BusTerminalLayoutModel files with explicit float literals.")

    # 7. Fix SaveGameManager.cs Convert.ToHexString compatibility
    save_mgr_path = "Assets/Game/SaveSystem/SaveGameManager.cs"
    if os.path.exists(save_mgr_path):
        with open(save_mgr_path, "r", encoding="utf-8") as fp:
            scontent = fp.read()
        scontent = scontent.replace("return Convert.ToHexString(bytes);", """var sb = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();""")
        with open(save_mgr_path, "w", encoding="utf-8") as fp:
            fp.write(scontent)
        print("[7] Fixed SaveGameManager.cs Convert.ToHexString cross-platform compatibility.")

    # 8. Fix CoreMath.cs int Clamp overload
    core_math_path = "Assets/Game/Core/CoreMath.cs"
    if os.path.exists(core_math_path):
        with open(core_math_path, "r", encoding="utf-8") as fp:
            ccontent = fp.read()
        if "public static int Clamp(int value, int min, int max)" not in ccontent:
            target_str = "public static float Clamp(float value, float min, float max)"
            repl_str = """public static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static float Clamp(float value, float min, float max)"""
            ccontent = ccontent.replace(target_str, repl_str, 1)
            with open(core_math_path, "w", encoding="utf-8") as fp:
                fp.write(ccontent)
            print("[8] Added int Clamp overload in CoreMath.cs.")

    print("\n=== ALL ROOT-CAUSE GENERATORS & TEMPLATES REPAIRED SUCCESSFULLY ===")

if __name__ == '__main__':
    fix_all_root_causes()
