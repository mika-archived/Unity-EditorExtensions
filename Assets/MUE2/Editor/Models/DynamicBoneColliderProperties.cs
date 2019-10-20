using System;

using UnityEditor;

using UnityEngine;

namespace MUE2.Editor.Models
{
    [Serializable]
    internal class DynamicBoneColliderProperties
    {
        private DynamicBoneColliderProperties() { }

        public static DynamicBoneColliderProperties CreateObject(Component component)
        {
            using (var serializer = new SerializedObject(component))
            {
                return new DynamicBoneColliderProperties
                {
                    Bound = serializer.FindProperty("m_Bound").enumValueIndex,
                    Center = serializer.FindProperty("m_Center").vector3Value,
                    Direction = serializer.FindProperty("m_Direction").enumValueIndex,
                    Height = serializer.FindProperty("m_Height").floatValue,
                    Radius = serializer.FindProperty("m_Radius").floatValue
                };
            }
        }

        public void ApplyProperties(Component component)
        {
            using (var serializer = new SerializedObject(component))
            {
                serializer.FindProperty("m_Bound").enumValueIndex = Bound;
                serializer.FindProperty("m_Center").vector3Value = Center;
                serializer.FindProperty("m_Direction").enumValueIndex = Direction;
                serializer.FindProperty("m_Height").floatValue = Height;
                serializer.FindProperty("m_Radius").floatValue = Radius;

                serializer.ApplyModifiedPropertiesWithoutUndo();
            }
        }

        #region Bound

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private int m_Bound;

        public int Bound
        {
            get { return m_Bound; }
            set { m_Bound = value; }
        }

        #endregion

        #region Center

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private Vector3 m_Center;

        public Vector3 Center
        {
            get { return m_Center; }
            set { m_Center = value; }
        }

        #endregion

        #region Direction

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private int m_Direction;

        public int Direction
        {
            get { return m_Direction; }
            set { m_Direction = value; }
        }

        #endregion

        #region Height

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private float m_Height;

        public float Height
        {
            get { return m_Height; }
            set { m_Height = value; }
        }

        #endregion

        #region Radius

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private float m_Radius;

        public float Radius
        {
            get { return m_Radius; }
            set { m_Radius = value; }
        }

        #endregion
    }
}