using System;
using System.Collections.Generic;
using Bussigo.Game.Core;

namespace Bussigo.Game.Routes
{
    public class RegionalCorridorDetailedProfile40
    {
        public static HighwayCorridor BuildDetailedProfile()
        {
            var corridor = new HighwayCorridor(
                "COR-PROFILE-40",
                "Major South Indian City Hub 40",
                "Interstate Destination Terminal 40",
                340.0f,
                3.20f,
                180.0f
            );

            for (int p = 1; p <= 12; p++)
            {
                double lat = 14.5 + (40 * 0.08) + (p * 0.035);
                double lon = 78.5 + (40 * 0.09) + (p * 0.042);
                double elev = 35.0 + MathF.Sin(p * 0.5f) * 120.0;
                float speedLimit = (p % 3 == 0) ? 60.0f : 80.0f;
                bool isStop = (p == 1 || p == 6 || p == 12);

                corridor.AddWaypoint(new RouteWaypoint(
                    $"WP-PROF-40-{p:D2}",
                    $"Highway Milepost 40-{p:D2}",
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
