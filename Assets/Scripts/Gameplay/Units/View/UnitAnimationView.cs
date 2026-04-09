using System.Collections.Generic;
using System.Linq;
using Extentions;
using Gameplay.Data.Configs;
using Gameplay.Enviroment;
using UniRx;
using UniRx.Triggers;
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
            _unit.Stagger.Began += DiscardCurrentActionTime;
            
            _spriteRenderer.transform.localPosition = _unit.Type.SpriteMap.SpriteHeight * Vector2.up;
            _defaultSortingOrder = _unit.Type.IsAir
                ? (_config.UnitBaseOrder + _config.AirUnitOrderBonus)
                : _config.UnitBaseOrder;

            this.UpdateAsObservable()
                .Where(_ => TacticalPause.IsUnpaused)
                .Subscribe(_ => UpdateSprite());
        }

        private void UpdateSprite()
        {
            
            string newAction = GetCurrentUnitAction();
            if (newAction.Equals(_currentAction))
            {
                _currentActionTime += Time.deltaTime;
            }
            else
            {
                DiscardCurrentActionTime();
                _currentAction = newAction;
            }
            
            if (!_unit.Visibility.IsVisibleToPlayer)
            {
                _spriteRenderer.sprite = null;
                return;
            }

            _spriteRenderer.sprite = _unit.Type.SpriteMap.GetSprite(_currentAction, _currentActionTime, _unit.Direction.LookAngle);
            _spriteRenderer.sortingOrder = CalculateSortingOrder();
        }

        private string GetCurrentUnitAction()
        {
            if (_unit.Stagger.IsStaggered)
                return _unit.Stagger.Action;
            if (_unit.Stagger.RecoveryFramesLeft < -1)
                return (_unit.CanMove && _unit.Movement.HasPath) ? "move" : "idle";
            return _unit.Stagger.Action;
        }

        private void DiscardCurrentActionTime()
        {
            _currentActionTime = 0;
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