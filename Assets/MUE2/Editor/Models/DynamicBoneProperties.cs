using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using MUE2.Editor.Reflection;

using UnityEditor;

using UnityEngine;

namespace MUE2.Editor.Models
{
    // Property Model for DynamicBone
    [Serializable]
    internal class DynamicBoneProperties
    {
        public static DynamicBoneProperties CreateObject(Component component)
        {
            var path = PropertyTransformer.TransformToStringPath(component);
            using (var serializer = new SerializedObject(component))
            {
                return new DynamicBoneProperties
                {
                    Colliders = PropertyTransformer.TransformToList(serializer.FindProperty("m_Colliders")).Select(w => ToRelativePath(path, PropertyTransformer.TransformToStringPath(w.objectReferenceValue))).ToList(),
                    Damping = serializer.FindProperty("m_Damping").floatValue,
                    DampingDistrib = serializer.FindProperty("m_DampingDistrib").animationCurveValue,
                    DistanceToObject = serializer.FindProperty("m_DistanceToObject").floatValue,
                    DistantDisable = serializer.FindProperty("m_DistantDisable").boolValue,
                    Elasticity = serializer.FindProperty("m_Elasticity").floatValue,
                    ElasticityDistrib = serializer.FindProperty("m_ElasticityDistrib").animationCurveValue,
                    EndLength = serializer.FindProperty("m_EndLength").floatValue,
                    EndOffset = serializer.FindProperty("m_EndOffset").vector3Value,
                    Exclusion = PropertyTransformer.TransformToList(serializer.FindProperty("m_Exclusions")).Select(w => ToRelativePath(path, PropertyTransformer.TransformToStringPath(w.objectReferenceValue))).ToList(),
                    Force = serializer.FindProperty("m_Force").vector3Value,
                    FreezeAxis = serializer.FindProperty("m_FreezeAxis").enumValueIndex,
                    Gravity = serializer.FindProperty("m_Gravity").vector3Value,
                    Inert = serializer.FindProperty("m_Inert").floatValue,
                    InertDistrib = serializer.FindProperty("m_InertDistrib").animationCurveValue,
                    Radius = serializer.FindProperty("m_Radius").floatValue,
                    RadiusDistrib = serializer.FindProperty("m_RadiusDistrib").animationCurveValue,
                    ReferenceObject = ToRelativePath(path, PropertyTransformer.TransformToStringPath(serializer.FindProperty("m_ReferenceObject").objectReferenceValue)),
                    RootReference = ToRelativePath(path, PropertyTransformer.TransformToStringPath(serializer.FindProperty("m_Root").objectReferenceValue)),
                    Stiffness = serializer.FindProperty("m_Stiffness").floatValue,
                    StiffnessDistrib = serializer.FindProperty("m_StiffnessDistrib").animationCurveValue,
                    UpdateMode = serializer.FindProperty("m_UpdateMode").enumValueIndex,
                    UpdateRate = serializer.FindProperty("m_UpdateRate").floatValue
                };
            }
        }

        private static string ToRelativePath(string from, string to)
        {
            if (to == "(null)")
                return "(null)";
            if (from == to)
                return "(self)";

            var fromUri = new Uri($"file:///{from}");
            var toUri = new Uri($"file:///{to}");
            return $"../{fromUri.MakeRelativeUri(toUri)}";
        }

        private static string ToAbsolutePath(string @base, string to)
        {
            if (to == "(null)")
                return "";
            if (to == "(self)")
                return @base;

            return Path.GetFullPath(Path.Combine(@base, to)).Replace(Environment.CurrentDirectory, "").Substring(1);
        }

        private static Transform ToActualObject(string path, string relative, GameObject rootObject)
        {
            var absolute = ToAbsolutePath(path, relative).Replace(Path.DirectorySeparatorChar.ToString(), "/");
            var transforms = rootObject.GetComponentsInChildren<Transform>();
            return transforms.SingleOrDefault(w => PropertyTransformer.TransformToStringPath(w) == absolute);
        }

