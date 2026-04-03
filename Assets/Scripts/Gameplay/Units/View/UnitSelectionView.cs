using Gameplay.Player;
using UniRx;
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
        [SerializeField] private Material _highlightedMaterial;

        [Inject] private PlayerSelection Selection { get; set; }
        [Inject] private PlayerMouseTargeting MouseTargeting { get; set; }

        private void Start()
        {
            Observable.EveryUpdate()
                .Subscribe(_ => UpdateMaterial());
        }

        private void UpdateMaterial()
        {
            _overlaySpriteRenderer.sprite = _mainSpriteRenderer.sprite;
            _overlaySpriteRenderer.material = MouseTargeting.Unit == _unit
                ? _highlightedMaterial : (Selection.IsUnitSelected(_unit) ?  _selectedMaterial : _defaultMaterial);
        }
    }
}