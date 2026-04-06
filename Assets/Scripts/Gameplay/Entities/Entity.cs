using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.Entities
{
    public class Entity : PoolElement<Entity>
    {
        public int Lifetime { get; set; }
        
        [Inject] private TacticalPause TacticalPause { get; set; }

        private void Awake()
        {
            Observable.EveryFixedUpdate()
                .Where(_ => IsSpawned)
                .Where(_ => TacticalPause.IsUnpaused)
                .Subscribe(_ => TickLifetime());
        }

        public override void OnSpawn() { }

        protected override void OnDespawn() { }

        private void TickLifetime()
        {
            Lifetime--;
            if (Lifetime == 0)
                Despawn();
        }
    }
}