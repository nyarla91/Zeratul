using System.Linq;
using Cysharp.Threading.Tasks;
using Extentions;
using Gameplay.Data.Effects;
using Gameplay.Units;
using Gameplay.Visual;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class Projectile : PoolElement<Projectile>
    {
        private const float MaxUnitSpeedToFollow = 1;
        
        [SerializeField] private float _speed;
        [SerializeField] private float _contactDistance;
        [SerializeField] private bool _useAoe;
        [SerializeField] private AoeVariant _aoeVariant;
        
        [Inject] private TacticalPause TacticalPause { get; set; }
        [Inject] private PoolFactory<AoeView> AoeFactory { get; set; }
        
        public async void Launch(Unit caster, Vector2 target, EffectTargetingPoint[] effects)
        {
            MoveToCaster(caster);
            
            await Travel(new OrderTarget(target, null));
            foreach (EffectTargetingPoint effect in effects)
            {
                effect.Apply(caster, target);
            }
            Despawn();
        }

        public async void Launch(Unit caster, Unit target, EffectTargetingUnit[] effects)
        {
            MoveToCaster(caster);
            
            TravelResult travelResult = await Travel(new OrderTarget(target.Position, target));
            
            if (travelResult.UnitContact)
            {
                foreach (EffectTargetingUnit effect in effects)
                    effect.Apply(caster, target);   
                Despawn();
                return;
            }
            EffectTargetingPoint[] pointEffects = effects.Select(e => e as EffectTargetingPoint).ClearNull().ToArray();
            foreach (EffectTargetingPoint pointEffect in pointEffects)
            {
                pointEffect.Apply(caster, travelResult.FinalPosition);
            }
            Despawn();
        }

        public override void OnSpawn() { }

        protected override void OnDespawn() { }

        private void MoveToCaster(Unit caster)
        {
            transform.position = caster.Position;
        }

        private async UniTask<TravelResult> Travel(OrderTarget target)
        {
            bool unit = target.Unit;
            AoeView aoe = _useAoe ? AoeFactory.Get() : null;
            if (aoe)
            {
                aoe.Set(_aoeVariant);
            }
            
            Vector2 destination = unit ? target.Unit.Position : target.Point;

            while (Isometry.Distance(transform.position, destination) > _contactDistance)
            {
                await UniTask.WaitForFixedUpdate();
                if (TacticalPause.IsPaused)
                    await UniTask.WaitUntil(() => TacticalPause.IsUnpaused);

                if (unit && ! IsUnitFollowable(target.Unit, destination))
                    unit = false;
                
                if (unit)
                    destination = target.Unit.Position;
                transform.position += (Vector3) transform.DirectionTo2D(destination) * (Time.fixedDeltaTime * _speed);
                
                aoe?.Move(destination);
            }
            aoe?.Despawn();
            return new TravelResult(unit, destination);
        }

        private bool IsUnitFollowable(Unit unit, Vector2 previousPosition)
        {
            return unit != null && unit.IsAlive && Isometry.Distance(unit.Position, previousPosition) <= MaxUnitSpeedToFollow;
        }

        private struct TravelResult
        {
            public bool UnitContact { get; }
            public Vector2 FinalPosition { get; }

            public TravelResult(bool unitContact, Vector2 finalPosition)
            {
                UnitContact = unitContact;
                FinalPosition = finalPosition;
            }
        }
    }
}