using System;
using _Core;
using Gameplay.Data;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Gameplay.Visual
{
    public class AoeView : PoolElement<AoeView>
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private int _segments;
        [SerializeField] private float _widthMultiplier;
        [SerializeField] private AoeVariant _startingVariant;
        
        private float _rotationSpeed;
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
            Set(_startingVariant);
            transform.localScale = new Vector3(1, 0.5f, 1);
        }

        public void Set(AoeVariant variant)
        {
            _lineRenderer.material = variant.Material;
            _lineRenderer.sortingOrder = variant.SortingOrder;
            _lineRenderer.colorGradient = variant.Color.ToGradient();
            SetRadius(variant.Radius);
            _rotationSpeed = variant.RotationSpeed;
        }

        public override void OnSpawn() => Show();

        public void Show() => _lineRenderer.enabled = true;

        public void Hide() => _lineRenderer.enabled = false;

        public void Move(Vector2 position) => transform.position = position;

        protected override void OnDespawn() => Hide();

        private void SetRadius(float radius)
        {
            _lineRenderer.positionCount = _segments;
            Vector3[] positions = new Vector3[_segments];
            for (int i = 0; i < _segments; i++)
            {
                float angle = 360f / _segments * i;
                positions[i] = angle.DegreesToVector2() * radius;
            }
            _lineRenderer.SetPositions(positions);
        }

        private void Update()
        {
            float z = Time.time * _rotationSpeed % 360f;
            _lineRenderer.transform.rotation = Quaternion.Euler(0, 0, z);
            
            float width = _mainCamera.orthographicSize * _widthMultiplier;
            _lineRenderer.widthMultiplier = width;
        }
    }

    [Serializable]
    public struct AoeVariant
    {
        [SerializeField] private Material _material;
        [SerializeField] private Color _color;
        [SerializeField] private int _sortingOrder;
        [SerializeField] private ReferenceIRadiusSource _radiusSource;
        [SerializeField] private float _radius;
        [SerializeField] private float _rotationSpeed;

        public Material Material => _material;
        public int SortingOrder => _sortingOrder;
        public Color Color => _color;
        public float Radius => _radiusSource?.I?.Radius ?? _radius;
        public float RotationSpeed => _rotationSpeed;

        public AoeVariant WithRadius(float radius)
        {
            AoeVariant result = this;
            result._radiusSource = null;
            result._radius = radius;
            return result;
        }
    }
}