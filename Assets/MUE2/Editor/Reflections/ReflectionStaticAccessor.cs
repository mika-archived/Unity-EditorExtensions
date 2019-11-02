using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MUE2.Editor.Reflections
{
    public class ReflectionStaticAccessor
    {
        private readonly Type _class;
        private readonly Dictionary<string, Func<object[], object>> _methodCaches;
        private readonly Dictionary<string, Func<object>> _propertyCaches;

        protected ReflectionStaticAccessor(Type @class)
        {
            _class = @class;
            _methodCaches = new Dictionary<string, Func<object[], object>>();
            _propertyCaches = new Dictionary<string, Func<object>>();
        }

        protected T CallMethod<T>(string name, BindingFlags bindingFlags, params object[] parameters) where T : class
        {
            if (_methodCaches.ContainsKey(name))
                return _methodCaches[name].Invoke(parameters) as T;

            var method = _class.GetMethod(name, bindingFlags);
            if (method == null)
                throw new InvalidOperationException(string.Format("Method '{0}' is not found in this instance", name));
            _methodCaches.Add(name, CreateMethodAccessor(method));

            return _methodCaches[name].Invoke(parameters) as T;
        }

        protected T AccessProperty<T>(string name, BindingFlags bindingFlags) where T : class
        {
            if (_propertyCaches.ContainsKey(name))
                return _propertyCaches[name].Invoke() as T;

            var property = _class.GetProperty(name, bindingFlags);
            if (property == null)
                throw new InvalidOperationException(string.Format("Property '{0}' is not found in this instance", name));
            _propertyCaches.Add(name, CreatePropertyAccessor(property));

            return _propertyCaches[name].Invoke() as T;
        }

        private Func<object[], object> CreateMethodAccessor(MethodInfo mi)
        {
            var args = Expression.Parameter(typeof(object[]), "args");

            if (mi.GetParameters().Length == 0)
            {
                // (params object[] args) => (object) Class.Method();
                var body = Expression.Convert(Expression.Call(null, mi), typeof(object));
                return Expression.Lambda<Func<object[], object>>(body, args).Compile();
            }
            else
            {
                // (S instance, params object[] args) => (object) instance.Method((T1) args[0], ...);
                // (object instance, params object[] args) => (object) ((isa) instance).Method((T1) args[0], ...);
                var parameters = mi.GetParameters().Select((w, i) => Expression.Convert(Expression.ArrayIndex(args, Expression.Constant(i)), w.ParameterType)).Cast<Expression>().ToArray();
                var body = Expression.Convert(Expression.Call(null, mi, parameters), typeof(object));
                return Expression.Lambda<Func<object[], object>>(body, args).Compile();
            }
        }

        private Func<object> CreatePropertyAccessor(PropertyInfo pi)
        {
            // (S instance) => (object) Class.Property;
            var body = Expression.Convert(Expression.Property(null, pi), typeof(object));
            return Expression.Lambda<Func<object>>(body).Compile();
        }
    }
}