using _Core;
using Save.Data;
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

        public EntitySaveData Save()
        {
            string prefabName = Prefab.name;
            SerializableVector2 position = SerializableVector2.FromVector2(transform.position);
            int instigatorId = Instigator?.Id ?? -1;
            int duration = DespawnFrame - GameTime.Frame;
            return new EntitySaveData(prefabName, position, instigatorId, Owner, duration);
        }

        public void InitEntity(Vector2 position, Unit instigator, int duration)
        {
            transform.position = position;
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