        private static void ApplyArrayToProperty(SerializedProperty property, List<Transform> transforms)
        {
            if (transforms.Count == 0)
                return;

            for (var i = 0; i < transforms.Count; i++)
            {
                property.InsertArrayElementAtIndex(i);
                property.GetArrayElementAtIndex(i).objectReferenceValue = transforms[i].gameObject.GetComponent(TypeFinder.GetType("DynamicBoneCollider"));
            }

            property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        public void ApplyProperties(Component component, GameObject root)
        {
            var path = PropertyTransformer.TransformToStringPath(component);
            using (var serializer = new SerializedObject(component))
            {
                ApplyArrayToProperty(serializer.FindProperty("m_Colliders"), Colliders.Select(w => ToActualObject(path, w, root)).ToList());
                serializer.FindProperty("m_Damping").floatValue = Damping;
                serializer.FindProperty("m_DampingDistrib").animationCurveValue = DampingDistrib;
                serializer.FindProperty("m_DistanceToObject").floatValue = DistanceToObject;
                serializer.FindProperty("m_DistantDisable").boolValue = DistantDisable;
                serializer.FindProperty("m_Elasticity").floatValue = Elasticity;
                serializer.FindProperty("m_ElasticityDistrib").animationCurveValue = ElasticityDistrib;
                serializer.FindProperty("m_EndLength").floatValue = EndLength;
                serializer.FindProperty("m_EndOffset").vector3Value = EndOffset;
                ApplyArrayToProperty(serializer.FindProperty("m_Exclusions"), Exclusion.Select(w => ToActualObject(path, w, root)).ToList());
                serializer.FindProperty("m_Force").vector3Value = Force;
                serializer.FindProperty("m_FreezeAxis").enumValueIndex = FreezeAxis;
                serializer.FindProperty("m_Gravity").vector3Value = Gravity;
                serializer.FindProperty("m_Inert").floatValue = Inert;
                serializer.FindProperty("m_InertDistrib").animationCurveValue = InertDistrib;
                serializer.FindProperty("m_Radius").floatValue = Radius;
                serializer.FindProperty("m_RadiusDistrib").animationCurveValue = RadiusDistrib;
                serializer.FindProperty("m_ReferenceObject").objectReferenceValue = ToActualObject(path, ReferenceObject, root);
                serializer.FindProperty("m_Root").objectReferenceValue = ToActualObject(path, RootReference, root);
                serializer.FindProperty("m_Stiffness").floatValue = Stiffness;
                serializer.FindProperty("m_StiffnessDistrib").animationCurveValue = StiffnessDistrib;
                serializer.FindProperty("m_UpdateMode").enumValueIndex = UpdateMode;
                serializer.FindProperty("m_UpdateRate").floatValue = UpdateRate;

                serializer.ApplyModifiedPropertiesWithoutUndo();
            }
        }

        #region Colliders

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private List<string> m_Colliders;

        public List<string> Colliders
        {
            get => m_Colliders;
            set => m_Colliders = value;
        }

        #endregion

        #region Damping

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private float m_Damping;

        public float Damping
        {
            get => m_Damping;
            set => m_Damping = value;
        }

        #endregion

        #region DampingDistrib

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private AnimationCurve m_DampingDistrib;

        public AnimationCurve DampingDistrib
        {
            get => m_DampingDistrib;
            set => m_DampingDistrib = value;
        }

        #endregion

        #region DistanceToObject

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private float m_DistanceToObject;

        public float DistanceToObject
        {
            get => m_DistanceToObject;
            set => m_DistanceToObject = value;
        }

        #endregion

        #region DistantDisable

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private bool m_DistantDisable;

        public bool DistantDisable
        {
            get => m_DistantDisable;
            set => m_DistantDisable = value;
        }

        #endregion

        #region Elasticity

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private float m_Elasticity;

        public float Elasticity
        {
            get => m_Elasticity;
            set => m_Elasticity = value;
        }

        #endregion

        #region ElasticityDistrib

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private AnimationCurve m_ElasticityDistrib;

        public AnimationCurve ElasticityDistrib
        {
            get => m_ElasticityDistrib;
            set => m_ElasticityDistrib = value;
        }

        #endregion

        #region EndLength

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private float m_EndLength;

        public float EndLength
        {
            get => m_EndLength;
            set => m_EndLength = value;
        }

        #endregion

        #region EndOffset

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private Vector3 m_EndOffset;

        public Vector3 EndOffset
        {
            get => m_EndOffset;
            set => m_EndOffset = value;
        }

        #endregion

        #region Exclusions (Reference)

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private List<string> m_Exclusion;

        public List<string> Exclusion
        {
            get => m_Exclusion;
            set => m_Exclusion = value;
        }

        #endregion

        #region Force

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private Vector3 m_Force;

        public Vector3 Force
        {
            get => m_Force;
            set => m_Force = value;
        }

        #endregion

        #region FreezeAxis

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private int m_FreezeAxis;

        public int FreezeAxis
        {
            get => m_FreezeAxis;
            set => m_FreezeAxis = value;
        }

        #endregion

        #region Gravity

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private Vector3 m_Gravity;

        public Vector3 Gravity
        {
            get => m_Gravity;
            set => m_Gravity = value;
        }

        #endregion

        #region Inert

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private float m_Inert;

        public float Inert
        {
            get => m_Inert;
            set => m_Inert = value;
        }

        #endregion

        #region InertDistrib

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private AnimationCurve m_InertDistrib;

        public AnimationCurve InertDistrib
        {
            get => m_InertDistrib;
            set => m_InertDistrib = value;
        }

        #endregion

        #region Radius

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private float m_Radius;

        public float Radius
        {
            get => m_Radius;
            set => m_Radius = value;
        }

        #endregion

        #region RadiusDistrib

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private AnimationCurve m_RadiusDistrib;

        public AnimationCurve RadiusDistrib
        {
            get => m_RadiusDistrib;
            set => m_RadiusDistrib = value;
        }

        #endregion

        #region ReferenceObject (Reference)

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private string m_ReferenceObject;

        public string ReferenceObject
        {
            get => m_ReferenceObject;
            set => m_ReferenceObject = value;
        }

        #endregion

        #region Root (Reference)

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private string m_RootReference;

        public string RootReference
        {
            get => m_RootReference;
            set => m_RootReference = value;
        }

        #endregion

        #region Stiffness

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private float m_Stiffness;

        public float Stiffness
        {
            get => m_Stiffness;
            set => m_Stiffness = value;
        }

        #endregion

        #region StiffnessDistrib

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private AnimationCurve m_StiffnessDistrib;

        public AnimationCurve StiffnessDistrib
        {
            get => m_StiffnessDistrib;
            set => m_StiffnessDistrib = value;
        }

        #endregion

        #region UpdateMode

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private int m_UpdateMode;

        public int UpdateMode
        {
            get => m_UpdateMode;
            set => m_UpdateMode = value;
        }

        #endregion

        #region UpdateRate

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private float m_UpdateRate;

        public float UpdateRate
        {
            get => m_UpdateRate;
            set => m_UpdateRate = value;
        }

        #endregion
    }
}