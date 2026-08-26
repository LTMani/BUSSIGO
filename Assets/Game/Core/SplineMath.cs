using System;

namespace Bussigo.Game.Core
{
    public static class SplineMath
    {
        public static Vector3D EvaluateCatmullRom(Vector3D p0, Vector3D p1, Vector3D p2, Vector3D p3, float t)
        {
            float t2 = t * t;
            float t3 = t2 * t;

            float f0 = -0.5f * t3 + t2 - 0.5f * t;
            float f1 = 1.5f * t3 - 2.5f * t2 + 1.0f;
            float f2 = -1.5f * t3 + 2.0f * t2 + 0.5f * t;
            float f3 = 0.5f * t3 - 0.5f * t2;

            return p0 * f0 + p1 * f1 + p2 * f2 + p3 * f3;
        }

        public static Vector3D EvaluateCatmullRomTangent(Vector3D p0, Vector3D p1, Vector3D p2, Vector3D p3, float t)
        {
            float t2 = t * t;

            float f0 = -1.5f * t2 + 2.0f * t - 0.5f;
            float f1 = 4.5f * t2 - 5.0f * t;
            float f2 = -4.5f * t2 + 4.0f * t + 0.5f;
            float f3 = 1.5f * t2 - 1.0f * t;

            return (p0 * f0 + p1 * f1 + p2 * f2 + p3 * f3).Normalized;
        }

        public static Vector3D EvaluateBezier(Vector3D p0, Vector3D p1, Vector3D p2, Vector3D p3, float t)
        {
            float u = 1.0f - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector3D p = p0 * uuu;
            p += p1 * (3.0f * uu * t);
            p += p2 * (3.0f * u * tt);
            p += p3 * ttt;
            return p;
        }

        public static float ApproximateSplineLength(Vector3D p0, Vector3D p1, Vector3D p2, Vector3D p3, int steps = 20)
        {
            float length = 0.0f;
            Vector3D lastPoint = EvaluateCatmullRom(p0, p1, p2, p3, 0.0f);
            for (int i = 1; i <= steps; i++)
            {
                float t = (float)i / steps;
                Vector3D pt = EvaluateCatmullRom(p0, p1, p2, p3, t);
                length += Vector3D.Distance(lastPoint, pt);
                lastPoint = pt;
            }
            return length;
        }
    }
}
