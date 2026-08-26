using System;

namespace Bussigo.Game.Core
{
    public struct Vector2D : IEquatable<Vector2D>
    {
        public float X;
        public float Y;

        public static readonly Vector2D Zero = new Vector2D(0f, 0f);
        public static readonly Vector2D One = new Vector2D(1f, 1f);
        public static readonly Vector2D UnitX = new Vector2D(1f, 0f);
        public static readonly Vector2D UnitY = new Vector2D(0f, 1f);

        public Vector2D(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float Length => MathF.Sqrt(X * X + Y * Y);
        public float SqrLength => X * X + Y * Y;

        public Vector2D Normalized
        {
            get
            {
                float len = Length;
                if (len > CoreMath.Epsilon)
                    return new Vector2D(X / len, Y / len);
                return Zero;
            }
        }

        public static Vector2D operator +(Vector2D a, Vector2D b) => new Vector2D(a.X + b.X, a.Y + b.Y);
        public static Vector2D operator -(Vector2D a, Vector2D b) => new Vector2D(a.X - b.X, a.Y - b.Y);
        public static Vector2D operator *(Vector2D a, float scalar) => new Vector2D(a.X * scalar, a.Y * scalar);
        public static Vector2D operator /(Vector2D a, float scalar) => new Vector2D(a.X / scalar, a.Y / scalar);
        public static Vector2D operator -(Vector2D a) => new Vector2D(-a.X, -a.Y);

        public static float Dot(Vector2D a, Vector2D b) => a.X * b.X + a.Y * b.Y;
        public static float Distance(Vector2D a, Vector2D b) => (a - b).Length;
        public static float SqrDistance(Vector2D a, Vector2D b) => (a - b).SqrLength;

        public static Vector2D Lerp(Vector2D a, Vector2D b, float t)
        {
            t = CoreMath.Clamp01(t);
            return new Vector2D(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t);
        }

        public bool Equals(Vector2D other) => MathF.Abs(X - other.X) < CoreMath.Epsilon && MathF.Abs(Y - other.Y) < CoreMath.Epsilon;
        public override bool Equals(object obj) => obj is Vector2D other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(X, Y);
        public override string ToString() => $"({X:F2}, {Y:F2})";
    }

    public struct Vector3D : IEquatable<Vector3D>
    {
        public float X;
        public float Y;
        public float Z;

        public static readonly Vector3D Zero = new Vector3D(0f, 0f, 0f);
        public static readonly Vector3D One = new Vector3D(1f, 1f, 1f);
        public static readonly Vector3D Forward = new Vector3D(0f, 0f, 1f);
        public static readonly Vector3D Back = new Vector3D(0f, 0f, -1f);
        public static readonly Vector3D Up = new Vector3D(0f, 1f, 0f);
        public static readonly Vector3D Down = new Vector3D(0f, -1f, 0f);
        public static readonly Vector3D Right = new Vector3D(1f, 0f, 0f);
        public static readonly Vector3D Left = new Vector3D(-1f, 0f, 0f);

        public Vector3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float Length => MathF.Sqrt(X * X + Y * Y + Z * Z);
        public float SqrLength => X * X + Y * Y + Z * Z;

        public Vector3D Normalized
        {
            get
            {
                float len = Length;
                if (len > CoreMath.Epsilon)
                    return new Vector3D(X / len, Y / len, Z / len);
                return Zero;
            }
        }

        public static Vector3D operator +(Vector3D a, Vector3D b) => new Vector3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vector3D operator -(Vector3D a, Vector3D b) => new Vector3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Vector3D operator *(Vector3D a, float scalar) => new Vector3D(a.X * scalar, a.Y * scalar, a.Z * scalar);
        public static Vector3D operator /(Vector3D a, float scalar) => new Vector3D(a.X / scalar, a.Y / scalar, a.Z / scalar);
        public static Vector3D operator -(Vector3D a) => new Vector3D(-a.X, -a.Y, -a.Z);

        public static float Dot(Vector3D a, Vector3D b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

        public static Vector3D Cross(Vector3D a, Vector3D b)
        {
            return new Vector3D(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X
            );
        }

        public static float Distance(Vector3D a, Vector3D b) => (a - b).Length;
        public static float SqrDistance(Vector3D a, Vector3D b) => (a - b).SqrLength;

        public static Vector3D Lerp(Vector3D a, Vector3D b, float t)
        {
            t = CoreMath.Clamp01(t);
            return new Vector3D(
                a.X + (b.X - a.X) * t,
                a.Y + (b.Y - a.Y) * t,
                a.Z + (b.Z - a.Z) * t
            );
        }

        public bool Equals(Vector3D other) => MathF.Abs(X - other.X) < CoreMath.Epsilon && MathF.Abs(Y - other.Y) < CoreMath.Epsilon && MathF.Abs(Z - other.Z) < CoreMath.Epsilon;
        public override bool Equals(object obj) => obj is Vector3D other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(X, Y, Z);
        public override string ToString() => $"({X:F2}, {Y:F2}, {Z:F2})";
    }
}
