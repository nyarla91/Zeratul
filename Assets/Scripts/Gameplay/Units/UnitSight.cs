using System;
using System.Collections.Generic;
using Extentions;
using Gameplay.Data.Configs;
using Gameplay.Data.Validator;
using Gameplay.Vision;
using UniRx;
using UniRx.Triggers;

namespace Gameplay.Units
{
    public class UnitSight : UnitComponent
    {
        private readonly VisionConfig _config;
        private readonly VisionMap _visionMap;
        private readonly VisionSource _visionSource;

        public Modifier RadiusModifier { get; } = new Modifier();
        public float Radius => UnitType.SightRadius * RadiusModifier.Value;

        public UnitSight(Unit unit, VisionConfig config, VisionSource visionSource, VisionMap visionMap, Owner owner) : base(unit)
        {
            _config = config;
            _visionSource = visionSource;
            _visionMap = visionMap;
            
            _visionSource.Set(Unit.transform, UnitType.IsAir, Radius, unit.Alliance.CurrentOwner);
            
            Unit.Alliance.ObserveEveryValueChanged(o => o.CurrentOwner)
                .Subscribe(o => _visionSource.Owner = o);
            
            Unit.FixedUpdateAsObservable()
                .Subscribe(_ => _visionSource.IsSimulated = Unit.Simulation.IsSimulated);
            
            this.ObserveEveryValueChanged(v => v.Radius)
                .Subscribe(r => _visionSource.Radius = r);

            Unit.Killed += _visionSource.Dispose;
        }

        public HashSet<Unit> VisibleUnits(UnitValidatorGroup validatorGroup = default) =>
            _visionSource.VisibleUnits(Unit, validatorGroup);
    }
}