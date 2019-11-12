using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MUE2.Editor.Reflection.Expressions.Generic
{
    public class ReflectionClass<T> where T : class
    {
        protected T Instance { get; }

        protected ReflectionClass(T instance)
        {
            Instance = instance;
        }

        protected TResult InvokeMethod<TResult>(string name, BindingFlags bindingFlags, params object[] parameters)
        {
            Cache<T>.Methods.TryGetValue(name, out var cache);
            if (cache != null)
                return (TResult) cache.Invoke(Instance, parameters);

            var mi = Instance.GetType().GetMethod(name, bindingFlags);
            if (mi == null)
                throw new InvalidOperationException($"Method '{name}' is not found in this class");

            Cache<T>.Methods.Add(name, CreateMethodAccessor(mi));
            return (TResult) Cache<T>.Methods[name].Invoke(Instance, parameters);
        }

        protected TResult InvokeMember<TResult>(string name)
        {
            Cache<T>.Members.TryGetValue(name, out var cache);
            if (cache != null)
                return (TResult) cache.Invoke(Instance);

            Cache<T>.Members.Add(name, CreateMemberAccessor(name));
            return (TResult) Cache<T>.Members[name].Invoke(Instance);
        }

        private static Func<T, object[], object> CreateMethodAccessor(MethodInfo mi)
        {
            var instance = Expression.Parameter(typeof(T), "instance");
            var args = Expression.Parameter(typeof(object[]), "args");
            var body = mi.GetParameters().Length == 0
                ? Expression.Call(instance, mi)
                : Expression.Call(instance, mi, mi.GetParameters().Select((w, i) => Expression.Convert(Expression.ArrayIndex(args, Expression.Constant(i)), w.ParameterType)).Cast<Expression>().ToArray());

            return Expression.Lambda<Func<T, object[], object>>(Expression.Convert(body, typeof(object)), instance, args).Compile();
        }

        private static Func<T, object> CreateMemberAccessor(string name)
        {
            try
            {
                var instance = Expression.Parameter(typeof(T), "instance");
                var body = Expression.PropertyOrField(instance, name);

                return Expression.Lambda<Func<T, object>>(Expression.Convert(body, typeof(object)), instance).Compile();
            }
            catch
            {
                throw new InvalidOperationException($"Member '{name}' is not found in this class");
            }
        }

        private static class Cache<TCache> where TCache : class
        {
            public static readonly Dictionary<string, Func<TCache, object[], object>> Methods = new Dictionary<string, Func<TCache, object[], object>>();
            public static readonly Dictionary<string, Func<TCache, object>> Members = new Dictionary<string, Func<TCache, object>>();
        }
    }
}