using System;

namespace Save.Data
{
    [Serializable]
    public class EntitiesSaveSystem : ISaveSystem
    {
        public static string LoadKey => "entities";
        public string SaveKey => LoadKey;

        public EntitySaveData[] entities;

        public EntitiesSaveSystem() { }
        
        public EntitiesSaveSystem(EntitySaveData[] entities)
        {
            this.entities = entities;
        }

        public bool IsValid()
        {
            return entities != null;
        }
    }
}