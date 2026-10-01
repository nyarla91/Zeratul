using System;
using _Core;

namespace Save.Data
{
    [Serializable]
    public class MapSaveSystem : ISaveSystem
    {
        public static string LoadKey => "map";
        public string SaveKey => LoadKey;

        public SerializableVector2 cameraPosition;
        public float cameraSize;
        public FogOfWarCell[] cells;
        public bool displayEnemyVision;

        public MapSaveSystem(SerializableVector2 cameraPosition, float cameraSize, FogOfWarCell[] cells, bool displayEnemyVision)
        {
            this.cameraPosition = cameraPosition;
            this.cameraSize = cameraSize;
            this.cells = cells;
            this.displayEnemyVision = displayEnemyVision;
        }
    }
}