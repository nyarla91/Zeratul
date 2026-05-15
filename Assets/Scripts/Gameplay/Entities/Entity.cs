using System;
using Extentions;
using UniRx;
using UnityEngine;
using Zenject;
using Unit = Gameplay.Units.Unit;

namespace Gameplay.Entities
{
    public class Entity : PoolElement<Entity>
    {
        public Owner Owner { get; private set; }
        public Unit Instigator { get; private set; }
        public int DespawnFrame { get; private set; }
        
        [Inject] private GameTime GameTime { get; set; }
        
        private void Awake()
        {
            Observable.EveryFixedUpdate()
                .Where(_ => IsSpawned)
                .Subscribe(_ => TickLifetime());
        }

        public void InitEntity(Unit instigator, int duration)
        {
            Instigator = instigator;
            Owner = instigator.Alliance.CurrentOwner;
            DespawnFrame = GameTime.Frame + duration;
        }

        public override void OnSpawn() { }

        protected override void OnDespawn() { }

        private void TickLifetime()
        {
            if (GameTime.Frame == DespawnFrame)
                Despawn();
        }
    }
}