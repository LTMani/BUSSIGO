using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Routes
{
    public class InterstateExpansionCorridorDefinition36
    {
        public static HighwayCorridor BuildInterstateCorridor()
        {
            var corridor = new HighwayCorridor(
                "COR-INTERSTATE-36",
                "South Indian Capital Hub 36",
                "Interstate Terminal Hub 36",
                600.0f,
                5.38f,
                290.0f
            );

            for (int w = 1; w <= 14; w++)
            {
                double lat = 13.0 + (36 * 0.09) + (w * 0.038);
                double lon = 77.5 + (36 * 0.11) + (w * 0.045);
                double elev = 45.0 + (w * 22.0);
                float spd = (w % 3 == 0) ? 60.0f : 80.0f;
                bool isStop = (w == 1 || w == 7 || w == 14);

                corridor.AddWaypoint(new RouteWaypoint(
                    $"WP-INTERSTATE-36-W{w:D2}",
                    $"Interstate Node 36-{w:D2}",
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
