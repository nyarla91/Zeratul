using _Core;
using Gameplay.Data.Units;
using Save.Data;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    public class UnitSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;

        public int NextId { get; private set; }

        [Inject] private ContainerInstantiator Instantiator { get; set; }
        
        public Unit Spawn(Vector2 position, UnitType type, int id = -1, UnitSpawnInfo spawnInfo = null)
        {
            Unit unit = Instantiator.Instantiate<Unit>(_prefab, position, transform.parent);
            if (id == -1)
            {
                id = NextId;
                NextId++;
            }
            unit.Init(id, type, spawnInfo);
            return unit;
        }

        public void ReproduceFromSaveData(UnitsSaveSystem saveSystem)
        {
            NextId = saveSystem.nextId;
        }
    }
}