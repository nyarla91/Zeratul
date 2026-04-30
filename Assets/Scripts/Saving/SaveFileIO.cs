using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Saving.Data;
using UnityEngine;

namespace Saving
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
            string json = await File.ReadAllTextAsync(SaveFolderPath + filename + ".json");
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