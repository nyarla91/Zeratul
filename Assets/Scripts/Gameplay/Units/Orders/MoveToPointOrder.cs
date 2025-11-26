using Gameplay.Pathfinding;
using UnityEngine;

namespace Gameplay.Units.Orders
{
    public class MoveToPointOrder : OrderTargetingPoint
    {
        public MoveToPointOrder(Unit actor, Vector2 target) : base(actor, target)
        {
            
        }

        public override void OnProceed()
        {
            Actor.Movement.Move(Target);
        }

        public override void OnUpdate(float deltaTime)
        {
            
        }

        public override bool IsCarriedOut() => ! Actor.Movement.HasPath;

        public override void Dispose()
        {
            Actor.Movement.Stop();
        }
    }
}