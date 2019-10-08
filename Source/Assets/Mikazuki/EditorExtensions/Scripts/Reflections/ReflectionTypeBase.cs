using System;
using System.Collections.Generic;
using System.Reflection;

namespace Assets.Mikazuki.EditorExtensions.Scripts.Reflections
{
    public class ReflectionTypeBase
    {
        private readonly Dictionary<string, MethodInfo> _caches;
        private readonly object _instance;

        protected ReflectionTypeBase(object instance)
        {
            _instance = instance;
            _caches = new Dictionary<string, MethodInfo>();
        }

        protected T CallMethod<T>(string name, BindingFlags bindingFlags, params object[] parameters) where T : class
        {
            if (_caches.ContainsKey(name))
                return _caches[name].Invoke(_instance, parameters) as T;

            var method = _instance.GetType().GetMethod(name, bindingFlags);
            if (method == null)
                throw new InvalidOperationException(string.Format("Method '{0}' is not found in this instance", name));
            _caches[name] = method;

            return _caches[name].Invoke(_instance, parameters) as T;
        }

        protected bool CallMethodAsBool(string name, BindingFlags bindingFlags, params object[] parameters)
        {
            return (bool) CallMethod<object>(name, bindingFlags, parameters);
        }
    }
}