using Extentions;
using Gameplay.Data.Effects;
using UnityEngine;

namespace Gameplay.Data.Statuses
{
    [CreateAssetMenu(menuName = "Gameplay Data/Statuses/Ticking Unit Effect", order = 0)]
    public class TickingUnitEffectStatus : StatusType
    {
        [SerializeField] private EffectTargetingUnit _effect;
        [SerializeField] private int _tickPeriod;
        [SerializeField] private bool _applyOnRemoval;
        
        public override void OnAdd(Status status)
        {
            
        }

        public override void OnUpdate(Status status)
        {
            if (status.ExpirationTimer.FramesElapsed % _tickPeriod == 0)
            {
                _effect.Apply(status.Instigator, status.Host);
            }
        }

        public override void OnRemove(Status status)
        {
            if (_applyOnRemoval)
            {
                _effect.Apply(status.Instigator, status.Host);
            }
        }
    }
}