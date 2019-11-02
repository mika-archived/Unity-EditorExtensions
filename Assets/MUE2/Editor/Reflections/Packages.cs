using System.Reflection;

using UnityEditor.PackageManager;

namespace MUE2.Editor.Reflections
{
    internal class Packages : ReflectionStaticAccessor
    {
        private static Packages _staticInstance;

        // UnityEditor.PackageManager.Packages has no instance members/methods
        private Packages() : base(typeof(PackageInfo).Assembly.GetType("UnityEditor.PackageManager.Packages")) { }

        public static PackageInfo GetForAssetPath(string assetPath)
        {
            if (_staticInstance == null)
                _staticInstance = new Packages();
            return _staticInstance.CallMethod<PackageInfo>("GetForAssetPath", BindingFlags.Static | BindingFlags.Public, assetPath);
        }
    }
}