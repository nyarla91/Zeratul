using Gameplay.Data.Abilities;
using Save.Data.Units;
using UnityEngine;

namespace Gameplay.Units
{
    public class Ability
    {
        private readonly GameTime _gameTime;
        
        public AbilityType Type { get; }
        public Unit Caster { get; }
        public int LastCastFrame { get; private set; } = -1000;
        public int Charges { get; private set; }

        public int CooldownLeft => Mathf.Max(Type.Cooldown - _gameTime.Frame + LastCastFrame, 0);
        public bool IsReady => CooldownLeft == 0;

        public Ability(AbilityType type, Unit caster, GameTime gameTime)
        {
            _gameTime = gameTime;
            Type = type;
            Caster = caster;
            Charges = type.StartingCharges;
        }

        public AbilitySaveData Save() => new(LastCastFrame, Charges);

        public void ReproduceFromSaveData(AbilitySaveData saveData)
        {
            LastCastFrame = saveData.lastCastFrame;
            Charges = saveData.charges;
        }
        
        public void StartCooldown()
        {
            LastCastFrame = _gameTime.Frame;
        }
        
        public void SpendCharges() => Charges -= Type.ChargesToUse;
        
        public void AddCharges(int amount) => Charges += Mathf.Max(amount, 0);

        public bool CanBeCast(OrderTarget target) => Type.CanBeCast(this, target);
    }
}