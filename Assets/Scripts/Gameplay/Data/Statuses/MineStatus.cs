using System.Linq;
using Gameplay.Data.Effects;
using Gameplay.Data.Validator;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Data.Statuses
{
    [CreateAssetMenu(menuName = "Gameplay Data/Statuses/Mine", order = 0)]
    public class MineStatus : StatusType
    {
        [SerializeField] private SOInjectPresenter _gameplayPresenter;
        [SerializeField] private float _triggerRadius;
        [SerializeField] private int _cooldown;
        [SerializeField] private UnitValidatorGroup _triggerValidator;
        [SerializeField] private EffectTargetingUnit[] _triggerEffects;
        [SerializeField] private EffectTargetingUnit[] _mineTriggerEffects;
        
        [Inject] private IsometricOverlap Overlap { get; } 
        
        public override void OnAdd(Status status)
        {
            _gameplayPresenter.Inject(this);
        }

        public override void OnUpdate(Status status)
        {
            if (status.CurrentFrame % 5 != 0)
                return;
            if ( ! status.Host.Simulation.IsSimulated)
                return;
            if (status.IsLocked)
                return;

            Overlap.TryGetUnits(status.Host.Position, _triggerRadius, out Unit[] triggeringUnits);
            Unit triggeringUnit = triggeringUnits.FirstOrDefault(u => _triggerValidator.IsValid(status.Host, u));
            if ( ! triggeringUnit)
                return;
            
            foreach (EffectTargetingUnit effect in _triggerEffects)
            {
                effect.Apply(status.Host, triggeringUnits[0]);
            }

            foreach (EffectTargetingUnit effect in _mineTriggerEffects)
            {
                effect.Apply(status.Host, status.Host);
            }
            
            status.Host.Kill();
        }

        public override void OnRemove(Status status)
        {
            
        }
    }
}