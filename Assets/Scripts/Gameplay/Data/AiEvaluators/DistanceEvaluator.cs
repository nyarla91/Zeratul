using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.AiEvaluators
{
    [CreateAssetMenu(menuName = "Gameplay Data/AI Evaluator/Unit Distance", order = 0)]
    public class DistanceEvaluator : AiUnitTargetEvaluator
    {
        [SerializeField] private float _maxWorth;
        [SerializeField] private float _minDistance;
        [SerializeField] private float _maxDistance;
        [SerializeField] private bool _isFurtherBetter;
        
        public override float EvaluteTargetWorth(Unit agent, Unit target)
        {
            float distance = Isometry.Distance(agent.Position, target.Position);
            
            float result = Mathf.InverseLerp(_minDistance, _maxDistance, distance);
            result = Mathf.Clamp(result, 0, 1);
            if ( ! _isFurtherBetter)
            {
                result = 1 - result;
            }
            return result * _maxWorth;
        }
    }
}