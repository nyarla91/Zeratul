using Gameplay.Units;
using UnityEngine;

namespace Gameplay
{
    public class ScenarioInstantiation : MonoBehaviour
    {
        [SerializeField] private Transform _unitSpawnPointsParent;
        
        public UnitSpawnPoint[] UnitSpawnPoints => _unitSpawnPointsParent.GetComponentsInChildren<UnitSpawnPoint>();
    }
}