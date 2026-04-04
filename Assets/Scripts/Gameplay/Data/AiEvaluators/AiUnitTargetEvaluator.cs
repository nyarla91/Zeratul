using System;
using System.Linq;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.AI
{
    public abstract class AiUnitTargetEvaluator : ScriptableObject
    {
        public abstract float EvaluteTargetWorth(Unit agent, Unit target);
    }
    
    [Serializable]
    public struct AiUnitTargetEvaluatorGroup
    {
        [SerializeField] private float _baseWorth;
        [SerializeField] private AiUnitTargetEvaluator[] _evaluators;

        public float EvaluteTargetWorth(Unit agent, Unit target) =>
            _baseWorth + _evaluators.Sum(e => e.EvaluteTargetWorth(agent, target));
    }
}