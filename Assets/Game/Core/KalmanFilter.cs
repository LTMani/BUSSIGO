using System;

namespace Bussigo.Game.Core
{
    public class KalmanFilter1D
    {
        private float _q; // Process noise covariance
        private float _r; // Measurement noise covariance
        private float _x; // Value estimate
        private float _p; // Estimation error covariance
        private float _k; // Kalman gain

        public KalmanFilter1D(float processNoise = 0.05f, float measurementNoise = 0.8f, float initialEstimate = 0.0f)
        {
            _q = processNoise;
            _r = measurementNoise;
            _x = initialEstimate;
            _p = 1.0f;
        }

        public float Update(float measurement)
        {
            // Prediction update
            _p = _p + _q;

            // Measurement update
            _k = _p / (_p + _r);
            _x = _x + _k * (measurement - _x);
            _p = (1.0f - _k) * _p;

            return _x;
        }

        public void Reset(float value = 0.0f)
        {
            _x = value;
            _p = 1.0f;
        }

        public float State => _x;
    }
}
