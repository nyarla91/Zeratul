using System;
using Extentions;
using Gameplay.UI;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Units.View
{
    public class UnitBarsView : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Unit _unit;
        [SerializeField] private FillBarView _hitPointsBar;
        [SerializeField] private FillBarView _shieldPointsBar;
        [SerializeField] private FillBarView _energyPointsBar;
        [SerializeField] private Image _hitPointsFill;
        [SerializeField] private Gradient _hitPointsGradient;

        private RectTransform _canvasRectTransform;
        
        private void Start()
        {
            float canvasWidth = _unit.Type.Size;
            float canvasYOffset = _unit.Type.SpriteMap.SpriteHeight - _unit.Type.Size * Isometry.VerticalScale * 0.8f;
            _canvasRectTransform = _canvas.GetComponent<RectTransform>();
            _canvasRectTransform.sizeDelta = _canvasRectTransform.sizeDelta.WithX(canvasWidth);
            _canvasRectTransform.localPosition = new Vector3(0, canvasYOffset);

            IObservable<float> observableHitPoints = _unit.ObserveEveryValueChanged(u => u.Life.HitPercent);
            _hitPointsBar.SubscribeToPercent(observableHitPoints);
            observableHitPoints.Subscribe(UpdateHitPointsColor);
            
            if (_unit.Life.HasShieldPoints)
                _shieldPointsBar.SubscribeToPercent(_unit.ObserveEveryValueChanged(u => u.Life.ShieldPercent));
            else
                _shieldPointsBar.Hide();
            
            if (_unit.Abilities.HasEnergyPoints)
                _energyPointsBar.SubscribeToPercent(_unit.ObserveEveryValueChanged(u => u.Abilities.EnergyPercent));
            else
                _energyPointsBar.Hide();
            
            _unit.ObserveEveryValueChanged(u => u.Visibility.IsVisibleToPlayer)
                .Subscribe(v => _canvas.enabled = v);
        }

        private void UpdateHitPointsColor(float percent)
        {
            _hitPointsFill.color = _hitPointsGradient.Evaluate(percent);
        }
    }
}