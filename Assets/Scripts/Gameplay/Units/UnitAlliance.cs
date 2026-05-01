using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Gameplay.Data;
using Saving.Data.Units;

namespace Gameplay.Units
{
    public class UnitAlliance : UnitComponent
    {
        protected override string LoadKey => UnitAllianceSaveSystem.LoadKey;

        private Owner _initialOwner; 
        
        private readonly Dictionary<object, Owner> _owners = new();

        public Owner CurrentOwner => _owners.Any() ? _owners.Values.Last() : _initialOwner;
        public bool OwnedByPlayer => CurrentOwner == Owner.Player;
        public bool OwnedByAlly => CurrentOwner == Owner.Ally;
        public bool OwnedByNeutral => CurrentOwner == Owner.Neutral;
        public bool OwnedByEnemy => CurrentOwner == Owner.Enemy;

        public event Action<Owner> OwnerUpdated;
        
        public UnitAlliance(Unit unit, Owner initialOwner) : base(unit)
        {
            _initialOwner = initialOwner;
        }

        public override IUnitSaveSystem Save()
        {
            return new UnitAllianceSaveSystem(_initialOwner);
        }

        public override void ReproduceFromSave(UnitSaveData saveData)
        {
            UnitAllianceSaveSystem system = GetSaveSystem<UnitAllianceSaveSystem>(saveData);
            _initialOwner = system.initialOwner;
        }
        
        public void AddOwner(object source, Owner owner)
        {
            _owners.TryAdd(source, owner);
            OwnerUpdated?.Invoke(CurrentOwner);
        }

        public void RemoveOwner(object source)
        {
            _owners.Remove(source);
            OwnerUpdated?.Invoke(CurrentOwner);
        }

        public bool IsFriendly(Unit other) => IsFriendly(other.Alliance.CurrentOwner);
        
        public bool IsFriendly(Owner other)
        {
            return CurrentOwner switch
            {
                Owner.Player or Owner.Ally => other is Owner.Player or Owner.Ally,
                Owner.Neutral => other == Owner.Neutral,
                Owner.Enemy => other == Owner.Enemy,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public bool IsHostile(Unit other) => IsHostile(other.Alliance.CurrentOwner);
        
        public bool IsHostile(Owner other)
        {
            return CurrentOwner switch
            {
                Owner.Player or Owner.Ally => other == Owner.Enemy,
                Owner.Neutral => false,
                Owner.Enemy => other is Owner.Player or Owner.Ally,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}