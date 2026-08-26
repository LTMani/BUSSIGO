using System;
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
