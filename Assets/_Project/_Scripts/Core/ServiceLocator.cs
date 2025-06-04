using System;
using System.Collections.Generic;
using UnityEngine;

namespace HairvestMoon.Core
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public static void Register<T>(T service)
        {
            var type = typeof(T);
            if (_services.ContainsKey(type))
            {
                Debug.LogWarning($"ServiceLocator: Service {type} is already registered. Overwriting.");
            }
            _services[type] = service;
        }

        public static T Get<T>()
        {
            var type = typeof(T);
            if (_services.TryGetValue(type, out var service))
            {
                return (T)service;
            }
            Debug.LogError($"ServiceLocator: Service {type} not found.");
            return default;
        }

        public static void Clear()
        {
            _services.Clear();
        }
    }

}
