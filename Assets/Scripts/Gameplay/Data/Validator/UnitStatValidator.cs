using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Stat", order = 0)]
    public class UnitStatValidator : UnitPropertyValidator
    {
        [SerializeField] private bool _countHitPoints;
        [SerializeField] private bool _countShieldPoints;
        [SerializeField] private bool _countEnergy;
        [SerializeField] private bool _countMissing;
        [SerializeField] private bool _countPercentage;
        
        protected override int GetUnitProperty(Unit unit)
        {
            if (unit.Type.IsInvulnerable)
                return 0;
            
            int result = 0;
            
            if (_countHitPoints && unit.HasLife)
                result += _countMissing ? unit.Life.MissingHitPoints : unit.Life.HitPoints;
            if (_countShieldPoints && unit.HasLife)
                result += _countMissing ? unit.Life.MissingShieldPoints : unit.Life.ShieldPoints;
            if (_countEnergy)
                result += _countMissing ? unit.Abilities.MissingEnergyPoints : unit.Abilities.EnergyPoints;

            if (_countPercentage)
            {
                float max = (_countHitPoints ? unit.Life.MaxHitPoints : 0) + (_countShieldPoints ? unit.Life.MaxShieldPoints : 0);
                result = Mathf.RoundToInt(result / max * 100);
            }
            return result;
        }
        
    }
}