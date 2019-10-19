using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MUE2.Editor.Reflections
{
    public class ReflectionAccessor<TS>
    {
        private readonly Dictionary<string, Func<TS, object>> _fieldCaches;
        private readonly Type _isa;
        private readonly Dictionary<string, Func<TS, object[], object>> _methodCaches;
        private readonly Dictionary<string, Func<TS, object>> _propertyCaches;

        protected TS Instance { get; private set; }

        protected ReflectionAccessor(TS instance)
        {
            Instance = instance;
            _fieldCaches = new Dictionary<string, Func<TS, object>>();
            _methodCaches = new Dictionary<string, Func<TS, object[], object>>();
            _propertyCaches = new Dictionary<string, Func<TS, object>>();
        }

        protected ReflectionAccessor(TS instance, Type isa)
        {
            Instance = instance;
            _isa = isa;
            _fieldCaches = new Dictionary<string, Func<TS, object>>();
            _methodCaches = new Dictionary<string, Func<TS, object[], object>>();
            _propertyCaches = new Dictionary<string, Func<TS, object>>();
        }

        protected T CallMethod<T>(string name, BindingFlags bindingFlags, params object[] parameters) where T : class
        {
            if (_methodCaches.ContainsKey(name))
                return _methodCaches[name].Invoke(Instance, parameters) as T;

            var method = Instance.GetType().GetMethod(name, bindingFlags);
            if (method == null)
                throw new InvalidOperationException(string.Format("Method '{0}' is not found in this instance", name));
            _methodCaches.Add(name, CreateMethodAccessor(method));

            return _methodCaches[name].Invoke(Instance, parameters) as T;
        }

        protected T CallMethodAs<T>(string name, BindingFlags bindingFlags, params object[] parameters) where T : struct
        {
            return (T) CallMethod<object>(name, bindingFlags, parameters);
        }

        protected T AccessField<T>(string name, BindingFlags bindingFlags) where T : class
        {
            if (_fieldCaches.ContainsKey(name))
                return _fieldCaches[name].Invoke(Instance) as T;

            var field = Instance.GetType().GetField(name, bindingFlags);
            if (field == null)
                throw new InvalidOperationException(string.Format("Field '{0}' is not found in this instance", name));
            _fieldCaches.Add(name, CreateFieldAccessor(field));

            return _fieldCaches[name].Invoke(Instance) as T;
        }

        protected T AccessProperty<T>(string name, BindingFlags bindingFlags) where T : class
        {
            if (_propertyCaches.ContainsKey(name))
                return _propertyCaches[name].Invoke(Instance) as T;

            var property = Instance.GetType().GetProperty(name, bindingFlags);
            if (property == null)
                throw new InvalidOperationException(string.Format("Property '{0}' is not found in this instance", name));
            _propertyCaches.Add(name, CreatePropertyAccessor(property));

            return _propertyCaches[name].Invoke(Instance) as T;
        }

        protected T AccessPropertyAs<T>(string name, BindingFlags bindingFlags) where T : struct
        {
            return (T) AccessProperty<object>(name, bindingFlags);
        }

        private Func<TS, object[], object> CreateMethodAccessor(MethodInfo mi)
        {
            var instance = Expression.Parameter(typeof(TS), "instance");
            var args = Expression.Parameter(typeof(object[]), "args");

            if (mi.GetParameters().Length == 0)
            {
                // (S instance, params object[] args) => (object) instance.Method();
                // (object instance, params object[] args) => (object) ((isa) instance).Method();
                var body = _isa == null
                    ? Expression.Convert(Expression.Call(instance, mi), typeof(object))
                    : Expression.Convert(Expression.Call(Expression.Convert(instance, _isa), mi), typeof(object));

                return Expression.Lambda<Func<TS, object[], object>>(body, instance, args).Compile();
            }
            else
            {
                // (S instance, params object[] args) => (object) instance.Method((T1) args[0], ...);
                // (object instance, params object[] args) => (object) ((isa) instance).Method((T1) args[0], ...);
                var parameters = mi.GetParameters().Select((w, i) => Expression.Convert(Expression.ArrayIndex(args, Expression.Constant(i)), w.ParameterType)).Cast<Expression>().ToArray();
                var body = _isa == null
                    ? Expression.Convert(Expression.Call(instance, mi, parameters), typeof(object))
                    : Expression.Convert(Expression.Call(Expression.Convert(instance, _isa), mi, parameters), typeof(object));

                return Expression.Lambda<Func<TS, object[], object>>(body, instance, args).Compile();
            }
        }

        private Func<TS, object> CreateFieldAccessor(FieldInfo fi)
        {
            // (S instance) => (object) instance.field;
            // (object instance) => (object) ((isa) instance).field;
            var instance = Expression.Parameter(typeof(TS), "instance");
            var body = _isa == null
                ? Expression.Convert(Expression.Field(instance, fi), typeof(object))
                : Expression.Convert(Expression.Field(Expression.Convert(instance, _isa), fi), typeof(object));

            return Expression.Lambda<Func<TS, object>>(body, instance).Compile();
        }

        private Func<TS, object> CreatePropertyAccessor(PropertyInfo pi)
        {
            // (S instance) => (object) instance.Property;
            // (object instance) => (object) ((isa) instance).Property;
            var instance = Expression.Parameter(typeof(TS), "instance");
            var body = _isa == null
                ? Expression.Convert(Expression.Property(instance, pi), typeof(object))
                : Expression.Convert(Expression.Property(Expression.Convert(instance, _isa), pi), typeof(object));

            return Expression.Lambda<Func<TS, object>>(body, instance).Compile();
        }
    }
}