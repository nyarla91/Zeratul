using System;
using System.Linq;
using Extentions;
using Gameplay.Data.Orders;
using Gameplay.Units;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Player
{
    public class PlayerOrderTargetSelector : MonoBehaviour
    {
        [SerializeField] private LayerMask _unitsMask;
        
        private TargetRequirement _currentRequirement;
        private OrderTarget _currentTarget;

        private Vector2 EstimatedPointTarget => Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        private Unit EstimatedUnitTarget
        {
            get
            {
                Collider2D[] overlap = Physics2D.OverlapPointAll(EstimatedPointTarget, _unitsMask); 
                Unit[] units = overlap.Select(x => x.transform.GetComponentInParent<Unit>()).ClearNull();
                if (units.Length == 0)
                    return null;
                Unit unit = units[0];
                if ( ! unit.Visibility.IsVisibleToPlayer)
                    return null;
                return unit;
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

        public OrderTarget GetTargetForRequirement(TargetRequirement requirement)
        {
            return requirement switch
            {
                TargetRequirement.None => default,
                TargetRequirement.Point => new OrderTarget(EstimatedPointTarget, null),
                TargetRequirement.Unit => new OrderTarget(default, EstimatedUnitTarget),
                TargetRequirement.PointOrUnit => EstimatedPointOrUnitTarget,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void Update()
        {
            _currentTarget = GetTargetForRequirement(_currentRequirement);
        }
    }
}