using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Data;

namespace Gameplay.Units
{
    public class UnitOwnership : UnitComponent
    {
        private readonly Dictionary<object, bool> _owners = new();

        public bool OwnedByPlayer => _owners.Values.Last();
        public bool OwnedByEnemy => ! OwnedByPlayer;

        public event Action<bool> OwnerUpdated;
        
        public UnitOwnership(Unit unit, bool ownedByPlayer) : base(unit)
        {
            _owners.Add(this, ownedByPlayer);
        }
        
        public void AddOwner(object owner, bool ownedByPlayer)
        {
            _owners.TryAdd(owner, ownedByPlayer);
            OwnerUpdated?.Invoke(OwnedByPlayer);
        }

        public void RemoveOwner(object owner)
        {
            _owners.Remove(owner);
            OwnerUpdated?.Invoke(OwnedByPlayer);
        }

        public bool IsFriendly(Unit other) => OwnedByPlayer == other.Ownership.OwnedByPlayer;
        
        public bool IsHostile(Unit other) => ! IsFriendly(other);
    }
}