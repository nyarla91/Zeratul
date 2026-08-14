using System;
using _Core;
using UnityEngine;

namespace Gameplay.Units.View
{
    public class UnitMinimapPipView : MonoBehaviour
    {
        [SerializeField] private Unit _unit;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _defaultScale;
        [SerializeField] private float _selectedScale;
        [SerializeField] private Color _playerColor;
        [SerializeField] private Color _allyColor;
        [SerializeField] private Color _neutralColor;
        [SerializeField] private Color _enemyColor;

        private void Update()
        {
            if ( ! _unit.CanBeTargetedByPlayer || _unit.Type.HideOnMinimap)
            {
                _spriteRenderer.enabled = false;
                return;
            }
            _spriteRenderer.enabled = true;
            _spriteRenderer.color = _unit.Alliance.CurrentOwner switch
            {
                Owner.Player => _playerColor,
                Owner.Ally => _allyColor,
                Owner.Neutral => _neutralColor,
                Owner.Enemy => _enemyColor,
                _ => throw new ArgumentOutOfRangeException()
            };
            transform.localScale = (_unit.IsSelected ? _selectedScale : _defaultScale) * Vector3.one;
        }
    }
}