using Extentions;
using Gameplay.Vision;
using Save.Data;
using UnityEngine;

namespace Gameplay.Arrangement.Saving
{
    public class MapSavingSystem : SavingSystem<MapSaveSystem>
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private FogOfWar _fogOfWar;
        
        protected override string LoadKey => MapSaveSystem.LoadKey;
        
        public override void ReproduceFromSaveData(MapSaveSystem payload)
        {
            _camera.transform.position = payload.cameraPosition.ToVector2().WithZ(_camera.transform.position.z);
            _camera.orthographicSize = payload.cameraSize;
            _fogOfWar.ReproduceFromSaveData(payload);
        }

        public override ISaveSystem Save()
        {
            SerializableVector2 cameraPosition = SerializableVector2.FromVector2(_camera.transform.position);
            return new MapSaveSystem(cameraPosition, _camera.orthographicSize, _fogOfWar.Cells);
        }
    }
}