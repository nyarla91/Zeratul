using Gameplay.Player;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Gameplay.Units.View
{
    public class UnitSelectionView : MonoBehaviour
    {
        [SerializeField] private Unit _unit;
        [SerializeField] private SpriteRenderer _mainSpriteRenderer;
        [SerializeField] private SpriteRenderer _overlaySpriteRenderer;
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _selectedMaterial;
        [SerializeField] private Material _selectedEnemyMaterial;
        [SerializeField] private Material _highlightedMaterial;

        [Inject] private PlayerSelection Selection { get; set; }

        private void Start()
        {
            this.UpdateAsObservable()
                .Subscribe(_ => UpdateMaterial());
        }

        private void UpdateMaterial()
        {
            _overlaySpriteRenderer.sprite = _mainSpriteRenderer.sprite;
            if (_unit.IsHighlighted)
                _overlaySpriteRenderer.material = _highlightedMaterial;
            else
            {
                if (_unit.IsSelected)
                    _overlaySpriteRenderer.material = _unit.Ownership.OwnedByEnemy ? _selectedEnemyMaterial : _selectedMaterial;
                else
                    _overlaySpriteRenderer.material = _defaultMaterial;
            }
        }
    }
}