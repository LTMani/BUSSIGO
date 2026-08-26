using System;
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
