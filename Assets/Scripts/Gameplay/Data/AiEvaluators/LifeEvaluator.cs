using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.AiEvaluators
{
    [CreateAssetMenu(menuName = "Gameplay Data/AI Evaluator/Unit Life", order = 0)]
    public class LifeEvaluator : AiUnitTargetEvaluator
    {
        [SerializeField] private bool _countHitPoints;
        [SerializeField] private bool _countShieldPoints;
        [SerializeField] private bool _countMissing;
        [SerializeField] private bool _countPercentage;
        [SerializeField] private float _multiplier = 1;
        public override float EvaluteTargetWorth(Unit agent, Unit target)
        {
            float result = 0;
            
            if (_countHitPoints)
                result += _countMissing ? target.Life.MissingHitPoints : target.Life.HitPoints;
            if (_countShieldPoints)
                result += _countMissing ? target.Life.MissingShieldPoints : target.Life.ShieldPoints;

            if (_countPercentage)
            {
                float max = (_countHitPoints ? target.Life.MaxHitPoints : 0) + (_countShieldPoints ? target.Life.MaxShieldPoints : 0);
                result /= max;
            }
            return result * _multiplier;
        }
    }
}