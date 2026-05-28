using Extentions;
using Extentions.Pause;
using Save.Data.Units;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Gameplay.Units
{
    public class UnitDirection : UnitComponent
    {
        protected override string LoadKey => UnitDirectionSaveSystem.LoadKey;

        public float LookAngle { get; private set; }
        public float TargetLookAngle { get; private set; }
        
        public UnitDirection(Unit unit, IPauseReadonly tacticalPause, float lookAngle) : base(unit)
        {
            LookAngle = lookAngle;
            Unit.FixedUpdateAsObservable()
                .Where(_ => tacticalPause.IsUnpaused)
                .Subscribe(_ => UpdateLookAngle());
        }

        public override IUnitSaveSystem Save()
        {
            return new UnitDirectionSaveSystem(LookAngle);
        }

        public override void ReproduceFromSave(UnitSaveData saveData)
        {
            UnitDirectionSaveSystem system = GetSaveSystem<UnitDirectionSaveSystem>(saveData);
            TargetLookAngle = LookAngle = system.lookAngle;
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