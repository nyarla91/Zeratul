using System.IO;
using Cysharp.Threading.Tasks;
using UnityEditor.Overlays;
using UnityEngine;

namespace Saving
{
    public class SaveFileIO : ISaveFileLoadService, ISaveFileSaveService
    {
        private string SaveFolderPath => Application.dataPath + "/save/";

        public async void Save(SaveData data, string filename)
        {
            string json = JsonUtility.ToJson(data);
            if ( ! Directory.Exists(SaveFolderPath))
                Directory.CreateDirectory(SaveFolderPath);
            await File.WriteAllTextAsync(SaveFolderPath + filename + ".json", json);
        }

        public async UniTask<SaveData> Load(string filename)
        {
            string json = await File.ReadAllTextAsync(SaveFolderPath + filename + ".json");
            return JsonUtility.FromJson<SaveData>(json);
        }
    }
    
    public interface ISaveFileSaveService
    {
        public void Save(SaveData data, string filename);
    }

    public interface ISaveFileLoadService
    {
        public UniTask<SaveData> Load(string filename);
    }
}