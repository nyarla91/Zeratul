using System;
using System.Text;
using Extentions;
using Gameplay.Data;
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
        [SerializeField] private bool _ownedByPlayer;

        [Inject] private ContainerInstantiator Instantiator { get; set; }
        
        private void Start()
        {
            Unit unit = Instantiator.Instantiate<Unit>(_prefab, transform.position, transform.parent);
            unit.gameObject.name = gameObject.name;
            unit.Init(_unitType, _ownedByPlayer);
            Destroy(gameObject);
        }

        public void OnValidate()
        {
            _spriteRenderer.sprite = _unitType?.SpriteMap.GenericSprite;
            _spriteRenderer.color = _ownedByPlayer ? Color.green : Color.red;
            
            StringBuilder name =  new();
            name.Append($"{(_ownedByPlayer ? "Player" : "Enemy")} - ");
            name.Append(_unitType?.DisplayName ?? "No unit");
            gameObject.name = name.ToString();
        }
    }
}