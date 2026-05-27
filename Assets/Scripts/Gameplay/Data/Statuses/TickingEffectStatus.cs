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
        [SerializeField] private bool _requireSimulation;
        
        public override void OnAdd(Status status)
        {
            
        }

        public override void OnUpdate(Status status)
        {
            if (status.FramesSinceAddition % _tickPeriod != 0)
                return;
            if (_requireSimulation && ! status.Host.IsSimulated)
                return;
            if (status.IsLocked)
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