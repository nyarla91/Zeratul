using System;
using Extentions;
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
        [SerializeField] private Material _selectedPlayerMaterial;
        [SerializeField] private Material _selectedAllyMaterial;
        [SerializeField] private Material _selectedNeutralMaterial;
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
                {
                    _overlaySpriteRenderer.material = _unit.Alliance.CurrentOwner switch
                    {
                        Owner.Player => _selectedPlayerMaterial,
                        Owner.Ally => _selectedAllyMaterial,
                        Owner.Neutral => _selectedNeutralMaterial,
                        Owner.Enemy => _selectedEnemyMaterial,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    
                }
                else
                    _overlaySpriteRenderer.material = _defaultMaterial;
            }
        }
    }
}