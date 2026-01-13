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

        public Ability(AbilityType type, Unit caster, IPauseReadonly pauseReadonly)
        {
            Type = type;
            Caster = caster;
            CooldownTimer = new Timer(caster, Type.Cooldown, pauseReadonly);
        }

        public void StartCooldown()
        {
            CooldownTimer.Duration = Type.Cooldown;
            CooldownTimer.Restart();
        }
    }
}