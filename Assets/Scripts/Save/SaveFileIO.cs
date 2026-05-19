using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Save.Data;
using UnityEngine;

namespace Save
{
    public class SaveFileIO : ISaveFileLoadService, ISaveFileSaveService
    {
        private string SaveFolderPath => Application.dataPath + "/save/";

        public async void Write(SaveData data, string filename)
        {
            string json = JsonConvert.SerializeObject(data);
            if ( ! Directory.Exists(SaveFolderPath))
                Directory.CreateDirectory(SaveFolderPath);
            await File.WriteAllTextAsync(SaveFolderPath + filename + ".json", json);
        }

        public async UniTask<SaveData> Read(string filename)
        {
            string path = SaveFolderPath + filename + ".json";
            if (!File.Exists(path))
                return null;
            string json = await File.ReadAllTextAsync(path);
            return JsonConvert.DeserializeObject<SaveData>(json);
        }
    }
    
    public interface ISaveFileSaveService
    {
        public void Write(SaveData data, string filename);
    }

    public interface ISaveFileLoadService
    {
        public UniTask<SaveData> Read(string filename);
    }
}