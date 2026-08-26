using System;

namespace Bussigo.Game.Core
{
    public struct ComplexNumber
    {
        public float Real;
        public float Imaginary;

        public ComplexNumber(float real, float imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public float Magnitude => MathF.Sqrt(Real * Real + Imaginary * Imaginary);
        public float Phase => MathF.Atan2(Imaginary, Real);

        public static ComplexNumber operator +(ComplexNumber a, ComplexNumber b) => new ComplexNumber(a.Real + b.Real, a.Imaginary + b.Imaginary);
        public static ComplexNumber operator -(ComplexNumber a, ComplexNumber b) => new ComplexNumber(a.Real - b.Real, a.Imaginary - b.Imaginary);
        public static ComplexNumber operator *(ComplexNumber a, ComplexNumber b) => new ComplexNumber(a.Real * b.Real - a.Imaginary * b.Imaginary, a.Real * b.Imaginary + a.Imaginary * b.Real);
        public static ComplexNumber operator *(ComplexNumber a, float scalar) => new ComplexNumber(a.Real * scalar, a.Imaginary * scalar);
    }

    public static class FastFourierTransform
    {
        public static void ForwardFFT(ComplexNumber[] buffer)
        {
            int n = buffer.Length;
            if ((n & (n - 1)) != 0)
                throw new ArgumentException("FFT buffer length must be a power of 2.");

            int j = 0;
            for (int i = 0; i < n - 1; i++)
            {
                if (i < j)
                {
                    ComplexNumber temp = buffer[i];
                    buffer[i] = buffer[j];
                    buffer[j] = temp;
                }
                int k = n >> 1;
                while (k <= j)
                {
                    j -= k;
                    k >>= 1;
                }
                j += k;
            }

            for (int len = 2; len <= n; len <<= 1)
            {
                float angle = -2.0f * MathF.PI / len;
                ComplexNumber wlen = new ComplexNumber(MathF.Cos(angle), MathF.Sin(angle));

                for (int i = 0; i < n; i += len)
                {
                    ComplexNumber w = new ComplexNumber(1.0f, 0.0f);
                    for (int k = 0; k < len / 2; k++)
                    {
                        ComplexNumber u = buffer[i + k];
                        ComplexNumber v = buffer[i + k + len / 2] * w;
                        buffer[i + k] = u + v;
                        buffer[i + k + len / 2] = u - v;
                        w = w * wlen;
                    }
                }
            }
        }
    }
}
