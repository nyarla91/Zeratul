using System.Linq;
using Gameplay.Data.Validator;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Data.Effects
{
    [CreateAssetMenu(menuName = "Gameplay Data/Effects/AOE", order = 0)]
    public class AOEffect : EffectTargetingPoint
    {
        [SerializeField] private SOInjectPresenter _gameplayerInjectPresenter;
        [Space]
        [SerializeField] private float _radius;
        [SerializeField] private UnitValidatorGroup _applyValidators;
        [SerializeField] private EffectTargetingUnit[] _effects;
        
        [Inject] private IsometricOverlap IsometricOverlap { get; set; }
        
        public override void Apply(Unit caster, Vector2 target)
        {
            _gameplayerInjectPresenter.Inject(this);
            if  ( ! IsometricOverlap.TryGetUnits(target, _radius, out Unit[] units))
                return;
            units = units.Where(unit => _applyValidators.IsValid(caster, unit)).ToArray();
            foreach (Unit unit in units)
            {
                foreach (EffectTargetingUnit effect in _effects)
                {
                    effect.Apply(caster, unit);
                }
            }
        }
    }
}