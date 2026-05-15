using System;
using Extentions;
using Newtonsoft.Json;

namespace Save.Data.Units
{
    [Serializable]
    public class UnitAISaveSystem : IUnitSaveSystem
    {
        public static string LoadKey => "ai";
        public string SaveKey => LoadKey;

        [JsonProperty] public SerializableVector2 spawnPoint;
        [JsonProperty] public UnitPatrolPath patrolPath;

        public UnitAISaveSystem() { }

        public UnitAISaveSystem(UnitPatrolPath patrolPath, SerializableVector2 spawnPoint)
        {
            this.spawnPoint = spawnPoint;
            this.patrolPath = patrolPath;
        }
    }
}