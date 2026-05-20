using System.Collections.Generic;
using System.Linq;
using Gameplay.Schemes.Values.Variables;
using Save.Data;
using UnityEngine;

namespace Gameplay.Arrangement.Saving
{
    public class SchemeSavingSystem : SavingSystem<SchemeSaveSystem>
    {
        protected override string LoadKey => SchemeSaveSystem.LoadKey;

        private HashSet<ISaveableVariable> _variables;
        
        public override void ReproduceFromSaveData(SchemeSaveSystem payload)
        {
            _variables ??= InitVariables();

            foreach (ISaveableVariable saveableVariable in _variables)
            {
                if (payload.variables.TryGetValue(saveableVariable.Key, out string json))
                    saveableVariable.ReproduceFromSaveData(json);
            }
        }

        public override ISaveSystem Save()
        {
            _variables ??= InitVariables();

            Dictionary<string, string> variablesData = _variables
                .ToDictionary(v => v.Key, v => v.Save());
            
            return new SchemeSaveSystem(variablesData);
        }

        private HashSet<ISaveableVariable> InitVariables()
        {
            return FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .OfType<ISaveableVariable>()
                .Where(v => v.Key != "")
                .ToHashSet();
        }
    }
}