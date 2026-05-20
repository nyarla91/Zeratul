using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Gameplay.Schemes.Values.Variables
{
    public abstract class SchemeVariable<T> : SchemeValue<T>, ISaveableVariable
    {
        [SerializeField] private string _key;
        
        protected T value;

        public override T Value => value;
        
        protected abstract T DefaultValue { get; }
        protected abstract string DisplayDefaultValue { get; }
        
        public string Key => _key;
        
        public virtual string Save()
        {
            return JsonConvert.SerializeObject(value);
        }

        public virtual void ReproduceFromSaveData(string json)
        {
            value = JsonConvert.DeserializeObject<T>(json);
        }

        public void Set(T value) => this.value = value;
        

        protected virtual void Awake()
        {
            value = DefaultValue;
        }

        private void OnValidate()
        {
            gameObject.name = _key == "" ? DisplayDefaultValue : _key;
        }
    }

    public interface ISaveableVariable
    {
        public string Key { get; }
        public string Save();
        public void ReproduceFromSaveData(string json);
    }
}