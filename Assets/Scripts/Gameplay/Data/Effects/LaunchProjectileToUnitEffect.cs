using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Data.Effects
{
    [CreateAssetMenu(menuName = "Gameplay Data/Effects/Launch Projectile to Unit", order = 0)]
    public class LaunchProjectileToUnitEffect : EffectTargetingUnit
    {
        [SerializeField] private SOInjectPresenter _gameplayPresenter;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private EffectTargetingUnit[] _effectsOnHit;
        
        [Inject] private PoolFactory<Projectile> ProjectileFactory { get; set; }
        
        public override void Apply(Unit caster, Unit target)
        {
            _gameplayPresenter.Inject(this);

            Projectile projectile = ProjectileFactory.Get(_prefab);
            projectile.Launch(caster, target, _effectsOnHit);
        }
    }
}