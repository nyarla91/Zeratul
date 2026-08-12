using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gameplay.Data
{
    [CreateAssetMenu(menuName = "Gameplay Data/Game Data Registry", order = 0)]
    public class GameDataRegistry : ScriptableObject
    {
        [SerializeField] private Object[] _directories;
        [SerializeField] private Object[] _objects;

        private Dictionary<string, Object> _registry;

        public T Get<T>(string name) where T : Object
        {
            _registry ??= _objects.ToDictionary(o => o.name, o => o);

            if ( ! _registry.TryGetValue(name, out Object result))
                throw new KeyNotFoundException($"{this.name} does not contain object {name} of type {typeof(T).Name}");
            return (T) result;
        }

#if UNITY_EDITOR
        [Button("Refresh")]
        private void RefreshObjectsFromFolders()
        {
            List<Object> result = new();

            foreach (Object dir in _directories)
            {
                if ( ! dir)
                    continue;

                string path = AssetDatabase.GetAssetPath(dir);
                if (string.IsNullOrEmpty(path) || !AssetDatabase.IsValidFolder(path))
                    continue;

                string[] guids = AssetDatabase.FindAssets("", new[] { path });

                foreach (string guid in guids)
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    if (AssetDatabase.IsValidFolder(assetPath)) 
                        continue;

                    Object asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                    if (asset != null)
                        result.Add(asset);
                }
            }

            _objects = result.ToArray();
            EditorUtility.SetDirty(this);
        }
#endif

        private void OnValidate()
        {
            _objects = _objects.ToHashSet().ToArray();
        }
    }
}