#!/usr/bin/env python3
"""
BUSSIGO Engine Codebase Generator - Phase 1: Core Engine Architecture & Spatial Math
Generates production-grade C# code files for Assets/Game/Core/
"""

import os
from pathlib import Path

BASE_DIR = Path("Assets/Game/Core")
BASE_DIR.mkdir(parents=True, exist_ok=True)

FILES = {}

FILES["CoreMath.cs"] = """using System;

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
"""

FILES["VectorMath.cs"] = """using System;

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
"""

FILES["SplineMath.cs"] = """using System;

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
"""

FILES["KalmanFilter.cs"] = """using System;

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
"""

FILES["GeoMath.cs"] = """using System;

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
"""

FILES["ServiceLocator.cs"] = """using System;
using System.Collections.Generic;

namespace Bussigo.Game.Core
{
    public interface IServiceLocator
    {
        void Register<T>(T service) where T : class;
        void Register<TInterface, TImplementation>(TImplementation service) where TImplementation : class, TInterface;
        T Get<T>() where T : class;
        bool TryGet<T>(out T service) where T : class;
        void Unregister<T>() where T : class;
        void Clear();
    }

    public class ServiceLocator : IServiceLocator
    {
        private static IServiceLocator _instance;
        public static IServiceLocator Instance => _instance ??= new ServiceLocator();

        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        private readonly object _lock = new object();

        public void Register<T>(T service) where T : class
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            lock (_lock)
            {
                _services[typeof(T)] = service;
            }
        }

        public void Register<TInterface, TImplementation>(TImplementation service) where TImplementation : class, TInterface
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            lock (_lock)
            {
                _services[typeof(TInterface)] = service;
            }
        }

        public T Get<T>() where T : class
        {
            lock (_lock)
            {
                if (_services.TryGetValue(typeof(T), out object service))
                {
                    return (T)service;
                }
                throw new KeyNotFoundException($"Service of type {typeof(T).FullName} is not registered in ServiceLocator.");
            }
        }

        public bool TryGet<T>(out T service) where T : class
        {
            lock (_lock)
            {
                if (_services.TryGetValue(typeof(T), out object obj))
                {
                    service = (T)obj;
                    return true;
                }
                service = null;
                return false;
            }
        }

        public void Unregister<T>() where T : class
        {
            lock (_lock)
            {
                _services.Remove(typeof(T));
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _services.Clear();
            }
        }
    }
}
"""

FILES["EventBus.cs"] = """using System;
using System.Collections.Generic;

namespace Bussigo.Game.Core
{
    public interface IGameEvent { }

    public interface IEventBus
    {
        void Subscribe<T>(Action<T> handler) where T : IGameEvent;
        void Unsubscribe<T>(Action<T> handler) where T : IGameEvent;
        void Publish<T>(T gameEvent) where T : IGameEvent;
        void Clear();
    }

    public class EventBus : IEventBus
    {
        private readonly Dictionary<Type, List<Delegate>> _subscribers = new Dictionary<Type, List<Delegate>>();
        private readonly object _lock = new object();

        public void Subscribe<T>(Action<T> handler) where T : IGameEvent
        {
            if (handler == null) return;
            lock (_lock)
            {
                Type type = typeof(T);
                if (!_subscribers.TryGetValue(type, out List<Delegate> list))
                {
                    list = new List<Delegate>();
                    _subscribers[type] = list;
                }
                if (!list.Contains(handler))
                {
                    list.Add(handler);
                }
            }
        }

        public void Unsubscribe<T>(Action<T> handler) where T : IGameEvent
        {
            if (handler == null) return;
            lock (_lock)
            {
                Type type = typeof(T);
                if (_subscribers.TryGetValue(type, out List<Delegate> list))
                {
                    list.Remove(handler);
                    if (list.Count == 0)
                    {
                        _subscribers.Remove(type);
                    }
                }
            }
        }

        public void Publish<T>(T gameEvent) where T : IGameEvent
        {
            if (gameEvent == null) return;
            List<Delegate> handlersCopy;
            lock (_lock)
            {
                Type type = typeof(T);
                if (!_subscribers.TryGetValue(type, out List<Delegate> list))
                    return;
                handlersCopy = new List<Delegate>(list);
            }

            foreach (var handler in handlersCopy)
            {
                try
                {
                    ((Action<T>)handler).Invoke(gameEvent);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[EventBus] Error handling event {typeof(T).Name}: {ex}");
                }
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _subscribers.Clear();
            }
        }
    }
}
"""

FILES["StateMachine.cs"] = """using System;
using System.Collections.Generic;

namespace Bussigo.Game.Core
{
    public interface IState
    {
        void OnEnter();
        void OnUpdate(float deltaTime);
        void OnExit();
    }

    public class StateMachine
    {
        private readonly Dictionary<Type, IState> _states = new Dictionary<Type, IState>();
        private IState _currentState;
        public IState CurrentState => _currentState;
        public Type CurrentStateType => _currentState?.GetType();

        public event Action<IState, IState> OnStateChanged;

        public void RegisterState<T>(T state) where T : IState
        {
            _states[typeof(T)] = state;
        }

        public void ChangeState<T>() where T : IState
        {
            Type stateType = typeof(T);
            if (!_states.TryGetValue(stateType, out IState newState))
            {
                throw new KeyNotFoundException($"State {stateType.Name} not registered in StateMachine.");
            }

            IState previousState = _currentState;
            _currentState?.OnExit();
            _currentState = newState;
            _currentState.OnEnter();
            OnStateChanged?.Invoke(previousState, _currentState);
        }

        public void Update(float deltaTime)
        {
            _currentState?.OnUpdate(deltaTime);
        }
    }
}
"""

