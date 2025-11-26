using UnityEngine;

namespace Gameplay.Units.Orders
{
    public abstract class OrderTargetingPoint : Order
    {
        protected Vector2 Target { get; private set; }

        protected OrderTargetingPoint(Unit actor, Vector2 target) : base(actor)
        {
            Target = target;
        }
    }
}