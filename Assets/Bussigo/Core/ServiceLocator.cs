using System;
using System.Collections.Generic;

namespace Bussigo.Core
{
    /// <summary>
    /// Type-safe, decoupled service locator for core system dependencies.
    /// </summary>
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, IService> services = new Dictionary<Type, IService>();

        public static void Register<T>(T service) where T : class, IService
        {
            Type type = typeof(T);
            if (services.ContainsKey(type))
            {
                services[type].Shutdown();
                services[type] = service;
            }
            else
            {
                services.Add(type, service);
            }
            service.Initialize();
        }

        public static T Get<T>() where T : class, IService
        {
            Type type = typeof(T);
            if (services.TryGetValue(type, out IService service))
            {
                return service as T;
            }
            throw new InvalidOperationException($"[ServiceLocator] Service of type '{type.Name}' is not registered.");
        }

        public static bool TryGet<T>(out T service) where T : class, IService
        {
            Type type = typeof(T);
            if (services.TryGetValue(type, out IService registeredService))
            {
                service = registeredService as T;
                return true;
            }
            service = null;
            return false;
        }

        public static void Unregister<T>() where T : class, IService
        {
            Type type = typeof(T);
            if (services.TryGetValue(type, out IService service))
            {
                service.Shutdown();
                services.Remove(type);
            }
        }

        public static void Reset()
        {
            foreach (var kvp in services)
            {
                kvp.Value.Shutdown();
            }
            services.Clear();
        }
    }
}
