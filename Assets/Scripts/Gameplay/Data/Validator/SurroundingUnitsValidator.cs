using System.Linq;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Surrounding Units", order = 0)]
    public class SurroundingUnitsValidator : UnitValidator
    {
        [SerializeField] private SOInjectPresenter _gameplayPresenter;
        [SerializeField] private UnitValidatorGroup _surroundingValidators;
        [SerializeField] private float _radius;
        [SerializeField] private int _maxValid;
        [SerializeField] private int _minValid;

        [Inject] private IsometricOverlap Overlap { get; set; }
        
        public override bool IsValid(Unit actor, Unit target)
        {
            _gameplayPresenter.Inject(this);
            
            if ( ! Overlap.TryGetUnits(target.Position, _radius, out Unit[] units))
                return _minValid <= 0 && _maxValid >= 0;

            int validUnits = units
                .Count(u => u != target && _surroundingValidators.IsValid(target, u));
            
            return validUnits >= _minValid && validUnits <= _maxValid;
        }
    }
}