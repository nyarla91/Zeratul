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
        public Modifier RadiusModifier { get; } = new Modifier();
        public float Radius => UnitType.SightRadius * RadiusModifier.Value;
        
        public VisionSource VisionSource { get; private set; }

        public UnitSight(Unit unit, VisionMap visionMap) : base(unit)
        {
            VisionSource = visionMap.CreateSource(() => Unit.Position, () => unit.Alliance.CurrentOwner, () => Radius, () => UnitType.IsAir);
            
            Unit.Killed += () => visionMap.RemoveSource(VisionSource);
        }
    }
}