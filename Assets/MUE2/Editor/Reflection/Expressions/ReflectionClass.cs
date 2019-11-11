using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MUE2.Editor.Reflection.Expressions
{
    public class ReflectionClass
    {
        private static readonly Hashtable Caches = new Hashtable();
        private readonly object _instance;
        private readonly Type _type;

        protected ReflectionClass(object instance, Type type)
        {
            _instance = instance;
            _type = type;

            if (Caches[_type] == null)
                Caches[_type] = new Cache();
        }

        protected TResult InvokeMethod<TResult>(string name, BindingFlags bindingFlags, params object[] parameters)
        {
            Func<object, object[], object> cache;
            ((Cache) Caches[_type]).Methods.TryGetValue(name, out cache);
            if (cache != null)
                return (TResult) cache.Invoke(_instance, parameters);

            var mi = _instance.GetType().GetMethod(name, bindingFlags);
            if (mi == null)
                throw new InvalidOperationException(string.Format("Method '{0}' is not found in this class", name));

            ((Cache) Caches[_type]).Methods.Add(name, CreateMethodAccessor(mi));
            return (TResult) ((Cache) Caches[_type]).Methods[name].Invoke(_instance, parameters);
        }

        protected TResult InvokeMember<TResult>(string name)
        {
            Func<object, object> cache;
            ((Cache) Caches[_type]).Members.TryGetValue(name, out cache);
            if (cache != null)
                return (TResult) cache.Invoke(_instance);

            ((Cache) Caches[_type]).Members.Add(name, CreateMemberAccessor(name));
            return (TResult) ((Cache) Caches[_type]).Members[name].Invoke(_instance);
        }

        private Func<object, object[], object> CreateMethodAccessor(MethodInfo mi)
        {
            var instance = Expression.Parameter(typeof(object), "instance");
            var args = Expression.Parameter(typeof(object[]), "args");
            var body = mi.GetParameters().Length == 0
                ? Expression.Call(Expression.Convert(instance, _type), mi)
                : Expression.Call(Expression.Convert(instance, _type), mi, mi.GetParameters().Select((w, i) => Expression.Convert(Expression.ArrayIndex(args, Expression.Constant(i)), w.ParameterType)).Cast<Expression>().ToArray());

            return Expression.Lambda<Func<object, object[], object>>(Expression.Convert(body, typeof(object)), instance, args).Compile();
        }

        private Func<object, object> CreateMemberAccessor(string name)
        {
            try
            {
                var instance = Expression.Parameter(typeof(object), "instance");
                var body = Expression.PropertyOrField(Expression.Convert(instance, _type), name);

                return Expression.Lambda<Func<object, object>>(Expression.Convert(body, typeof(object)), instance).Compile();
            }
            catch
            {
                throw new InvalidOperationException(string.Format("Member '{0}' is not found in this class", name));
            }
        }

        private class Cache
        {
            public readonly Dictionary<string, Func<object, object>> Members = new Dictionary<string, Func<object, object>>();
            public readonly Dictionary<string, Func<object, object[], object>> Methods = new Dictionary<string, Func<object, object[], object>>();
        }
    }
}