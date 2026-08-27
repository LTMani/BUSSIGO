using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Routes
{
    public class RuralFeederHighwayCorridor30
    {
        public static HighwayCorridor BuildRuralFeederRoute()
        {
            var corridor = new HighwayCorridor(
                "COR-RURAL-FEEDER-30",
                "Rural Feeder Mandal Hub 30",
                "District Commercial Center 30",
                171.0f,
                2.05f,
                30.0f
            );

            for (int w = 1; w <= 10; w++)
            {
                double lat = 15.2 + (30 * 0.05) + (w * 0.025);
                double lon = 79.1 + (30 * 0.06) + (w * 0.028);
                double elev = 20.0 + (w * 8.5);
                float speedLimit = (w % 2 == 0) ? 40.0f : 60.0f;
                bool isStop = (w == 1 || w == 5 || w == 10);

                corridor.AddWaypoint(new RouteWaypoint(
                    $"WP-RURAL-30-W{w:D2}",
                    $"Village Bus Shelter 30-{w:D2}",
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
