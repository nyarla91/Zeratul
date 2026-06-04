using System;
using _Core;
using Save.Data.Units;

namespace Gameplay.Units
{
    public class UnitAlliance : UnitComponent
    {
        protected override string LoadKey => UnitAllianceSaveSystem.LoadKey;

        public Owner InitialOwner { get; private set; }
        public Owner CurrentOwner { get; private set; }
        public bool OwnedByPlayer => CurrentOwner == Owner.Player;
        public bool OwnedByAlly => CurrentOwner == Owner.Ally;
        public bool OwnedByNeutral => CurrentOwner == Owner.Neutral;
        public bool OwnedByEnemy => CurrentOwner == Owner.Enemy;

        public event Action<Owner> OwnerUpdated;
        
        public UnitAlliance(Unit unit, Owner owner) : base(unit)
        {
            InitialOwner = CurrentOwner = owner;
        }

        public override IUnitSaveSystem Save()
        {
            return new UnitAllianceSaveSystem(InitialOwner, CurrentOwner);
        }

        public override void ReproduceFromSave(UnitSaveData saveData)
        {
            UnitAllianceSaveSystem system = GetSaveSystem<UnitAllianceSaveSystem>(saveData);
            InitialOwner = system.initialOwner;
            CurrentOwner = system.currentOwner;
        }
        
        public void RevertOwner() => SetOwner(InitialOwner);

        public void SetOwner(Owner owner)
        {
            if (CurrentOwner == owner)
                return;
            CurrentOwner = owner;
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