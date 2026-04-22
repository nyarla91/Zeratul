using System;
using System.Linq;
using Gameplay.Pickups;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class ScenarioCompletion : MonoBehaviour
    {
        [SerializeField] private Pickup[] _pickups;
        
        [Inject] private ScenarioLifetime ScenarioLifetime { get; set; }
        
        private void Awake()
        {
            foreach (Pickup pickup in _pickups)
            {
                pickup.PickedUp += CheckCompletion;
            }
        }

        private void CheckCompletion(Unit _)
        {
            if (_pickups.Count(p => p) > 1)
                return;
            ScenarioLifetime.LeaveScenario();
        }
    }
}