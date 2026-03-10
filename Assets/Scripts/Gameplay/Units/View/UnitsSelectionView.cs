using System;
using Gameplay.Player;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Gameplay.Units.View
{
    public class UnitsSelectionView : MonoBehaviour
    {
        [SerializeField] private Unit _unit;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Color _selectedColor;
        
        [Inject] private PlayerSelection _playerSelection;

        private void Start()
        {
            _spriteRenderer.transform.localPosition = _unit.Type.SpriteMap.SpriteHeight * Vector2.up;
            transform.localScale = _unit.Type.Size * Vector3.one;

            _unit.ObserveEveryValueChanged(u => _playerSelection.IsUnitSelected(u))
                .Subscribe(ToggleVisibility);
            
            ToggleVisibility(false);
        }

        private void ToggleVisibility(bool visible)
        {
            _spriteRenderer.color = visible ? _selectedColor : Color.clear;
        }
    }
}