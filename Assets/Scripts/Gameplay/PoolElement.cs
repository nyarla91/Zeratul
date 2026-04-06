using System;
using UnityEngine;

namespace Gameplay
{
    public abstract class PoolElement<TElement> : MonoBehaviour where TElement : PoolElement<TElement>
    {
        private PoolFactory<TElement> Factory { get; set; }
        
        protected bool IsSpawned => gameObject.activeSelf;
        
        public virtual void Init(PoolFactory<TElement> factory)
        {
            if (Factory != null)
                throw new Exception($"{this} is already initialized");
            Factory = factory;
        }
        
        public abstract void OnSpawn();

        protected abstract void OnDespawn();

        public void Despawn()
        {
            OnDespawn();
            Factory.Despawn(this);
        }
    }
}