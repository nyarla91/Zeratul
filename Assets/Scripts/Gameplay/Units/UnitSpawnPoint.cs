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
        [SerializeField] private UnitType _unitType;
        [SerializeField] private UnitSpawnInfo _spawnInfo;

        [Inject] private ContainerInstantiator Instantiator { get; set; }

        public UnitType UnitType => _unitType;
        public UnitSpawnInfo SpawnInfo => _spawnInfo;

        public void Dispose()
        {
            Destroy(gameObject);
        }
        
        private void OnValidate()
        {
            _spriteRenderer.sprite = UnitType?.SpriteMap.GetSprite("idle", 0, SpawnInfo.LookAngle);
            _spriteRenderer.color = SpawnInfo.Owner switch
            {
                Owner.Player => Color.green,
                Owner.Ally => Color.cyan,
                Owner.Neutral => Color.yellow,
                Owner.Enemy => Color.red,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            StringBuilder name =  new();
            string ownerLabel = SpawnInfo.Owner switch
            {
                Owner.Player => "Player",
                Owner.Ally => "Ally",
                Owner.Neutral => "Neutral",
                Owner.Enemy => "Enemy",
                _ => throw new ArgumentOutOfRangeException()
            };
            name.Append($"{ownerLabel} - ");
            name.Append(UnitType?.DisplayName ?? "No unit");
            gameObject.name = name.ToString();
        }
    }
}