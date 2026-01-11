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

        public Ability(AbilityType type, Unit caster, IPauseGet pauseGet)
        {
            Type = type;
            Caster = caster;
            CooldownTimer = new Timer(caster, Type.Cooldown, pauseGet);
        }

        public void StartCooldown()
        {
            CooldownTimer.Duration = Type.Cooldown;
            CooldownTimer.Restart();
        }
    }
}