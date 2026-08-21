using System.Collections.Generic;
using System.Linq;
using Gameplay.Data.Statuses;
using Gameplay.Data.Validator;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Data.Effects
{
    [CreateAssetMenu(menuName = "Gameplay Data/Effects/Clear Instigated Statuses", order = 0)]
    public class ClearInstigatedStatuses : EffectTargetingUnit
    {
        [SerializeField] private SOInjectPresenter _gameplayPresenter;
        [SerializeField] private UnitValidatorGroup _hostsValidator;
        [SerializeField] private StatusType[] _statusesCleared;
        
        [Inject] private UnitPool UnitPool { get; set; }
        
        public override void Apply(Unit caster, Unit target)
        {
            _gameplayPresenter.Inject(this);
            HashSet<Unit> affectedUnits = UnitPool.Units
                .Where(u => _hostsValidator.IsValid(target, u))
                .ToHashSet();
            foreach (Unit affectedUnit in affectedUnits)
            {
                foreach (IStatusInfo status in affectedUnit.Statuses.StatusesInfo)
                {
                    if (_statusesCleared.Contains(status.Type) && status.Instigator == target)
                    {
                        affectedUnit.Statuses.RemoveStatus(status.Type);
                        Debug.Log(affectedUnit);
                    }
                }
            }
        }
    }
}