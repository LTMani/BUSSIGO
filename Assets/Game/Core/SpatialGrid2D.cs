using System;
using System.Collections.Generic;

namespace Bussigo.Game.Core
{
    public class SpatialGrid2D<T> where T : class
    {
        private readonly float _cellSize;
        private readonly Dictionary<long, List<T>> _grid = new Dictionary<long, List<T>>();
        private readonly Dictionary<T, long> _itemKeys = new Dictionary<T, long>();

        public SpatialGrid2D(float cellSize = 50.0f)
        {
            _cellSize = MathF.Max(5.0f, cellSize);
        }

        private long GetCellKey(float x, float z)
        {
            int cellX = (int)MathF.Floor(x / _cellSize);
            int cellZ = (int)MathF.Floor(z / _cellSize);
            return ((long)cellX << 32) | (uint)cellZ;
        }

        public void Insert(T item, Vector3D position)
        {
            if (item == null) return;
            long key = GetCellKey(position.X, position.Z);

            if (!_grid.TryGetValue(key, out var list))
            {
                list = new List<T>();
                _grid[key] = list;
            }
            list.Add(item);
            _itemKeys[item] = key;
        }

        public void UpdatePosition(T item, Vector3D newPosition)
        {
            if (item == null) return;
            long newKey = GetCellKey(newPosition.X, newPosition.Z);

            if (_itemKeys.TryGetValue(item, out long oldKey))
            {
                if (oldKey == newKey) return; // Same cell

                if (_grid.TryGetValue(oldKey, out var oldList))
                {
                    oldList.Remove(item);
                }
            }

            if (!_grid.TryGetValue(newKey, out var newList))
            {
                newList = new List<T>();
                _grid[newKey] = newList;
            }
            newList.Add(item);
            _itemKeys[item] = newKey;
        }

        public void Remove(T item)
        {
            if (item == null) return;
            if (_itemKeys.TryGetValue(item, out long key))
            {
                if (_grid.TryGetValue(key, out var list))
                {
                    list.Remove(item);
                }
                _itemKeys.Remove(item);
            }
        }

        public List<T> QueryRadius(Vector3D center, float radius)
        {
            var results = new List<T>();
            int minCellX = (int)MathF.Floor((center.X - radius) / _cellSize);
            int maxCellX = (int)MathF.Floor((center.X + radius) / _cellSize);
            int minCellZ = (int)MathF.Floor((center.Z - radius) / _cellSize);
            int maxCellZ = (int)MathF.Floor((center.Z + radius) / _cellSize);

            float sqrRadius = radius * radius;

            for (int cx = minCellX; cx <= maxCellX; cx++)
            {
                for (int cz = minCellZ; cz <= maxCellZ; cz++)
                {
                    long key = ((long)cx << 32) | (uint)cz;
                    if (_grid.TryGetValue(key, out var list))
                    {
                        results.AddRange(list);
                    }
                }
            }
            return results;
        }

        public void Clear()
        {
            _grid.Clear();
            _itemKeys.Clear();
        }
    }
}
