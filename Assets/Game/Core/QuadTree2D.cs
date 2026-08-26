using System;
using System.Collections.Generic;

namespace Bussigo.Game.Core
{
    public struct Rect2D
    {
        public float XMin;
        public float ZMin;
        public float XMax;
        public float ZMax;

        public Rect2D(float xMin, float zMin, float xMax, float zMax)
        {
            XMin = xMin;
            ZMin = zMin;
            XMax = xMax;
            ZMax = zMax;
        }

        public bool Contains(Vector3D point)
        {
            return point.X >= XMin && point.X <= XMax && point.Z >= ZMin && point.Z <= ZMax;
        }

        public bool Intersects(Rect2D other)
        {
            return !(other.XMin > XMax || other.XMax < XMin || other.ZMin > ZMax || other.ZMax < ZMin);
        }
    }

    public class QuadTree2D<T> where T : class
    {
        private const int MaxObjects = 8;
        private const int MaxLevels = 6;

        private readonly int _level;
        private readonly Rect2D _bounds;
        private readonly List<(T Item, Vector3D Position)> _objects = new List<(T, Vector3D)>();
        private QuadTree2D<T>[] _children;

        public QuadTree2D(Rect2D bounds, int level = 0)
        {
            _bounds = bounds;
            _level = level;
        }

        public void Insert(T item, Vector3D position)
        {
            if (!_bounds.Contains(position)) return;

            if (_children != null)
            {
                int index = GetChildIndex(position);
                if (index != -1)
                {
                    _children[index].Insert(item, position);
                    return;
                }
            }

            _objects.Add((item, position));

            if (_objects.Count > MaxObjects && _level < MaxLevels && _children == null)
            {
                Subdivide();
                for (int i = _objects.Count - 1; i >= 0; i--)
                {
                    int index = GetChildIndex(_objects[i].Position);
                    if (index != -1)
                    {
                        _children[index].Insert(_objects[i].Item, _objects[i].Position);
                        _objects.RemoveAt(i);
                    }
                }
            }
        }

        public void QueryRange(Rect2D range, List<T> results)
        {
            if (!_bounds.Intersects(range)) return;

            foreach (var obj in _objects)
            {
                if (range.Contains(obj.Position))
                {
                    results.Add(obj.Item);
                }
            }

            if (_children != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    _children[i].QueryRange(range, results);
                }
            }
        }

        private void Subdivide()
        {
            float midX = (_bounds.XMin + _bounds.XMax) * 0.5f;
            float midZ = (_bounds.ZMin + _bounds.ZMax) * 0.5f;

            _children = new QuadTree2D<T>[4];
            _children[0] = new QuadTree2D<T>(new Rect2D(_bounds.XMin, _bounds.ZMin, midX, midZ), _level + 1); // SW
            _children[1] = new QuadTree2D<T>(new Rect2D(midX, _bounds.ZMin, _bounds.XMax, midZ), _level + 1); // SE
            _children[2] = new QuadTree2D<T>(new Rect2D(_bounds.XMin, midZ, midX, _bounds.ZMax), _level + 1); // NW
            _children[3] = new QuadTree2D<T>(new Rect2D(midX, midZ, _bounds.XMax, _bounds.ZMax), _level + 1); // NE
        }

        private int GetChildIndex(Vector3D pos)
        {
            float midX = (_bounds.XMin + _bounds.XMax) * 0.5f;
            float midZ = (_bounds.ZMin + _bounds.ZMax) * 0.5f;

            bool isNorth = pos.Z >= midZ;
            bool isEast = pos.X >= midX;

            if (!isNorth && !isEast) return 0; // SW
            if (!isNorth && isEast) return 1;  // SE
            if (isNorth && !isEast) return 2;  // NW
            if (isNorth && isEast) return 3;   // NE
            return -1;
        }

        public void Clear()
        {
            _objects.Clear();
            if (_children != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    _children[i].Clear();
                }
                _children = null;
            }
        }
    }
}
