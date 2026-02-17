using System.Collections.Generic;
using System.Linq;
using Extentions;
using Gameplay.Data.Configs;
using Gameplay.Enviroment;
using UnityEngine;

namespace Gameplay.Units.View
{
    public class UnitAnimationView : MonoBehaviour
    {
        [SerializeField] private SpriteLayeringConfig _config;
        [SerializeField] private PolygonCollider2D _collider;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Unit _unit;

        private int _defaultSortingOrder;
        
        private void Start()
        {
            _spriteRenderer.transform.localPosition = _unit.Type.SpriteMap.SpriteHeight * Vector2.up;
            _defaultSortingOrder = _unit.Type.IsAir
                ? (_config.UnitBaseOrder + _config.AirUnitOrderBonus)
                : _config.UnitBaseOrder;
        }

        private void Update()
        {
            if (!_unit.Visibility.IsVisibleToPlayer)
            {
                _spriteRenderer.sprite = null;
                return;
            }
            _spriteRenderer.sprite = _unit.Type.SpriteMap.GetSpriteForAngle(_unit.Movement.LookAngle);
            _spriteRenderer.sortingOrder = CalculateSortingOrder();
        }

        private int CalculateSortingOrder()
        {
            List<Collider2D> overlap = new();
            _collider.Overlap(overlap);
            SpriteOverlapArea[] areas = overlap.Select(c => c.GetComponent<SpriteOverlapArea>()).NoNull();
            if (areas.Length == 0)
                return _defaultSortingOrder;
            return areas.Max(a => a.SortingOrder) - 1;
        }

        private void LateUpdate()
        {
            float z = transform.position.y * _config.VerticalZScale;
            transform.position = transform.position.WithZ(z);
        }
    }
}