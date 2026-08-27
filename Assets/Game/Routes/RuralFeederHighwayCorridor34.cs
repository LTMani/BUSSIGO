using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Routes
{
    public class RuralFeederHighwayCorridor34
    {
        public static HighwayCorridor BuildRuralFeederRoute()
        {
            var corridor = new HighwayCorridor(
                "COR-RURAL-FEEDER-34",
                "Rural Feeder Mandal Hub 34",
                "District Commercial Center 34",
                187.8f,
                2.17f,
                90.0f
            );

            for (int w = 1; w <= 10; w++)
            {
                double lat = 15.2 + (34 * 0.05) + (w * 0.025);
                double lon = 79.1 + (34 * 0.06) + (w * 0.028);
                double elev = 20.0 + (w * 8.5);
                float speedLimit = (w % 2 == 0) ? 40.0f : 60.0f;
                bool isStop = (w == 1 || w == 5 || w == 10);

                corridor.AddWaypoint(new RouteWaypoint(
                    $"WP-RURAL-34-W{w:D2}",
                    $"Village Bus Shelter 34-{w:D2}",
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
