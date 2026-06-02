using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Extentions;
using Newtonsoft.Json;
using Save.Data;
using UnityEngine;

namespace Save
{
    public class SaveFileIO : ISaveFileWriteService, ISaveFileReadService
    {
        public string SaveFolderPath => Application.dataPath + "/save/";
        private string Extension => ".json";

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
                return true;
            }
            catch (IOException e)
            {
                return false;
            }
        }

        public void Delete(string filename)
        {
            string filepath = FilenameToPath(filename);
            if ( ! IsPathValid(filepath) || ! File.Exists(filepath))
                return;
            File.Delete(filepath);
        }

        public async UniTask<SaveData[]> ReadAll()
        {
            if ( ! Directory.Exists(SaveFolderPath))
                return new SaveData[0];
            
            List<SaveData> result = new();
            string[] files = Directory.GetFiles(SaveFolderPath);
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
            
            try
            {
                string json = await File.ReadAllTextAsync(filepath);
                SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);
                if ( ! saveData.IsValid())
                    return null;
                saveData.filename = FilepathToName(filepath);
                return saveData;
            }
            catch (JsonException e)
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
        public UniTask<SaveData[]> ReadAll();
        
        public UniTask<SaveData> Read(string filepath);
    }
}