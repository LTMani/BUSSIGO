using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Routes
{
    public class RuralFeederHighwayCorridor10
    {
        public static HighwayCorridor BuildRuralFeederRoute()
        {
            var corridor = new HighwayCorridor(
                "COR-RURAL-FEEDER-10",
                "Rural Feeder Mandal Hub 10",
                "District Commercial Center 10",
                87.0f,
                1.45f,
                30.0f
            );

            for (int w = 1; w <= 10; w++)
            {
                double lat = 15.2 + (10 * 0.05) + (w * 0.025);
                double lon = 79.1 + (10 * 0.06) + (w * 0.028);
                double elev = 20.0 + (w * 8.5);
                float speedLimit = (w % 2 == 0) ? 40.0f : 60.0f;
                bool isStop = (w == 1 || w == 5 || w == 10);

                corridor.AddWaypoint(new RouteWaypoint(
                    $"WP-RURAL-10-W{w:D2}",
                    $"Village Bus Shelter 10-{w:D2}",
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
