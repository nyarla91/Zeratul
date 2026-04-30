using Extentions;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    public class UnitSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;

        private int _nextId;
        
        [Inject] private ContainerInstantiator Instantiator { get; set; }
        
        public void Spawn(UnitSpawnInfo spawnInfo, Vector2 position)
        {
            Unit unit = Instantiator.Instantiate<Unit>(_prefab, position, transform.parent);
            spawnInfo.PatrolPath.Init();
            unit.Init(_nextId, spawnInfo);
            _nextId++;
            Destroy(gameObject);
        }
    }
}