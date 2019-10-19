using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

namespace MUE2.Editor.Models
{
    internal static class PropertyTransformer
    {
        private static string TransformToStringPathByComponent(Component component)
        {
            return TransformToStringPath(component.transform);
        }

        private static string TransformToStringPathByTransform(Transform transform)
        {
            var path = new List<string>();
            var currentTransform = transform;

            do
            {
                path.Insert(0, currentTransform.name);
                currentTransform = currentTransform.parent;
            } while (currentTransform != null);

            return string.Join("/", path.ToArray());
        }

        public static string TransformToStringPath(object @object)
        {
            if (@object is Transform)
                return TransformToStringPathByTransform((Transform) @object);
            if (@object is Component)
                return TransformToStringPathByComponent((Component) @object);
            if (@object is Object)
                return ((Object) @object).name;
            return @object == null ? "(null)" : "(unknown)";
        }

        public static List<SerializedProperty> TransformToList(SerializedProperty property)
        {
            var list = new List<SerializedProperty>();
            var size = property.arraySize;
            for (var i = 0; i < size; i++)
                list.Add(property.GetArrayElementAtIndex(i));

            return list;
        }
    }
}