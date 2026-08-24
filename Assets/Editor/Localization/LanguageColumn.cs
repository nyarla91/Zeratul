using System;
using Settings.Localization;
using UnityEngine;

namespace Editor.Localization
{
    [Serializable]
    public struct LanguageColumn
    {
        [SerializeField] private int _columnIndex;
        [SerializeField] private LanguageTable _table;
            
        public int ColumnIndex => _columnIndex;
        public LanguageTable Table => _table;
    } 
}