namespace Gameplay.Units.Orders
{
    public abstract class Order
    {
        protected Unit Actor { get; private set; }
        
        protected Order(Unit actor)
        {
            Actor = actor;
        }

        public abstract void OnProceed();
        
        public abstract void OnUpdate(float deltaTime);
        
        public abstract bool IsCarriedOut();

        public abstract void Dispose();
    }
}