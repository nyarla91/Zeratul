using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Units.View
{
    public class UnitStatView : MonoBehaviour
    {
        [SerializeField] private Image _fill;
        [SerializeField] private Image _trailFill;
        [SerializeField] private float _trailSmoothSpeed;
        [SerializeField] private float _trailAbsoluteSpeed;
        [SerializeField] private float _trailIncrementDelay;
        [SerializeField] private float _trailDecrementDelay;

        private float _trailTargetFrame;
        private float _percent;
        private float _trailPercent;

        public void UpdatePercent(float percent)
        {
            if (_percent.Equals(percent))
                return;
            if (percent > _percent)
                _trailTargetFrame = Time.time + _trailIncrementDelay;
            else
                _trailTargetFrame = Time.time + _trailDecrementDelay;
            _percent = percent;
        }

        private void Update()
        {
            float delta = Mathf.Abs(_percent - _trailPercent);
            float maxDelta = (delta * _trailSmoothSpeed + _trailAbsoluteSpeed) * Time.deltaTime;
            
            if (Time.time >= _trailTargetFrame)
                _trailPercent = Mathf.MoveTowards(_trailPercent, _percent, maxDelta);
            
            _fill.fillAmount = Mathf.Min(_percent, _trailPercent);
            _trailFill.fillAmount = delta;
            
            RectTransform trailRect = _trailFill.transform as RectTransform;
            trailRect.rotation = Quaternion.Euler(0f, 0f, -Mathf.Min(_percent, _trailPercent) * 360);
        }
    }
}