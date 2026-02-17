using Extentions;
using Gameplay.UI;
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
        }

        private void Update()
        {
            if ( ! _unit.Visibility.IsVisibleToPlayer)
            {
                _canvas.enabled = false;
                return;
            }
            _canvas.enabled = true;
            
            _hitPointsBar.UpdateBar(_unit.Life.HitPercent);
            
            if (_unit.Life.HasShieldPoints)
                _shieldPointsBar.UpdateBar(_unit.Life.ShieldPercent);
            else
                _shieldPointsBar.Hide();
            
            if (_unit.Abilities.HasEnergyPoints)
                _energyPointsBar.UpdateBar(_unit.Abilities.EnergyPercent);
            else
                _energyPointsBar.Hide();
        }
            
    }
}