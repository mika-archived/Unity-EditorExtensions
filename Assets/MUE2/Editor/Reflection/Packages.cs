using System;
using System.Reflection;

using MUE2.Editor.Reflection.Expressions;

using UnityEditor.PackageManager;

namespace MUE2.Editor.Reflection
{
    internal static class Packages
    {
        private static readonly Type PackageClass = typeof(PackageInfo).Assembly.GetType("UnityEditor.PackageManager.Packages");

        public static PackageInfo GetForAssetPath(string assetPath)
        {
            return ReflectionStaticClass.InvokeMethod<PackageInfo>(PackageClass, "GetForAssetPath", BindingFlags.Public, assetPath);
        }
    }
}