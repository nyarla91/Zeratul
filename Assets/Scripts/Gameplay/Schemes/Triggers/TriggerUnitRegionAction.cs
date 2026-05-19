using System;
using Extentions;
using Gameplay.Data.Validator;
using Gameplay.Schemes.Values.Variables;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Schemes.Triggers
{
    public class TriggerUnitRegionAction : SchemeTrigger
    {
        [SerializeField] private RegionAction _regionAction;
        [SerializeField] private UnitValidatorGroup _validators;
        [SerializeField] private Region _region;
        [SerializeField] private VariableUnit _out;

        private void Awake()
        {
            switch (_regionAction)
            {
                case RegionAction.Enter:
                    _region.UnitEntered += TryTrigger;
                    break;
                case RegionAction.Leave:
                    _region.UnitLeft += TryTrigger;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void TryTrigger(Unit unit)
        {
            if (_validators.IsInvalid(unit, unit)) 
                return;
            _out.Set(unit);
            Trigger();
        }

        private void OnValidate()
        {
            string action = _regionAction switch
            {
                RegionAction.Enter => "entered",
                RegionAction.Leave => "left",
                _ => throw new ArgumentOutOfRangeException()
            };
            gameObject.name = $"Unit {_validators} {action} ({_region?.name})";
        }

        private enum RegionAction
        {
            Enter,
            Leave,
        }
    }
}