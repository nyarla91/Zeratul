using System;
using UnityEngine;

namespace Localization
{
    [CreateAssetMenu(menuName = "Localization/Localization Config")]
    public class LocalizationConfig : ScriptableObject
    {
        [SerializeField] private Language _language;
        
        private enum Language
        {
            English = 0,
            Russian = 1,
        }
    }
}