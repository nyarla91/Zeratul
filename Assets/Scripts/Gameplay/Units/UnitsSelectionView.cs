using System;
using Gameplay.Player;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    public class UnitsSelectionView : MonoBehaviour
    {
        [SerializeField] private Unit _model;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Color _selectedColor;
        
        [Inject] private PlayerSelection _playerSelection;

        private void Start()
        {
            transform.localScale = Vector3.one * _model.Type.Movement.Size;
        }

        private void Update()
        {
            _spriteRenderer.color = _playerSelection.IsUnitSelected(_model) ? _selectedColor : Color.clear;
        }
    }
}