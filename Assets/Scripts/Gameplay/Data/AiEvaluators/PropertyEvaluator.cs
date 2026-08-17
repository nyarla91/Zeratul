using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.AiEvaluators
{
    [CreateAssetMenu(menuName = "Gameplay Data/AI Evaluator/Unit Property", order = 0)]
    public class PropertyEvaluator : AiUnitTargetEvaluator
    {
        [SerializeField] private bool _countHitPoints;
        [SerializeField] private bool _countShieldPoints;
        [SerializeField] private bool _countEnergy;
        [SerializeField] private bool _countMissing;
        [SerializeField] private bool _countPercentage;
        [SerializeField] private float _multiplier = 1;
        public override float EvaluteTargetWorth(Unit agent, Unit target)
        {
            if (target.Type.IsInvulnerable)
                return 0;
            
            float result = 0;
            
            if (_countHitPoints && target.HasLife)
                result += _countMissing ? target.Life.MissingHitPoints : target.Life.HitPoints;
            if (_countShieldPoints && target.HasLife)
                result += _countMissing ? target.Life.MissingShieldPoints : target.Life.ShieldPoints;
            if (_countEnergy)
                result += _countMissing ? target.Abilities.MissingEnergyPoints : target.Abilities.EnergyPoints;

            if (_countPercentage)
            {
                float max = (_countHitPoints ? target.Life.MaxHitPoints : 0) + (_countShieldPoints ? target.Life.MaxShieldPoints : 0);
                result /= max;
            }
            return result * _multiplier;
        }
    }
}