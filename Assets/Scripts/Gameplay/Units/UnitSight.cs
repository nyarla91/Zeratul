using _Core;
using Gameplay.Vision;

namespace Gameplay.Units
{
    public class UnitSight : UnitComponent
    {
        public Modifier RadiusModifier { get; } = new Modifier();
        public float Radius => UnitType.SightRadius * RadiusModifier.Value;
        
        public VisionSource VisionSource { get; private set; }

        public UnitSight(Unit unit, VisionMap visionMap) : base(unit)
        {
            VisionSource = visionMap.CreateSource(
                () => Unit.IsAlive ? Unit.Position : default,
                () => Unit.IsAlive ?  unit.Alliance.CurrentOwner : Owner.Neutral,
                () => Unit.IsAlive ? Radius : 0,
                () => UnitType.IsAir
                );
            
            Unit.Killed += VisionSource.Dispose;
        }
    }
}