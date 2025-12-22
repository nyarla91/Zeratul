using Extentions;
using Gameplay.UI;
using UnityEngine;

namespace Gameplay.Units.View
{
    public class UnitBarsView : MonoBehaviour
    {
        [SerializeField] private RectTransform _canvas;
        [SerializeField] private Unit _unit;
        [SerializeField] private FillBarView _hitPointsBar;
        [SerializeField] private FillBarView _shieldPointsBar;
        [SerializeField] private FillBarView _energyPointsBar;

        private void Start()
        {
            float canvasWidth = _unit.Type.Movement.Size;
            float canvasYOffset = _unit.Type.Graphics.SpriteHeight - _unit.Type.Movement.Size * Isometry.VerticalScale * 0.8f;
            _canvas.sizeDelta = _canvas.sizeDelta.WithX(canvasWidth);
            _canvas.localPosition = new Vector3(0, canvasYOffset);
        }

        private void Update()
        {
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