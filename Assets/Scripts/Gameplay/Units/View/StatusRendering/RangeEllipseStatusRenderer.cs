using System;
using Gameplay.Data.Statuses;
using Gameplay.Visual;
using UnityEngine;
using Zenject;

namespace Gameplay.Units.View.StatusRendering
{
    public class RangeEllipseStatusRenderer : StatusRenderer
    {
        [SerializeField] private float _radius;
        [SerializeField] private float _thickness;
        [SerializeField] private Color _playerColor;
        [SerializeField] private Color _enemyColor;
        
        [Inject] private RangeEllipseFactory RangeEllipseFactory { get; set; }

        private RangeEllipse _rangeEllipse;
        
        public override void OnShow(IStatusInfo status)
        {
            base.OnShow(status);
            _rangeEllipse = RangeEllipseFactory.Get();
            Color color = status.Host.Ownership.OwnedByPlayer ? _playerColor : _enemyColor;
            _rangeEllipse.Set(_radius, _thickness, color);
            _rangeEllipse.Show();
        }

        private void Update()
        {
            _rangeEllipse?.Move(transform.position);
        }

        public override void OnHide()
        {
            base.OnHide();
            _rangeEllipse?.Release();
        }
    }
}