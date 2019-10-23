using System.IO;
using System.Linq;

using UnityEditor;

using UnityEngine;

// ReSharper disable once CheckNamespace
#pragma warning disable RCS1110 // Declare type inside namespace.

internal static class PackageExport
#pragma warning restore RCS1110 // Declare type inside namespace.
{
    [MenuItem("Tools/Export as UnityPackage")]
    public static void Export()
    {
        var path = Path.Combine(Application.dataPath, "MUE2");
        var assets = Directory.GetFiles(path, "*", SearchOption.AllDirectories)
                              .Where(w => Path.GetExtension(w) == ".cs" && !w.EndsWith("PackageExport.cs"))
                              .Select(w => "Assets" + w.Replace(Application.dataPath, "").Replace("\\", "/"))
                              .ToArray();

        Debug.Log(string.Format("Export: {0}", string.Join(", ", assets)));

        AssetDatabase.ExportPackage(assets, "./Packages/MUE2.unitypackage", ExportPackageOptions.Default);
    }
}