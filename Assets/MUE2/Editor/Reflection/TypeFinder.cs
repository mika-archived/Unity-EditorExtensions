using System;
using System.Collections.Generic;

namespace MUE2.Editor.Reflection
{
    internal static class TypeFinder
    {
        private static readonly Dictionary<string, Type> Caches;

        static TypeFinder()
        {
            Caches = new Dictionary<string, Type>();
        }

        public static Type GetType(string fullname)
        {
            if (Caches.ContainsKey(fullname))
                return Caches[fullname];

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var t = assembly.GetType(fullname);
                if (t == null)
                    continue;

                Caches.Add(fullname, t);
                return t;
            }

            return null;
        }
    }
}