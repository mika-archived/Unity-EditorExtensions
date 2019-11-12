using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MUE2.Editor.Reflection.Expressions
{
    public static class ReflectionStaticClass
    {
        private static readonly Hashtable Caches = new Hashtable();

        internal static TResult InvokeMethod<TResult>(Type @class, string name, BindingFlags bindingFlags, params object[] parameters)
        {
            var cache = SafeCacheMethodAccess(@class, name);
            if (cache != null)
                return (TResult) cache.Invoke(parameters);

            var mi = @class.GetMethod(name, bindingFlags | BindingFlags.Static);
            if (mi == null)
                throw new InvalidOperationException($"Method '{name}' is not found in this class");

            ((Cache) Caches[@class]).Methods.Add(name, CreateMethodAccessor(mi));
            return (TResult) ((Cache) Caches[@class]).Methods[name].Invoke(parameters);
        }

        internal static TResult InvokeMember<TResult>(Type @class, string name)
        {
            var cache = SafeCacheMemberAccess(@class, name);
            if (cache != null)
                return (TResult) cache.Invoke();

            ((Cache) Caches[@class]).Members.Add(name, CreateMemberAccessor(@class, name));
            return (TResult) ((Cache) Caches[@class]).Members[name].Invoke();
        }

        private static Func<object[], object> CreateMethodAccessor(MethodInfo mi)
        {
            var args = Expression.Parameter(typeof(object[]), "args");
            var body = mi.GetParameters().Length == 0
                ? Expression.Call(null, mi)
                : Expression.Call(null, mi, mi.GetParameters().Select((w, i) => Expression.Convert(Expression.ArrayIndex(args, Expression.Constant(i)), w.ParameterType)).Cast<Expression>().ToArray());

            return Expression.Lambda<Func<object[], object>>(Expression.Convert(body, typeof(object)), args).Compile();
        }

        private static Func<object> CreateMemberAccessor(Type @class, string name)
        {
            return Expression.Lambda<Func<object>>(Expression.Convert(Expression.PropertyOrField(Expression.Constant(@class), name), typeof(object))).Compile;
        }

        private static Func<object[], object> SafeCacheMethodAccess(Type type, string name)
        {
            if (Caches[type] == null)
            {
                Caches[type] = new Cache();
                return null;
            }

            ((Cache) Caches[type]).Methods.TryGetValue(name, out var cache);
            return cache;
        }

        private static Func<object> SafeCacheMemberAccess(Type type, string name)
        {
            if (Caches[type] == null)
            {
                Caches[type] = new Cache();
                return null;
            }

            ((Cache) Caches[type]).Members.TryGetValue(name, out var cache);
            return cache;
        }

        private class Cache
        {
            public readonly Dictionary<string, Func<object>> Members = new Dictionary<string, Func<object>>();
            public readonly Dictionary<string, Func<object[], object>> Methods = new Dictionary<string, Func<object[], object>>();
        }
    }
}