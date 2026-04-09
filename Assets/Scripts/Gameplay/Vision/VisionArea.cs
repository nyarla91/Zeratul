using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Data.Configs;
using NaughtyAttributes;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Unit = Gameplay.Units.Unit;

namespace Gameplay.Vision
{
    public class VisionArea : MonoBehaviour
    {
        [SerializeField] private VisionConfig _config;
        [SerializeField] private CompositeCollider2D _compositeCollider;
        
        private readonly HashSet<VisionSource> _sources = new();
        private readonly HashSet<Unit> _visibleUnits = new();
        private bool _isInitialized;
        
        public bool IsOwnedByPlayer { get; private set; }
        public HashSet<Unit> VisibleUnits => _visibleUnits.ToHashSet();

        private void Awake()
        {
            this.FixedUpdateAsObservable()
                .Sample(TimeSpan.FromSeconds(_config.RecalculationPeriod))
                .Subscribe(_ => Recalculate());
        }

        private void Recalculate()
        {
            foreach (VisionSource source in _sources)
            {
                source.Recalculate();
            }
            _compositeCollider.GenerateGeometry();
        }

        public void Init(bool isOwnedByPlayer)
        {
            if (_isInitialized)
                return;
            _isInitialized = true;
            IsOwnedByPlayer = isOwnedByPlayer;
        }
        
        public void AttachSource(VisionSource source)
        {
            if ( ! _sources.Add(source))
                return;
            source.transform.SetParent(transform);
            source.gameObject.layer = gameObject.layer;
        }

        public void DetachSource(VisionSource source)
        {
            if ( ! _sources.Remove(source))
                return;
            if (source)
                source.transform.SetParent(null);
        }
        
        public bool IsUnitVisible(Unit unit)
            => unit.Ownership.OwnedByPlayer == IsOwnedByPlayer 
                || (_visibleUnits.Contains(unit) && unit.Visibility.IsRevealed);

        private void OnTriggerEnter2D(Collider2D other)
        {
            Unit unit = other.GetComponentInParent<Unit>();
            if (unit ==  null)
                return;
            _visibleUnits.Add(unit);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Unit unit = other.GetComponentInParent<Unit>();
            if (unit ==  null || ! _visibleUnits.Contains(unit))
                return;
            _visibleUnits.Remove(unit);
        }
    }
}