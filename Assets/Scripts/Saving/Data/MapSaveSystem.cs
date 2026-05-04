using System;
using Extentions;

namespace Saving.Data
{
    [Serializable]
    public class MapSaveSystem : ISaveSystem
    {
        public static string LoadKey => "map";
        public string SaveKey => LoadKey;

        public SerializableVector2 cameraPosition;
        public float cameraSize;
        public bool[,] scoutedFogOfWar;

        public MapSaveSystem(SerializableVector2 cameraPosition, float cameraSize, bool[,] scoutedFogOfWar)
        {
            this.cameraPosition = cameraPosition;
            this.cameraSize = cameraSize;
            this.scoutedFogOfWar = scoutedFogOfWar;
        }
    }
}