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
        
        public override void OnAdd(IStatusInfo status)
        {
            base.OnAdd(status);
            _rangeEllipse = RangeEllipseFactory.Get();
            Color color = status.Host.Ownership.OwnedByPlayer ? _playerColor : _enemyColor;
            _rangeEllipse.Set(_radius, _thickness, color);
            _rangeEllipse.Show();
        }

        private void Update()
        {
            if (_rangeEllipse == null)
                return;
            
            _rangeEllipse.Move(transform.position);
            if (Status.Host.Visibility.IsVisibleToPlayer)
                _rangeEllipse.Show();
            else
                _rangeEllipse.Hide();
        }

        public override void OnRemove()
        {
            base.OnRemove();
            _rangeEllipse?.Release();
        }
    }
}