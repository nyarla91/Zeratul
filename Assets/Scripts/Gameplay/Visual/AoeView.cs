using System;
using Gameplay.Data;
using Gameplay.Data.Effects;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Gameplay.Visual
{
    public class AoeView : PoolElement<AoeView>
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private AoeVariant _startingVariant;
        
        private float _rotationSpeed;

        public float Radius
        {
            get => _spriteRenderer.transform.localScale.x;
            set => _spriteRenderer.transform.localScale = Vector3.one * value;
        }

        public void Set(AoeVariant variant)
        {
            _spriteRenderer.sprite = variant.Sprite;
            _spriteRenderer.color = variant.Color;
            Radius = variant.Radius;
            _rotationSpeed = variant.RotationSpeed;
        }
        
        public override void OnSpawn() => Show();
        
        public void Show() => _spriteRenderer.enabled = true;

        public void Hide() => _spriteRenderer.enabled = false;
        
        public void Move(Vector2 position) => transform.position = position;
        
        protected override void OnDespawn() => Hide();

        private void Awake()
        {
            Set(_startingVariant);
            transform.localScale = new Vector3(1, 0.5f, 1);
            this.UpdateAsObservable()
                .Subscribe(_ => UpdateRotation());
        }

        private void UpdateRotation()
        {
            float z = Time.time * _rotationSpeed % 360f;
            _spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, z);
        }
    }

    [Serializable]
    public struct AoeVariant
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private AoeSpriteStep[] _steps;
        [SerializeField] private Color _color;
        [SerializeField] private ReferenceIRadiusSource _radiusSource;
        [SerializeField] private float _radius;
        [SerializeField] private float _rotationSpeed;

        public Sprite Sprite
        {
            get
            {
                Sprite result = _sprite;
                foreach (AoeSpriteStep step in _steps)
                {
                    if (_radius < step.MinRadius)
                        break;
                    result = step.Sprite;
                }
                return result;
            }
        }
        
        public Color Color => _color;
        public float Radius => _radiusSource?.I?.Radius ?? _radius;
        public float RotationSpeed => _rotationSpeed;

        public AoeVariant(ReferenceIRadiusSource radiusSource, float radius, Sprite sprite, AoeSpriteStep[] steps, Color color, float rotationSpeed)
        {
            _radiusSource = radiusSource;
            _sprite = sprite;
            _steps = steps;
            _color = color;
            _radius = radius;
            _rotationSpeed = rotationSpeed;
        }

        public AoeVariant WithRadius(float radius)
        {
            AoeVariant result = this;
            result._radiusSource = null;
            result._radius = radius;
            return result;
        }

        [Serializable]
        public struct AoeSpriteStep
        {
            [SerializeField] private Sprite _sprite;
            [SerializeField] private float _minRadius;
            
            public Sprite Sprite => _sprite;
            public float MinRadius => _minRadius;
        }
    }
}