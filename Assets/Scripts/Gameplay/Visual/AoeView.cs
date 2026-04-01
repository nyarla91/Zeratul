using System;
using UniRx;
using UnityEngine;

namespace Gameplay.Visual
{
    public class AoeView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _startingRadius;

        public float Radius
        {
            get => _spriteRenderer.transform.localScale.x;
            set => _spriteRenderer.transform.localScale = Vector3.one * value;
        }

        public void Set(Sprite sprite, Color color, float radius, float rotationSpeed)
        {
            _spriteRenderer.sprite = sprite;
            _spriteRenderer.color = color;
            Radius = radius;
        }
        
        private void Awake()
        {
            Radius = _startingRadius;
            transform.localScale = new Vector3(1, 0.5f, 1);
            Observable.EveryUpdate()
                .Subscribe(_ => UpdateRotation());
        }

        private void UpdateRotation()
        {
            float z = Time.time * _rotationSpeed % 360f;
            _spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, z);
        }
    }
}