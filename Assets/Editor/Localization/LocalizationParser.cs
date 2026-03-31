using System;
using System.Collections.Generic;
using System.Linq;
using Localization;
using NaughtyAttributes;
using UnityEngine;

namespace Editor.Localization
{
    [CreateAssetMenu(menuName = "Localization/Localization Parser")]
    public class LocalizationParser : ScriptableObject
    {
        [SerializeField] private TextAsset _csv;
        [SerializeField] private LanguageColumn[] _columns;
        
        [Button("Parse")]
        public void Parse()
        {
            string[,] table = CSVtoArray(_csv.text);

            foreach (LanguageColumn languageColumn in _columns)
            {
                List<LanguageEntry> entries = new();
                for (int y = 1; y < table.GetLength(1); y++)
                {
                    if (table[languageColumn.ColumnIndex, y] == "")
                        continue;
                    LanguageEntry entry = new(table[0, y], table[languageColumn.ColumnIndex, y]);
                    entries.Add(entry);
                }
                languageColumn.Table.Set(entries);
            }
        }

        private string[,] CSVtoArray(string csv)
        {
            string[] rows = csv.Split('\n');
            string[,] result =  new string[_columns.Max(c => c.ColumnIndex) + 1, rows.Length];

            for (int y = 0; y < result.GetLength(1); y++)
            {
                string[] row = rows[y].Split(',');
                
                for (int x = 0; x < result.GetLength(0) && x < row.Length; x++)
                {
                    Debug.Log($"{x}, {y} - {row[x]}");
                    result[x, y] = row[x];
                }
            }
            return result;
        }

        [Serializable]
        private struct LanguageColumn
        {
            [SerializeField] private int _columnIndex;
            [SerializeField] private LanguageTable _table;
            
            public int ColumnIndex => _columnIndex;
            public LanguageTable Table => _table;
        } 
    }
}