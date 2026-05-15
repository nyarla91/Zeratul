using Gameplay.Data.Effects;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Statuses
{
    [CreateAssetMenu(menuName = "Gameplay Data/Statuses/DelayedEffect", order = 0)]
    public class DelayedEffectStatus : StatusType
    {
        [SerializeField] private EffectTargetingUnit[] _effectsOnRemove;
        
        public override void OnAdd(Status status) { }

        public override void OnUpdate(Status status) { }

        public override void OnRemove(Status status)
        {
            foreach (EffectTargetingUnit effect in _effectsOnRemove)
            {
                effect.Apply(status.Instigator, status.Host);
            }
        }
    }
}