using System;
using _Core;
using UnityEngine;
using UnityEngine.UI;
using Range = _Core.Range;

namespace UIUtility
{
    public class LoadingAnimation : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private float _animationSpeed;
        [SerializeField] private Range _scaleRange;
        [SerializeField] private Range _alphaRange;

        private void Update()
        {
            float t = MathExtentions.TimeSin(0, 1, _animationSpeed);
            transform.localScale = Vector3.one * Mathf.Lerp(_scaleRange.Min, _scaleRange.Max, t);
            float alpha = Mathf.Lerp(_alphaRange.Min, _alphaRange.Max, t);
            _image.color = _image.color.WithA(alpha);
        }
    }
}