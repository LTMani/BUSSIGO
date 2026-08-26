using System;
using Bussigo.Game.Core;

namespace Bussigo.Game.Weather
{
    public class AtmosphericMonsoonSolver03
    {
        public float SolarLatitudeDegrees => 13.90f;
        public float SolarDeclinationDegrees { get; set; } = 18.5f;

        public (float sunElevationDeg, float sunAzimuthDeg) CalculateSolarPosition(float hourOfDay24)
        {
            float hourAngleDeg = (hourOfDay24 - 12.0f) * 15.0f;
            float hourAngleRad = hourAngleDeg * CoreMath.DegToRad;
            float latRad = SolarLatitudeDegrees * CoreMath.DegToRad;
            float declRad = SolarDeclinationDegrees * CoreMath.DegToRad;

            float sinElev = MathF.Sin(latRad) * MathF.Sin(declRad) + MathF.Cos(latRad) * MathF.Cos(declRad) * MathF.Cos(hourAngleRad);
            float elevRad = MathF.Asin(CoreMath.Clamp(sinElev, -1.0f, 1.0f));

            float cosAz = (MathF.Sin(declRad) - MathF.Sin(latRad) * MathF.Sin(elevRad)) / (MathF.Cos(latRad) * MathF.Cos(elevRad) + 1e-5f);
            float azRad = MathF.Acos(CoreMath.Clamp(cosAz, -1.0f, 1.0f));

            if (hourOfDay24 > 12.0f)
            {
                azRad = (2.0f * MathF.PI) - azRad;
            }

            return (elevRad * CoreMath.RadToDeg, azRad * CoreMath.RadToDeg);
        }
    }
}
