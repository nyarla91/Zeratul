using System.Collections.Generic;
using System.Linq;
using Extentions;
using Gameplay.Data.Configs;
using Gameplay.Enviroment;
using UnityEngine;
using Zenject;

namespace Gameplay.Units.View
{
    public class UnitAnimationView : MonoBehaviour
    {
        [SerializeField] private SpriteLayeringConfig _config;
        [SerializeField] private PolygonCollider2D _collider;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Unit _unit;

        private int _defaultSortingOrder;
        private string _currentAction;
        private float _currentActionTime;
        
        [Inject] private TacticalPause TacticalPause { get; set; }

        private void Start()
        {
            _spriteRenderer.transform.localPosition = _unit.Type.SpriteMap.SpriteHeight * Vector2.up;
            _defaultSortingOrder = _unit.Type.IsAir
                ? (_config.UnitBaseOrder + _config.AirUnitOrderBonus)
                : _config.UnitBaseOrder;
        }

        private void Update()
        {
            if (TacticalPause.IsPaused)
                return;
            
            string newAction = GetCurrentUnitAction();
            if (newAction.Equals(_currentAction))
            {
                _currentActionTime += Time.deltaTime;
            }
            else
            {
                _currentActionTime = 0;
                _currentAction = newAction;
            }
            
            if (!_unit.Visibility.IsVisibleToPlayer)
            {
                _spriteRenderer.sprite = null;
                return;
            }

            _spriteRenderer.sprite = _unit.Type.SpriteMap.GetSprite(_currentAction, _currentActionTime, _unit.Movement.LookAngle);
            _spriteRenderer.sortingOrder = CalculateSortingOrder();
        }

        private string GetCurrentUnitAction()
        {
            if (_unit.Stagger.IsStaggered)
                return _unit.Stagger.Action;
            if (_unit.Stagger.RecoveryFramesLeft < -1)
                return _unit.Movement.HasPath ? "move" : "idle";
            _currentActionTime = 0;
            return _unit.Stagger.Action;
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