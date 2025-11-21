using Gameplay.Player;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    public class UnitOwnership : UnitComponent
    {
        [SerializeField]
        private bool _ownedByPlayer;

        public bool OwnedByPlayer => _ownedByPlayer;

        [field: Inject]
        public PlayerOwnership PlayerOwnership { get; set; }

        private void Awake()
        {
            PlayerOwnership.AddOwnedUnit(this);
        }

        private void OnDestroy()
        {
            PlayerOwnership.RemoveOwnedUnit(this);
        }

    }
}