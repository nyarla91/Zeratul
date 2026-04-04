using Gameplay.Data.Validator;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.AiEvaluators
{
    public class ValidatorEvaluator : AiUnitTargetEvaluator
    {
        [SerializeField] private UnitValidatorGroup _validator;
        [SerializeField] private int _validWorth;
        [SerializeField] private int _invalidWorth;

        public override float EvaluteTargetWorth(Unit agent, Unit target) =>
            _validator.IsValid(agent, target) ? _validWorth : _invalidWorth;
    }
}