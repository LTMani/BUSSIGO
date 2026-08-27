using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Routes
{
    public class RegionalCorridorDetailedProfile12
    {
        public static HighwayCorridor BuildDetailedProfile()
        {
            var corridor = new HighwayCorridor(
                "COR-PROFILE-12",
                "Major South Indian City Hub 12",
                "Interstate Destination Terminal 12",
                158.0f,
                2.08f,
                80.0f
            );

            for (int p = 1; p <= 12; p++)
            {
                double lat = 14.5 + (12 * 0.08) + (p * 0.035);
                double lon = 78.5 + (12 * 0.09) + (p * 0.042);
                double elev = 35.0 + MathF.Sin(p * 0.5f) * 120.0;
                float speedLimit = (p % 3 == 0) ? 60.0f : 80.0f;
                bool isStop = (p == 1 || p == 6 || p == 12);

                corridor.AddWaypoint(new RouteWaypoint(
                    $"WP-PROF-12-{p:D2}",
                    $"Highway Milepost 12-{p:D2}",
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
