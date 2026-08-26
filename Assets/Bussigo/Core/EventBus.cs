using System;
using System.Collections.Generic;

namespace Bussigo.Core
{
    /// <summary>
    /// Decoupled, generic publish-subscribe Event Bus for inter-system communication.
    /// </summary>
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> subscribers = new Dictionary<Type, List<Delegate>>();

        public static void Subscribe<T>(Action<T> handler) where T : IGameEvent
        {
            Type eventType = typeof(T);
            if (!subscribers.ContainsKey(eventType))
            {
                subscribers[eventType] = new List<Delegate>();
            }
            if (!subscribers[eventType].Contains(handler))
            {
                subscribers[eventType].Add(handler);
            }
        }

        public static void Unsubscribe<T>(Action<T> handler) where T : IGameEvent
        {
            Type eventType = typeof(T);
            if (subscribers.TryGetValue(eventType, out List<Delegate> handlers))
            {
                handlers.Remove(handler);
                if (handlers.Count == 0)
                {
                    subscribers.Remove(eventType);
                }
            }
        }

        public static void Publish<T>(T gameEvent) where T : IGameEvent
        {
            Type eventType = typeof(T);
            if (subscribers.TryGetValue(eventType, out List<Delegate> handlers))
            {
                // Iterate on a copy to allow handlers to safely unsubscribe during invocation
                var handlersCopy = new List<Delegate>(handlers);
                foreach (var handler in handlersCopy)
                {
                    if (handler is Action<T> action)
                    {
                        action.Invoke(gameEvent);
                    }
                }
            }
        }

        public static void Clear()
        {
            subscribers.Clear();
        }
    }
}
