using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ostryzhnyi.DI
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class InjectAttribute : Attribute
    {
    }

    public class DIContainer
    {
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public void Register<TService>(TService implementation)
        {
            var type = typeof(TService);
            if (!_services.TryAdd(type, implementation))
            {
                throw new InvalidOperationException($"{type.Name} is already registered.");
            }
        }

        public void Register<TService>(Func<TService> factory)
        {
            var type = typeof(TService);
            if (_services.ContainsKey(type))
            {
                throw new InvalidOperationException($"{type.Name} is already registered.");
            }
            _services[type] = factory();
        }
        
        public object Resolve(Type type)
        {
            if (!_services.TryGetValue(type, out var service))
            {
                throw new InvalidOperationException($"{type.Name} is not registered.");
            }
            return service;
        }

        public TService Resolve<TService>()
        {
            var type = typeof(TService);
            if (!_services.TryGetValue(type, out var service))
            {
                throw new InvalidOperationException($"{type.Name} is not registered.");
            }
            return (TService)service;
        }

        public bool TryResolve(Type type, out object dependency)
        {
            if (_services.TryGetValue(type, out var service))
            {
                dependency = service;
                return true;
            }

            dependency = default;
            return false;
        }

        public void InjectDependencies(Object target)
        {
            var targetType = target.GetType();

            foreach (var field in targetType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                         .Where(f => f.IsDefined(typeof(InjectAttribute), false)))
            {
                if(TryResolve(field.FieldType, out var dependency))
                {
                    field.SetValue(target, dependency);
                }
            }

            foreach (var property in targetType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                         .Where(p => p.IsDefined(typeof(InjectAttribute), false) && p.CanWrite))
            {
                if(TryResolve(property.PropertyType, out var dependency))
                {
                    property.SetValue(target, dependency);
                }
            }
        }
    }
}