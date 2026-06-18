using System;
using _Core;
using UnityEngine;
using UnityEngine.UI;

namespace UIUtility
{
    public class ImageAnimator : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private float _frameRate;

        private void Update()
        {
            int index = (int)(_frameRate * Time.time);
            index = index.RepeatIndex(_sprites.Length);
            _image.sprite = _sprites[index];
        }
    }
}