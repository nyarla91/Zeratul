using Extentions;
using Extentions.Pause;
using UniRx;
using UnityEngine;

namespace Gameplay.Units
{
    public class UnitDirection : UnitComponent
    {
        public float LookAngle { get; private set; }
        public float TargetLookAngle { get; private set; }
        
        public UnitDirection(Unit unit, IPauseReadonly tacticalPause) : base(unit)
        {
            Observable.EveryFixedUpdate()
                .Where(_ => tacticalPause.IsUnpaused)
                .Subscribe(_ => UpdateLookAngle());
        }
        
        public void RotateTowards(Vector2 direction) => RotateTowards(direction.ToDegrees());
        
        public void RotateTowards(float angle) => TargetLookAngle = angle;

        private void UpdateLookAngle()
        {
            if (Unit.Stagger.IsStaggered)
                return;
            float maxDelta = UnitType.RotationSpeed * Time.fixedDeltaTime;
            LookAngle = Mathf.MoveTowardsAngle(LookAngle, TargetLookAngle, maxDelta);
        }
    }
}