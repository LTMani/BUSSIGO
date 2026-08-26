using System;
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
