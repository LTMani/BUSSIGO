using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Routes
{
    public class RuralFeederHighwayCorridor48
    {
        public static HighwayCorridor BuildRuralFeederRoute()
        {
            var corridor = new HighwayCorridor(
                "COR-RURAL-FEEDER-048",
                "Rural Feeder Mandal Hub 48",
                "District Commercial Center 48",
                246.6f,
                6.86f,
                275.0f
            );

            for (int w = 1; w <= 10; w++)
            {
                double lat = 15.2 + (r_idx * 0.05) + (w * 0.025);
                double lon = 79.1 + (r_idx * 0.06) + (w * 0.028);
                double elev = 20.0 + (w * 8.5);
                float speedLimit = (w % 2 == 0) ? 40.0f : 60.0f;
                bool isStop = (w == 1 || w == 5 || w == 10);

                corridor.AddWaypoint(new RouteWaypoint(
                    $"WP-RURAL-048-W{w:D2}",
                    $"Village Bus Shelter 048-{w:D2}",
                    lat,
                    lon,
                    elev,
                    speedLimit,
                    isStop
                ));
            }

            return corridor;
        }
    }
}
