using System;
using System.Collections.Generic;
using System.IO;
using _Core;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Save.Data;
using UnityEngine;

namespace Save
{
    public class SaveFileIO : ISaveFileWriteService, ISaveFileReadService
    {
        public string SaveFolderPath => Application.dataPath + "/save/";
        private string Extension => ".json";

        public event Action<SaveData> SaveWritten; 
        public event Action<string> SaveDeleted; 

        public async UniTask<bool> Write(SaveData data)
        {
            if ( ! data.filename.IsFilenameValid())
                return false;
            string filepath = FilenameToPath(data.filename);
            if ( ! IsPathValid(filepath))
                return false;
            string json = JsonConvert.SerializeObject(data);
            if ( ! Directory.Exists(SaveFolderPath))
                Directory.CreateDirectory(SaveFolderPath);
            try
            {
                await File.WriteAllTextAsync(filepath, json);
                SaveWritten?.Invoke(data);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to write file {filepath}: {e}");
                return false;
            }
        }

        public void Delete(string filename)
        {
            string filepath = FilenameToPath(filename);
            if ( ! IsPathValid(filepath) || ! File.Exists(filepath))
                return;
            try
            {
                File.Delete(filepath);
                SaveDeleted?.Invoke(filename);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to delete file {filepath}: {e}");
            }
        }

        public async UniTask<SaveData[]> ReadAll()
        {
            if ( ! Directory.Exists(SaveFolderPath))
                return new SaveData[0];
            
            List<SaveData> result = new();
            string[] files = await UniTask.RunOnThreadPool(() => Directory.GetFiles(SaveFolderPath));
            foreach (string file in files)
            {
                if ( ! file.EndsWith(".json"))
                    continue;

                result.Add(await Read(file));
            }
            return result.ToArray();
        }

        public async UniTask<SaveData> Read(string filepath)
        {
            if ( ! IsPathValid(filepath) || ! File.Exists(filepath))
                return null;

            string json;
            try
            {
                json = await UniTask.RunOnThreadPool(() => File.ReadAllText(filepath));
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to read file {filepath}: {e}");
                return null;
            }
            
            try
            {
                SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);
                if ( ! saveData.IsValid())
                    return null;
                saveData.filename = FilepathToName(filepath);
                return saveData;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to deserialize save file: {e}");
                return null;
            }
        }
        
        private string FilenameToPath(string filename) => SaveFolderPath + filename + Extension;
        
        private string FilepathToName(string filepath) => filepath.Replace(SaveFolderPath, "").Replace(Extension, "");

        private bool IsPathValid(string path) => path.Contains(SaveFolderPath) && path.EndsWith(Extension);
    }
    
    public interface ISaveFileWriteService
    {
        public UniTask<bool> Write(SaveData data);
        public void Delete(string filename);
    }

    public interface ISaveFileReadService
    {
        public event Action<SaveData> SaveWritten; 
        public event Action<string> SaveDeleted;
        public UniTask<SaveData[]> ReadAll();
        public UniTask<SaveData> Read(string filepath);
    }
}