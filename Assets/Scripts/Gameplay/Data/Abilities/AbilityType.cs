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
        [SerializeField] private float _cooldown;
        [SerializeField] private float _targetRadius;

        public TargetRequirement TargetRequirement => _targetRequirement;
        public float Cooldown => _cooldown;
    }

    public class Ability
    {
        public AbilityType Type { get; }
        public Unit Caster { get; }
        public Timer CooldownTimer { get; }

        public Ability(AbilityType type, Unit caster, IPauseRead pauseRead)
        {
            Type = type;
            Caster = caster;
            CooldownTimer = new Timer(caster, Type.Cooldown, pauseRead);
        }
    }
}