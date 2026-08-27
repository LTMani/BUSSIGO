using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Routes
{
    public class CorridorDefinitionSubnet10
    {
        public static HighwayCorridor BuildCorridor()
        {
            var corridor = new HighwayCorridor(
                "COR-SUBNET-10",
                "Origin Terminal Sector 10",
                "Destination Terminal Sector 10",
                185.0f,
                2.50f,
                190.0f
            );

            for (int w = 1; w <= 16; w++)
            {
                double lat = 15.0 + (10 * 0.12) + (w * 0.045);
                double lon = 78.0 + (10 * 0.15) + (w * 0.052);
                double elev = 25.0 + (w * 18.5) + ((10 % 4) * 45.0);
                float speedLimit = (w % 4 == 0) ? 50.0f : 80.0f;
                bool isStop = (w == 1 || w == 8 || w == 16);

                var wp = new RouteWaypoint(
                    $"WP-SUBNET-10-W{w:D2}",
                    $"Waypoint Sector 10 Node {w:D2}",
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
