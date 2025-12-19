using Extentions;
using Extentions.Pause;
using Gameplay.Data.Abilities;

namespace Gameplay.Units
{
    public class Ability
    {
        public AbilityType Type { get; }
        public Unit Caster { get; }
        public Timer CooldownTimer { get; }
        
        public bool IsReady => CooldownTimer.IsIdle;

        public Ability(AbilityType type, Unit caster, IPauseRead pauseRead)
        {
            Type = type;
            Caster = caster;
            CooldownTimer = new Timer(caster, Type.Cooldown, pauseRead);
        }

        public void StartCooldown()
        {
            CooldownTimer.Duration = Type.Cooldown;
            CooldownTimer.Restart();
        }
    }
}