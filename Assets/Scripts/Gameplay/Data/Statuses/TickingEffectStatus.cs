using Gameplay.Data.Effects;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Statuses
{
    [CreateAssetMenu(menuName = "Gameplay Data/Statuses/Ticking Effect", order = 0)]
    public class TickingEffectStatus : StatusType
    {
        [SerializeField] private EffectTargetingUnit[] _unitEffects;
        [SerializeField] private EffectTargetingPoint[] _pointEffects;
        [SerializeField] private PointCasterType _pointCasterType;
        [SerializeField] private int _tickPeriod;
        
        public override void OnAdd(Status status)
        {
            
        }

        public override void OnUpdate(Status status)
        {
            if (status.CurrentFrame % _tickPeriod != 0)
                return;
            
            foreach (EffectTargetingUnit effect in _unitEffects)
            {
                effect.Apply(status.Instigator, status.Host);
            }

            Unit pointCaster = _pointCasterType == PointCasterType.Host ? status.Host : status.Instigator;
            foreach (EffectTargetingPoint effect in _pointEffects)
            {
                effect.Apply(pointCaster, status.Host.transform.position);
            }
        }

        public override void OnRemove(Status status)
        {
            
        }

        private enum PointCasterType
        {
            Instigator,
            Host
        }
    }
}