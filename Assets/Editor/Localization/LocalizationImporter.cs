using System.Collections.Generic;
using System.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using NaughtyAttributes;
using Settings.Localization;
using UnityEditor;
using UnityEngine;

namespace Editor.Localization
{
    [CreateAssetMenu(menuName = "Localization/Localization Importer")]
    public class LocalizationImporter : ScriptableObject
    {
        [SerializeField] private string _speadsheetId;
        [SerializeField] private string _range;
        [SerializeField] private TextAsset _credentials;
        [SerializeField] private LanguageColumn[] _columns;

        [Button("Open Sheets")]
        public void OpenSheet()
        {
            Application.OpenURL($"https://docs.google.com/spreadsheets/d/{_speadsheetId}/edit");
        }

        [Button("Import")]
        public void Import()
        {
            try
            {
                GoogleCredential credential = GoogleCredential.FromJson(_credentials.text)
                    .CreateScoped(SheetsService.Scope.SpreadsheetsReadonly);

                SheetsService service = new(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Unity Connection Test"
                });

                string spreadsheetId = _speadsheetId;
                SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, _range);
                ValueRange response = request.Execute();
                
                Debug.Log($"Values retrieved successfully");

                foreach (IList<object> row in response.Values)
                {
                    Debug.Log(row[1].ToString());
                }

                foreach (LanguageColumn languageColumn in _columns)
                {
                    List<LanguageEntry> entries = new();
                    for (int i = 0; i < response.Values.Count; i++)
                    {
                        IList<object> row = response.Values[i];
                        if (i == 0 && row.Count <= languageColumn.ColumnIndex)
                        {
                            Debug.LogError($"Row {languageColumn.ColumnIndex} not found");
                            break;
                        }
                        if (i == 0)
                        {
                            continue;
                        }

                        entries.Add(new LanguageEntry(row[0].ToString(), row[languageColumn.ColumnIndex].ToString()));
                        languageColumn.Table.Set(entries);
                        EditorUtility.SetDirty(languageColumn.Table);
                        AssetDatabase.SaveAssetIfDirty(languageColumn.Table);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Connection failed: {ex.Message}");
            }
        }
    }
}