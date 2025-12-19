using Extentions;
using Gameplay.Data.Configs;
using UnityEngine;

namespace Gameplay.Units.View
{
    public class UnitAnimationView : MonoBehaviour
    {
        [SerializeField] private SpriteLayeringConfig _config;
        [SerializeField] private Unit _unit;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private void Start()
        {
            _spriteRenderer.transform.localPosition = _unit.Type.Graphics.SpriteHeight * Vector2.up;
            _spriteRenderer.sortingOrder = _unit.Type.Movement.IsAir
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
            _spriteRenderer.sprite = _unit.Type.Graphics.SpriteMap.GetSpriteForAngle(_unit.Movement.LookAngle);
        }

        private void LateUpdate()
        {
            float z = transform.position.y * _config.VerticalZScale;
            transform.position = transform.position.WithZ(z);
        }
    }
}