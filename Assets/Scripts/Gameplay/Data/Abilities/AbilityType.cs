using Extentions;
using Extentions.Pause;
using Gameplay.Data.Orders;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Abilities
{
    [CreateAssetMenu(menuName = "Gameplay Data/Ability", order = 0)]
    public class AbilityType : ScriptableObject
    {
        [SerializeField] private TargetRequirement _targetRequirement;
        [SerializeField] private float _targetRadius;
        [SerializeField] private float _cooldown;

        public TargetRequirement TargetRequirement => _targetRequirement;
        public float TargetRadius => _targetRadius;
        public float Cooldown => _cooldown;

        public bool TryCast(Ability ability, OrderTarget target)
        {
            if ( ! ability.IsReady)
                return false;
            Debug.Log($"{ability.Caster} casts {this} targeting {target.Point} {target.Unit}");
            ability.StartCooldown();
            return true;
        }
    }

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
            CooldownTimer.Restart();
        }
    }
}