using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Routes
{
    public class RuralFeederHighwayCorridor08
    {
        public static HighwayCorridor BuildRuralFeederRoute()
        {
            var corridor = new HighwayCorridor(
                "COR-RURAL-FEEDER-08",
                "Rural Feeder Mandal Hub 08",
                "District Commercial Center 08",
                78.6f,
                1.39f,
                75.0f
            );

            for (int w = 1; w <= 10; w++)
            {
                double lat = 15.2 + (8 * 0.05) + (w * 0.025);
                double lon = 79.1 + (8 * 0.06) + (w * 0.028);
                double elev = 20.0 + (w * 8.5);
                float speedLimit = (w % 2 == 0) ? 40.0f : 60.0f;
                bool isStop = (w == 1 || w == 5 || w == 10);

                corridor.AddWaypoint(new RouteWaypoint(
                    $"WP-RURAL-08-W{w:D2}",
                    $"Village Bus Shelter 08-{w:D2}",
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
