using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Routes
{
    public class CorridorDefinitionSubnet11
    {
        public static HighwayCorridor BuildCorridor()
        {
            var corridor = new HighwayCorridor(
                "COR-SUBNET-11",
                "Origin Terminal Sector 11",
                "Destination Terminal Sector 11",
                193.5f,
                2.55f,
                220.0f
            );

            for (int w = 1; w <= 16; w++)
            {
                double lat = 15.0 + (11 * 0.12) + (w * 0.045);
                double lon = 78.0 + (11 * 0.15) + (w * 0.052);
                double elev = 25.0 + (w * 18.5) + ((11 % 4) * 45.0);
                float speedLimit = (w % 4 == 0) ? 50.0f : 80.0f;
                bool isStop = (w == 1 || w == 8 || w == 16);

                var wp = new RouteWaypoint(
                    $"WP-SUBNET-11-W{w:D2}",
                    $"Waypoint Sector 11 Node {w:D2}",
                    lat,
                    lon,
                    elev,
                    speedLimit,
                    isStop
                );
                corridor.AddWaypoint(wp);
            }

            return corridor;
        }
    }
}
