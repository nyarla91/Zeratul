using System;
using Extentions;
using Gameplay.UI;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Units.View
{
    public class UnitStatsView : MonoBehaviour
    {
        [SerializeField] private Unit _unit;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Image _hitPoints;
        [SerializeField] private Image _shieldPoints;
        [SerializeField] private Image _energyPoints;
        [SerializeField] private Color _playerColor;
        [SerializeField] private Color _enemyColor;

        private RectTransform _canvasRectTransform;
        
        private void Start()
        {
            float canvasSize = _unit.Type.Size;
            float canvasYOffset = _unit.Type.SpriteMap.SpriteHeight;
            _canvasRectTransform = _canvas.GetComponent<RectTransform>();
            _canvasRectTransform.localScale = new Vector3(1, 0.5f, 1) * canvasSize;
            _canvasRectTransform.localPosition = new Vector3(0, canvasYOffset);

            if ( ! _unit.Life.HasShieldPoints)
                _shieldPoints.enabled = false;
            if ( ! _unit.Abilities.HasEnergyPoints)
                _energyPoints.enabled = false;
            
            UpdateOwnershipColor(_unit.Ownership.OwnedByPlayer);
            _unit.ObserveEveryValueChanged(u => u.Ownership.OwnedByPlayer)
                .Subscribe(UpdateOwnershipColor);
            
            _unit.ObserveEveryValueChanged(u => u.Visibility.IsVisibleToPlayer)
                .Subscribe(v => _canvas.enabled = v);
            
            Observable.EveryFixedUpdate()
                .Subscribe(_ => UpdateStats());
        }

        private void UpdateOwnershipColor(bool ownedByPlayer)
        {
            _hitPoints.color = ownedByPlayer ? _playerColor : _enemyColor;
        }

        private void UpdateStats()
        {
            _hitPoints.fillAmount = _unit.Life.HitPercent;
            _shieldPoints.fillAmount = _unit.Life.ShieldPercent;
            _energyPoints.fillAmount = _unit.Abilities.EnergyPercent;
        }
    }
}