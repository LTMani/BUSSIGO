using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Routes
{
    public class InterstateExpansionCorridorDefinition09
    {
        public static HighwayCorridor BuildInterstateCorridor()
        {
            var corridor = new HighwayCorridor(
                "COR-INTERSTATE-09",
                "South Indian Capital Hub 09",
                "Interstate Terminal Hub 09",
                262.5f,
                3.22f,
                185.0f
            );

            for (int w = 1; w <= 14; w++)
            {
                double lat = 13.0 + (9 * 0.09) + (w * 0.038);
                double lon = 77.5 + (9 * 0.11) + (w * 0.045);
                double elev = 45.0 + (w * 22.0);
                float spd = (w % 3 == 0) ? 60.0f : 80.0f;
                bool isStop = (w == 1 || w == 7 || w == 14);

                corridor.AddWaypoint(new RouteWaypoint(
                    $"WP-INTERSTATE-09-W{w:D2}",
                    $"Interstate Node 09-{w:D2}",
                    lat,
                    lon,
                    elev,
                    spd,
                    isStop
                ));
            }

            return corridor;
        }
    }
}
