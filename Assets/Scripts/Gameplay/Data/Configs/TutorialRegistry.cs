using System;
using UnityEngine;

namespace Gameplay.Data.Configs
{
    [CreateAssetMenu(menuName = "Tutorial Registry", order = 0)]
    public class TutorialRegistry : ScriptableObject
    {
        [SerializeField] private TutorialEntry[] _entries;
        
        public TutorialEntry GetEntry(int index) => _entries[index];
    }

    [Serializable]
    public class TutorialEntry
    {
        [SerializeField] private string _header;
        [SerializeField] [TextArea(1, 30)] private string[] _pages;

        public string Header => _header;
        public string[] Pages => _pages;
    }
}