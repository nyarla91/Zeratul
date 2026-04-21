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
        [SerializeField] private GameObject _prefab;
        [Space]
        [SerializeField] private UnitType _unitType;
        [SerializeField] private UnitSpawnInfo _spawnInfo;

        [Inject] private ContainerInstantiator Instantiator { get; set; }
        
        private void Start()
        {
            Unit unit = Instantiator.Instantiate<Unit>(_prefab, transform.position, transform.parent);
            unit.gameObject.name = gameObject.name;
            _spawnInfo.PatrolPath.Init();
            unit.Init(_unitType, _spawnInfo);
            Destroy(gameObject);
        }

        public void OnValidate()
        {
            _spriteRenderer.sprite = _unitType?.SpriteMap.GetSprite("idle", 0, _spawnInfo.LookAngle);
            _spriteRenderer.color = _spawnInfo.OwnedByPlayer ? Color.green : Color.red;
            
            StringBuilder name =  new();
            name.Append($"{(_spawnInfo.OwnedByPlayer ? "Player" : "Enemy")} - ");
            name.Append(_unitType?.DisplayName ?? "No unit");
            gameObject.name = name.ToString();
        }
    }
}