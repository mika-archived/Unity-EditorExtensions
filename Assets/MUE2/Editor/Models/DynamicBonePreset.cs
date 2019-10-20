using System;
using System.Collections.Generic;

using UnityEngine;

namespace MUE2.Editor.Models
{
    [Serializable]
    internal class DynamicBonePreset : ScriptableObject
    {
        // ReSharper disable once InconsistentNaming
        [SerializeField]
        private List<DynamicBoneConfiguration> m_Configs;

        public List<DynamicBoneConfiguration> Configs
        {
            get { return m_Configs; }
            set { m_Configs = value; }
        }

        public DynamicBonePreset()
        {
            Configs = new List<DynamicBoneConfiguration>();
        }
    }
}