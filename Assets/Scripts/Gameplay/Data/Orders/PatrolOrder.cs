using System;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Orders
{
    [Serializable]
    [CreateAssetMenu(menuName = "Gameplay Data/Orders/Patrol Order", order = 0)]
    public class PatrolOrder : OrderType
    {
        private Vector2 _originalPoint;
        private bool _isMovingBack;
        
        public override TargetRequirement TargetRequirement => TargetRequirement.Point;
        
        public override void OnProceed(Order order)
        {
            _originalPoint = order.Actor.transform.position;
            order.Actor.Movement.Move(order.Target.Point);
        }

        public override void OnUpdate(Order order)
        {
            if (order.Actor.Movement.HasPath)
                return;
            
            Vector2 nextPoint = _isMovingBack ? order.Target.Point : _originalPoint;
            order.Actor.Movement.Move(nextPoint);
            _isMovingBack = !_isMovingBack;
        }

        public override void Dispose(Order order)
        {
            order.Actor.Movement.Stop();
        }

        public override bool IsCarriedOut(Order order) => false;
    }
}