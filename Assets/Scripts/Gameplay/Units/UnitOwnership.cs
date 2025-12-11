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
            if (OwnedByPlayer)
                PlayerOwnership.AddOwnedUnit(this);
        }

        private void OnDestroy()
        {
            if (OwnedByPlayer)
                PlayerOwnership.RemoveOwnedUnit(this);
        }

    }
}