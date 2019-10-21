﻿using System;

using UnityEngine;

namespace MUE2.Editor.Models
{
    [Serializable]
    internal class DynamicBoneConfiguration
    {
        internal int OriginalInstanceId { get; set; }
        internal Component Component { get; set; }

        internal void StoreConfiguration()
        {
            switch (Component.GetType().FullName)
            {
                case "DynamicBone":
                    DynamicBoneProperties = DynamicBoneProperties.CreateObject(Component);
                    EffectiveAs = "DynamicBone";
                    break;

                case "DynamicBoneCollider":
                    DynamicBoneColliderProperties = DynamicBoneColliderProperties.CreateObject(Component);
                    EffectiveAs = "DynamicBoneCollider";
                    break;

                case "DynamicBonePlaneCollider":
                    Debug.Log("Not Supported Yet");
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #region DynamicBoneProperties

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private DynamicBoneProperties m_DynamicBoneProperties;

        public DynamicBoneProperties DynamicBoneProperties
        {
            get { return m_DynamicBoneProperties; }
            set { m_DynamicBoneProperties = value; }
        }

        #endregion

        #region DynamicBoneColliderProperties

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private DynamicBoneColliderProperties m_DynamicBoneColliderProperties;

        public DynamicBoneColliderProperties DynamicBoneColliderProperties
        {
            get { return m_DynamicBoneColliderProperties; }
            set { m_DynamicBoneColliderProperties = value; }
        }

        #endregion

        #region EffectiveAs

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private string m_EffectiveAs;

        public string EffectiveAs
        {
            get { return m_EffectiveAs; }
            set { m_EffectiveAs = value; }
        }

        #endregion

        #region Name

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private string m_Name;

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        #endregion

        #region NamingConvention

        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private string m_NamingConvention;

        public string NamingConvention
        {
            get { return m_NamingConvention; }
            set { m_NamingConvention = value; }
        }

        #endregion
    }
}