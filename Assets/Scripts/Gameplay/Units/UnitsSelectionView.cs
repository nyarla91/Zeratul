using Gameplay.Player;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    public class UnitsSelectionView : MonoBehaviour
    {
        [SerializeField]
        private Unit _model;
        
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        
        [Inject]
        private PlayerSelection _playerSelection;
        
        private void Update()
        {
            _spriteRenderer.color = _playerSelection.IsUnitSelected(_model) ? Color.green : Color.white;
        }
    }
}