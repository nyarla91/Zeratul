using Gameplay.Player;
using UnityEngine;
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
            _spriteRenderer.transform.localPosition = _unit.Type.Graphics.SpriteHeight * Vector2.up;
            transform.localScale = _unit.Type.Movement.Size * Vector3.one;
        }

        private void Update()
        {
            _spriteRenderer.color = _playerSelection.IsUnitSelected(_unit) ? _selectedColor : Color.clear;
        }
    }
}