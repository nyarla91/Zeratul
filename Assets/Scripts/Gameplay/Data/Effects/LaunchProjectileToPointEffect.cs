using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Data.Effects
{
    [CreateAssetMenu(menuName = "Gameplay Data/Effects/Launch Projectile to Point", order = 0)]
    public class LaunchProjectileToPointEffect : EffectTargetingPoint
    {
        [SerializeField] private SOInjectPresenter _gameplayPresenter;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private EffectTargetingPoint[] _effectsOnHit;
        
        [Inject] private PoolFactory<Projectile> ProjectileFactory { get; set; }

        public override void Apply(Unit caster, Vector2 target)
        {
            _gameplayPresenter.Inject(this);

            Projectile projectile = ProjectileFactory.Get(_prefab);
            projectile.Launch(caster, target, _effectsOnHit);
        }
    }
}