using System.Collections.Generic;
using System.Linq;
using Gameplay.Data;
using Gameplay.Entities;
using Gameplay.Units;
using Save.Data;
using UnityEngine;
using Zenject;

namespace Gameplay.Arrangement.Saving
{
    public class EntitiesSavingSystem : SavingSystem<EntitiesSaveSystem>
    {
        protected override string LoadKey => EntitiesSaveSystem.LoadKey;

        [Inject] private IGetUnitByIdService GetUnitByIdService { get; set; }
        [Inject] private GameDataRegistry GameDataRegistry { get; set; }
        [Inject] private PoolFactory<Entity> Factory { get; set; }
        
        public override void ReproduceFromSaveData(EntitiesSaveSystem payload)
        {
            foreach (EntitySaveData entityData in payload.entities)
            {
                GameObject prefab = GameDataRegistry.Get<GameObject>(entityData.prefabName);
                Vector2 position = entityData.position.ToVector2();
                Unit instigator = GetUnitByIdService.GetUnitById(entityData.instigatorId);
                
                Entity entity = Factory.Get(prefab);
                
                entity.InitEntity(position, instigator, entityData.duration);
            }
        }

        public override ISaveSystem Save()
        {
            EntitySaveData[] entities = Factory.Pool
                .Where(e => e.gameObject.activeSelf)
                .Select(e => e.Save())
                .ToArray();
            
            return new EntitiesSaveSystem(entities);
        }
    }
}