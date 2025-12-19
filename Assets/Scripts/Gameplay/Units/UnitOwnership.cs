using Gameplay.Data;
using Gameplay.Player;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    public class UnitOwnership : UnitComponent
    {
        public bool OwnedByPlayer { get; private set; }

        [field: Inject]
        public PlayerOwnership PlayerOwnership { get; set; }

        public void Init(UnitType unitType, bool ownedByPlayer)
        {
            OwnedByPlayer = ownedByPlayer;
            if (OwnedByPlayer)
                PlayerOwnership.AddOwnedUnit(this);
        }

        public bool IsFriendly(Unit other) => OwnedByPlayer == other.Ownership.OwnedByPlayer;
        
        public bool IsHostile(Unit other) => ! IsFriendly(other);

        private void OnDestroy()
        {
            if (OwnedByPlayer)
                PlayerOwnership.RemoveOwnedUnit(this);
        }

    }
}