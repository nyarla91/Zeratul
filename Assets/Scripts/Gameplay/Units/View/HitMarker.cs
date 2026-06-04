using System.Linq;
using _Core;
using UnityEngine;

namespace Gameplay.Units.View
{
    public class HitMarker : PoolElement<HitMarker>
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private float _time;
        [SerializeField] private AnimationCurve _widthFromDamage;
        [SerializeField] private Color _goodColor;
        [SerializeField] private Color _badColor;

        private float _startTimestamp;
        private float _endTimestamp;
        
        public void InitHit(Unit from, Unit to)
        {
            _lineRenderer.positionCount = 2;
            Vector3[] positions = new []{from.InteractionPosition, to.InteractionPosition}.Select(p => (Vector3) p).ToArray();
            _lineRenderer.SetPositions(positions);
            
            Color color = from.Alliance.IsFriendly(Owner.Player) ? _goodColor : _badColor;
            _lineRenderer.colorGradient = color.ToGradient();

            float damage = from.Type.WeaponType.BaseDamage;
            _lineRenderer.widthMultiplier = _widthFromDamage.Evaluate(damage);
            
            _startTimestamp = Time.time;
            _endTimestamp = _startTimestamp + _time;
        }

        private void Update()
        {
            if ( ! IsSpawned)
                return;

            if (Time.time > _endTimestamp)
            {
                Despawn();
                return;
            }
            
            Color color = _lineRenderer.colorGradient.Evaluate(0);
            float alpha = Mathf.InverseLerp(_endTimestamp, _startTimestamp, Time.time);
            _lineRenderer.colorGradient = color.WithA(alpha).ToGradient();
        }

        public override void OnSpawn()
        {
            
        }

        protected override void OnDespawn()
        {
            
        }
    }
}