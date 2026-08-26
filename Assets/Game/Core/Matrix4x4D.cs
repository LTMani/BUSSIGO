using System;

namespace Bussigo.Game.Core
{
    public struct Matrix4x4D : IEquatable<Matrix4x4D>
    {
        public float M00, M01, M02, M03;
        public float M10, M11, M12, M13;
        public float M20, M21, M22, M23;
        public float M30, M31, M32, M33;

        public static readonly Matrix4x4D Identity = new Matrix4x4D(
            1f, 0f, 0f, 0f,
            0f, 1f, 0f, 0f,
            0f, 0f, 1f, 0f,
            0f, 0f, 0f, 1f
        );

        public Matrix4x4D(
            float m00, float m01, float m02, float m03,
            float m10, float m11, float m12, float m13,
            float m20, float m21, float m22, float m23,
            float m30, float m31, float m32, float m33)
        {
            M00 = m00; M01 = m01; M02 = m02; M03 = m03;
            M10 = m10; M11 = m11; M12 = m12; M13 = m13;
            M20 = m20; M21 = m21; M22 = m22; M23 = m23;
            M30 = m30; M31 = m31; M32 = m32; M33 = m33;
        }

        public static Matrix4x4D CreateTranslation(Vector3D translation)
        {
            return new Matrix4x4D(
                1f, 0f, 0f, translation.X,
                0f, 1f, 0f, translation.Y,
                0f, 0f, 1f, translation.Z,
                0f, 0f, 0f, 1f
            );
        }

        public static Matrix4x4D CreateRotationY(float radians)
        {
            float cos = MathF.Cos(radians);
            float sin = MathF.Sin(radians);
            return new Matrix4x4D(
                cos, 0f, sin, 0f,
                0f, 1f, 0f, 0f,
                -sin, 0f, cos, 0f,
                0f, 0f, 0f, 1f
            );
        }

        public static Matrix4x4D CreateRotationX(float radians)
        {
            float cos = MathF.Cos(radians);
            float sin = MathF.Sin(radians);
            return new Matrix4x4D(
                1f, 0f, 0f, 0f,
                0f, cos, -sin, 0f,
                0f, sin, cos, 0f,
                0f, 0f, 0f, 1f
            );
        }

        public static Matrix4x4D CreateRotationZ(float radians)
        {
            float cos = MathF.Cos(radians);
            float sin = MathF.Sin(radians);
            return new Matrix4x4D(
                cos, -sin, 0f, 0f,
                sin, cos, 0f, 0f,
                0f, 0f, 1f, 0f,
                0f, 0f, 0f, 1f
            );
        }

        public static Matrix4x4D CreateScale(Vector3D scale)
        {
            return new Matrix4x4D(
                scale.X, 0f, 0f, 0f,
                0f, scale.Y, 0f, 0f,
                0f, 0f, scale.Z, 0f,
                0f, 0f, 0f, 1f
            );
        }

        public static Matrix4x4D operator *(Matrix4x4D a, Matrix4x4D b)
        {
            return new Matrix4x4D(
                a.M00 * b.M00 + a.M01 * b.M10 + a.M02 * b.M20 + a.M03 * b.M30,
                a.M00 * b.M01 + a.M01 * b.M11 + a.M02 * b.M21 + a.M03 * b.M31,
                a.M00 * b.M02 + a.M01 * b.M12 + a.M02 * b.M22 + a.M03 * b.M32,
                a.M00 * b.M03 + a.M01 * b.M13 + a.M02 * b.M23 + a.M03 * b.M33,

                a.M10 * b.M00 + a.M11 * b.M10 + a.M12 * b.M20 + a.M13 * b.M30,
                a.M10 * b.M01 + a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31,
                a.M10 * b.M02 + a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32,
                a.M10 * b.M03 + a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33,

                a.M20 * b.M00 + a.M21 * b.M10 + a.M22 * b.M20 + a.M23 * b.M30,
                a.M20 * b.M01 + a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31,
                a.M20 * b.M02 + a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32,
                a.M20 * b.M03 + a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33,

                a.M30 * b.M00 + a.M31 * b.M10 + a.M32 * b.M20 + a.M33 * b.M30,
                a.M30 * b.M01 + a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31,
                a.M30 * b.M02 + a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32,
                a.M30 * b.M03 + a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33
            );
        }

        public Vector3D TransformPoint(Vector3D point)
        {
            float x = M00 * point.X + M01 * point.Y + M02 * point.Z + M03;
            float y = M10 * point.X + M11 * point.Y + M12 * point.Z + M13;
            float z = M20 * point.X + M21 * point.Y + M22 * point.Z + M23;
            float w = M30 * point.X + M31 * point.Y + M32 * point.Z + M33;

            if (MathF.Abs(w - 1.0f) > CoreMath.Epsilon && MathF.Abs(w) > CoreMath.Epsilon)
            {
                return new Vector3D(x / w, y / w, z / w);
            }
            return new Vector3D(x, y, z);
        }

        public Vector3D TransformDirection(Vector3D dir)
        {
            float x = M00 * dir.X + M01 * dir.Y + M02 * dir.Z;
            float y = M10 * dir.X + M11 * dir.Y + M12 * dir.Z;
            float z = M20 * dir.X + M21 * dir.Y + M22 * dir.Z;
            return new Vector3D(x, y, z);
        }

        public bool Equals(Matrix4x4D other)
        {
            return MathF.Abs(M00 - other.M00) < CoreMath.Epsilon &&
                   MathF.Abs(M11 - other.M11) < CoreMath.Epsilon &&
                   MathF.Abs(M22 - other.M22) < CoreMath.Epsilon &&
                   MathF.Abs(M33 - other.M33) < CoreMath.Epsilon;
        }

        public override bool Equals(object obj) => obj is Matrix4x4D other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(M00, M11, M22, M33);
    }
}