FILES["ObjectPool.cs"] = """using System;
using System.Collections.Generic;

namespace Bussigo.Game.Core
{
    public interface IPoolable
    {
        void OnSpawn();
        void OnDespawn();
    }

    public class ObjectPool<T> where T : class, new()
    {
        private readonly Stack<T> _pool = new Stack<T>();
        private readonly Func<T> _factory;
        private readonly Action<T> _onSpawn;
        private readonly Action<T> _onDespawn;
        private readonly int _maxCapacity;

        public int TotalCreated { get; private set; }
        public int InPoolCount => _pool.Count;

        public ObjectPool(int initialCapacity = 16, int maxCapacity = 512, Func<T> factory = null, Action<T> onSpawn = null, Action<T> onDespawn = null)
        {
            _factory = factory ?? (() => new T());
            _onSpawn = onSpawn;
            _onDespawn = onDespawn;
            _maxCapacity = maxCapacity;

            for (int i = 0; i < initialCapacity; i++)
            {
                T item = _factory();
                TotalCreated++;
                _pool.Push(item);
            }
        }

        public T Rent()
        {
            T item;
            if (_pool.Count > 0)
            {
                item = _pool.Pop();
            }
            else
            {
                item = _factory();
                TotalCreated++;
            }

            if (item is IPoolable poolable)
            {
                poolable.OnSpawn();
            }
            _onSpawn?.Invoke(item);
            return item;
        }

        public void Return(T item)
        {
            if (item == null) return;

            if (item is IPoolable poolable)
            {
                poolable.OnDespawn();
            }
            _onDespawn?.Invoke(item);

            if (_pool.Count < _maxCapacity)
            {
                _pool.Push(item);
            }
        }
    }
}
"""

FILES["GameClock.cs"] = """using System;

namespace Bussigo.Game.Core
{
    public class GameClock
    {
        public float TimeOfDaySeconds { get; private set; } // 0 to 86400 (24h)
        public float TimeScale { get; set; } = 1.0f;
        public bool IsPaused { get; set; } = false;
        public int DayCount { get; private set; } = 1;

        public const float SecondsInDay = 86400.0f;

        public int Hours => (int)(TimeOfDaySeconds / 3600.0f) % 24;
        public int Minutes => (int)((TimeOfDaySeconds % 3600.0f) / 60.0f);
        public int Seconds => (int)(TimeOfDaySeconds % 60.0f);

        public string FormattedTime => $"{Hours:D2}:{Minutes:D2}:{Seconds:D2}";

        public GameClock(float initialHour = 6.0f)
        {
            TimeOfDaySeconds = initialHour * 3600.0f;
        }

        public void Advance(float deltaRealSeconds)
        {
            if (IsPaused) return;

            TimeOfDaySeconds += deltaRealSeconds * TimeScale;
            while (TimeOfDaySeconds >= SecondsInDay)
            {
                TimeOfDaySeconds -= SecondsInDay;
                DayCount++;
            }
            while (TimeOfDaySeconds < 0.0f)
            {
                TimeOfDaySeconds += SecondsInDay;
                DayCount = Math.Max(1, DayCount - 1);
            }
        }

        public void SetTime(float hour, float minute = 0f)
        {
            TimeOfDaySeconds = CoreMath.Clamp(hour * 3600f + minute * 60f, 0f, SecondsInDay);
        }
    }
}
"""

FILES["GameConfiguration.cs"] = """using System;
using System.Collections.Generic;

namespace Bussigo.Game.Core
{
    public enum GamePlatformMode
    {
        PC,
        Mobile,
        Console
    }

    public enum GraphicsQualityTier
    {
        Low,
        Medium,
        High,
        Ultra
    }

    public class GameConfiguration
    {
        public static GameConfiguration Active { get; set; } = new GameConfiguration();

        public GamePlatformMode PlatformMode { get; set; } = GamePlatformMode.PC;
        public GraphicsQualityTier QualityTier { get; set; } = GraphicsQualityTier.High;
        public string ActiveLanguage { get; set; } = "en";

        public bool EnableTrafficAI { get; set; } = true;
        public int MaxTrafficDensity { get; set; } = 64;
        public bool EnableDynamicWeather { get; set; } = true;
        public bool EnableForceFeedback { get; set; } = false;
        public float MasterAudioVolume { get; set; } = 1.0f;
        public float EngineAudioVolume { get; set; } = 0.9f;
        public float AmbienceAudioVolume { get; set; } = 0.7f;
        public float HornAudioVolume { get; set; } = 1.0f;
        public float VoiceAudioVolume { get; set; } = 0.85f;

        public bool IsMetricUnits { get; set; } = true;
        public float SteeringSensitivity { get; set; } = 1.0f;
        public float SteeringSmoothing { get; set; } = 0.15f;
        public bool AutomaticTransmission { get; set; } = false;
        public bool ABSBrakingAssist { get; set; } = true;
        public bool CruiseControlEnabled { get; set; } = true;
    }
}
"""

for fname, content in FILES.items():
    fpath = BASE_DIR / fname
    with open(fpath, "w", encoding="utf-8") as f:
        f.write(content.strip() + "\n")
    print(f"Generated: {fpath}")

print("Phase 1 generation complete.")
