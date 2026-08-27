import os

def fix_interstate_expansion_corridors():
    for i in range(1, 41):
        num_str = f"{i:02d}"
        path = f"Assets/Game/Routes/InterstateExpansionCorridorDefinition{num_str}.cs"
        if not os.path.exists(path):
            continue

        km = 150.0 + (i * 12.5)
        fare_mult = 2.50 + (i * 0.08)
        toll = 150.0 + ((i % 8) * 35.0)

        code = f"""using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Routes
{{
    public class InterstateExpansionCorridorDefinition{num_str}
    {{
        public static HighwayCorridor BuildInterstateCorridor()
        {{
            var corridor = new HighwayCorridor(
                "COR-INTERSTATE-{num_str}",
                "South Indian Capital Hub {num_str}",
                "Interstate Terminal Hub {num_str}",
                {km:.1f}f,
                {fare_mult:.2f}f,
                {toll:.1f}f
            );

            for (int w = 1; w <= 14; w++)
            {{
                double lat = 13.0 + ({i} * 0.09) + (w * 0.038);
                double lon = 77.5 + ({i} * 0.11) + (w * 0.045);
                double elev = 45.0 + (w * 22.0);
                float spd = (w % 3 == 0) ? 60.0f : 80.0f;
                bool isStop = (w == 1 || w == 7 || w == 14);

                corridor.AddWaypoint(new RouteWaypoint(
                    $"WP-INTERSTATE-{num_str}-W{{w:D2}}",
                    $"Interstate Node {num_str}-{{w:D2}}",
                    lat,
                    lon,
                    elev,
                    spd,
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

    print("Fixed all InterstateExpansionCorridorDefinition files (cr_idx replaced with loop index).")

if __name__ == '__main__':
    fix_interstate_expansion_corridors()
