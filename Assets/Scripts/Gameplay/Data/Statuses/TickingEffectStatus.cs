using Gameplay.Data.Effects;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Statuses
{
    [CreateAssetMenu(menuName = "Gameplay Data/Statuses/Ticking Effect", order = 0)]
    public class TickingEffectStatus : StatusType
    {
        [SerializeField] private CasterType _casterType = CasterType.Instigator;
        [SerializeField] private EffectTargetingUnit[] _unitEffects;
        [SerializeField] private int _tickPeriod;
        
        public override void OnAdd(Status status)
        {
            
        }

        public override void OnUpdate(Status status)
        {
            if (status.CurrentFrame % _tickPeriod != 0)
                return;
            
            Unit caster = _casterType == CasterType.Host ? status.Host : status.Instigator;
            foreach (EffectTargetingUnit effect in _unitEffects)
            {
                effect.Apply(caster, status.Host);
            }
        }

        public override void OnRemove(Status status)
        {
            
        }

        private enum CasterType
        {
            Instigator,
            Host
        }
    }
}