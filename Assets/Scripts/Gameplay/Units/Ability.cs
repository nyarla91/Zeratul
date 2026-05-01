using Extentions;
using Extentions.Pause;
using Gameplay.Data.Abilities;
using UnityEngine;

namespace Gameplay.Units
{
    public class Ability
    {
        private readonly GameTime _gameTime;
        
        public AbilityType Type { get; }
        public Unit Caster { get; }
        public int LastCastFrame { get; set; } = -1000;

        public int CooldownLeft => Mathf.Max(Type.Cooldown - _gameTime.Frame + LastCastFrame, 0);
        public bool IsReady => CooldownLeft == 0;

        public Ability(AbilityType type, Unit caster, GameTime gameTime)
        {
            _gameTime = gameTime;
            Type = type;
            Caster = caster;
        }

        public void StartCooldown()
        {
            LastCastFrame = _gameTime.Frame;
        }

        public bool CanBeCast(OrderTarget target) => Type.CanBeCast(this, target);
    }
}