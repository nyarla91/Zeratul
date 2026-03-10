using Extentions;
using Gameplay.UI;
using UniRx;
using UnityEngine;

namespace Gameplay.Units.View
{
    public class UnitBarsView : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Unit _unit;
        [SerializeField] private FillBarView _hitPointsBar;
        [SerializeField] private FillBarView _shieldPointsBar;
        [SerializeField] private FillBarView _energyPointsBar;

        private RectTransform _canvasRectTransform;
        
        private void Start()
        {
            float canvasWidth = _unit.Type.Size;
            float canvasYOffset = _unit.Type.SpriteMap.SpriteHeight - _unit.Type.Size * Isometry.VerticalScale * 0.8f;
            _canvasRectTransform = _canvas.GetComponent<RectTransform>();
            _canvasRectTransform.sizeDelta = _canvasRectTransform.sizeDelta.WithX(canvasWidth);
            _canvasRectTransform.localPosition = new Vector3(0, canvasYOffset);
            
            _hitPointsBar.SubscribeToPercent(_unit.ObserveEveryValueChanged(u => u.Life.HitPercent));
            
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
    }
}