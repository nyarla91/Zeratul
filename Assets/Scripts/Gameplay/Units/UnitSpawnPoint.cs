using System;
using System.Text;
using Extentions;
using Gameplay.Data.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    public class UnitSpawnPoint : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private UnitSpawnInfo _spawnInfo;

        [Inject] private ContainerInstantiator Instantiator { get; set; }
        
        public UnitSpawnInfo SpawnInfo => _spawnInfo;

        public void Dispose()
        {
            Destroy(gameObject);
        }
        
        private void OnValidate()
        {
            _spriteRenderer.sprite = SpawnInfo.UnitType?.SpriteMap.GetSprite("idle", 0, SpawnInfo.LookAngle);
            _spriteRenderer.color = SpawnInfo.OwnedByPlayer ? Color.green : Color.red;
            
            StringBuilder name =  new();
            name.Append($"{(SpawnInfo.OwnedByPlayer ? "Player" : "Enemy")} - ");
            name.Append(SpawnInfo.UnitType?.DisplayName ?? "No unit");
            gameObject.name = name.ToString();
        }
    }
}