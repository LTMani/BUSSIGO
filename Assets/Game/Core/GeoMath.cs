using System;

namespace Bussigo.Game.Core
{
    public struct GeoCoordinate
    {
        public double Latitude;
        public double Longitude;
        public double ElevationMeters;

        public GeoCoordinate(double lat, double lon, double elev = 0.0)
        {
            Latitude = lat;
            Longitude = lon;
            ElevationMeters = elev;
        }

        public override string ToString() => $"({Latitude:F6}°N, {Longitude:F6}°E, {ElevationMeters:F1}m)";
    }

    public static class GeoMath
    {
        public const double EarthRadiusKm = 6371.0;
        public const double EarthRadiusMeters = 6371000.0;

        public static double HaversineDistanceMeters(GeoCoordinate from, GeoCoordinate to)
        {
            double dLat = (to.Latitude - from.Latitude) * Math.PI / 180.0;
            double dLon = (to.Longitude - from.Longitude) * Math.PI / 180.0;

            double lat1 = from.Latitude * Math.PI / 180.0;
            double lat2 = to.Latitude * Math.PI / 180.0;

            double a = Math.Sin(dLat / 2.0) * Math.Sin(dLat / 2.0) +
                       Math.Sin(dLon / 2.0) * Math.Sin(dLon / 2.0) * Math.Cos(lat1) * Math.Cos(lat2);
            double c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));

            double groundDistance = EarthRadiusMeters * c;
            double dElev = to.ElevationMeters - from.ElevationMeters;
            return Math.Sqrt(groundDistance * groundDistance + dElev * dElev);
        }

        public static double BearingDegrees(GeoCoordinate from, GeoCoordinate to)
        {
            double lat1 = from.Latitude * Math.PI / 180.0;
            double lat2 = to.Latitude * Math.PI / 180.0;
            double dLon = (to.Longitude - from.Longitude) * Math.PI / 180.0;

            double y = Math.Sin(dLon) * Math.Cos(lat2);
            double x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);
            double initialBearing = Math.Atan2(y, x);

            return (initialBearing * 180.0 / Math.PI + 360.0) % 360.0;
        }

        public static Vector3D GeoToLocalMeters(GeoCoordinate point, GeoCoordinate origin)
        {
            double dLat = point.Latitude - origin.Latitude;
            double dLon = point.Longitude - origin.Longitude;

            double metersPerDegreeLat = 111132.92 - 559.82 * Math.Cos(2 * origin.Latitude * Math.PI / 180.0);
            double metersPerDegreeLon = 111412.84 * Math.Cos(origin.Latitude * Math.PI / 180.0);

            float x = (float)(dLon * metersPerDegreeLon);
            float z = (float)(dLat * metersPerDegreeLat);
            float y = (float)(point.ElevationMeters - origin.ElevationMeters);

            return new Vector3D(x, y, z);
        }
    }
}
