using System;
using System.Linq;
using Gameplay.Data.Orders;
using Gameplay.Units;
using Source.Extentions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.UnitManagement
{
    public class UnitOrderTargetSelector : MonoBehaviour
    {
        private TargetRequirement _currentRequirement;
        private OrderTarget _currentTarget;

        private Vector2 EstimatedPointTarget => Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        private Unit EstimatedUnitTarget
        {
            get
            {
                Collider2D[] overlap = Physics2D.OverlapPointAll(EstimatedPointTarget); 
                Unit[] units = overlap.Select(x => x.transform.GetComponentInParent<Unit>()).ClearNull();
                return units.Length == 0 ? null : units[0];
            }
        }

        private OrderTarget EstimatedPointOrUnitTarget => new OrderTarget(EstimatedPointTarget, EstimatedUnitTarget);
        
        public void StartTargeting(TargetRequirement requirement)
        {
            if (requirement == TargetRequirement.None)
                throw new ArgumentException($"Target is not required");
            
            _currentRequirement = requirement;
        }

        public OrderTarget FinishTargeting()
        {
            if (_currentRequirement == TargetRequirement.None)
                return default;
            
            _currentRequirement = TargetRequirement.None;
            return _currentTarget;
        }

        private void Update()
        {
            _currentTarget = _currentRequirement switch
            {
                TargetRequirement.None => default,
                TargetRequirement.Point => new OrderTarget(EstimatedPointTarget, null),
                TargetRequirement.Unit => new OrderTarget(default, EstimatedUnitTarget),
                TargetRequirement.PointOrUnit => EstimatedPointOrUnitTarget,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

    }
}