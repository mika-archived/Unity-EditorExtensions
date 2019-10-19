using System;

namespace MUE2.Editor.Reflections
{
    internal static class TypeFinder
    {
        public static Type GetType(string fullname)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var t = assembly.GetType(fullname);
                if (t != null)
                    return t;
            }

            return null;
        }
    }
}