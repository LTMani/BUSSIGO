using System;

namespace Bussigo.Game.Core
{
    /// <summary>
    /// High-performance spatial math, numerical solvers, and interpolation routines for vehicle simulation.
    /// </summary>
    public static class CoreMath
    {
        public const float Epsilon = 1e-6f;
        public const float Gravity = 9.80665f;
        public const float DegToRad = MathF.PI / 180.0f;
        public const float RadToDeg = 180.0f / MathF.PI;
        public const float KmhToMps = 1000.0f / 3600.0f;
        public const float MpsToKmh = 3600.0f / 1000.0f;

        public static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static float Clamp01(float value)
        {
            if (value < 0.0f) return 0.0f;
            if (value > 1.0f) return 1.0f;
            return value;
        }

        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * Clamp01(t);
        }

        public static float InverseLerp(float a, float b, float value)
        {
            if (MathF.Abs(b - a) < Epsilon) return 0.0f;
            return Clamp01((value - a) / (b - a));
        }

        public static float SmoothStep(float a, float b, float t)
        {
            t = Clamp01(t);
            t = t * t * (3.0f - 2.0f * t);
            return a + (b - a) * t;
        }

        public static float MoveTowards(float current, float target, float maxDelta)
        {
            if (MathF.Abs(target - current) <= maxDelta)
                return target;
            return current + MathF.Sign(target - current) * maxDelta;
        }

        public static float NormalizeAngleDegrees(float angle)
        {
            while (angle > 180.0f) angle -= 360.0f;
            while (angle < -180.0f) angle += 360.0f;
            return angle;
        }

        public static float DeltaAngleDegrees(float current, float target)
        {
            float delta = NormalizeAngleDegrees(target - current);
            return delta;
        }

        public static float LinearToDecibels(float linear)
        {
            if (linear <= 0.0001f) return -80.0f;
            return 20.0f * MathF.Log10(linear);
        }

        public static float DecibelsToLinear(float db)
        {
            return MathF.Pow(10.0f, db / 20.0f);
        }
    }
}
