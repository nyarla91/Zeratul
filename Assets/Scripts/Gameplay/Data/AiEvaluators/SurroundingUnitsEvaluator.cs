using System.Collections.Generic;
using System.Linq;
using Gameplay.Data.Validator;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Data.AiEvaluators
{
    [CreateAssetMenu(menuName = "Gameplay Data/AI Evaluator/Surrounding Units", order = 0)]
    public class SurroundingUnitsEvaluator : AiUnitTargetEvaluator
    {
        [SerializeField] private SOInjectPresenter _gameplayPresenter;
        [SerializeField] private ReferenceIRadiusSource _radiusSource; 
        [SerializeField] private UnitValidatorGroup _surroundingValidator; 
        [SerializeField] private float _worthForEach; 
        
        [Inject] private IsometricOverlap Overlap { get; set; } 
        
        public override float EvaluteTargetWorth(Unit agent, Unit target)
        {
            _gameplayPresenter.Inject(this);
            Overlap.TryGetUnits(target.Position, _radiusSource.I.Radius, out HashSet<Unit> surroundingUnits);
            return surroundingUnits.Count(u => _surroundingValidator.IsValid(target, u)) * _worthForEach;
        }
    }
}