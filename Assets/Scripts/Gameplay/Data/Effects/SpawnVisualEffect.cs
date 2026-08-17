using Gameplay.Entities;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Data.Effects
{
    [CreateAssetMenu(menuName = "Gameplay Data/Effects/Spawn Visual Effect", order = 0)]
    public class SpawnVisualEffect : EffectTargetingPoint
    {
        [SerializeField] private SOInjectPresenter _gameplayPresenter;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _lifetime;
        [SerializeField] private bool _followUnit;
        
        [Inject] private PoolFactory<Entity> EntityFactory { get; set; }

        public override void Apply(Unit caster, Unit target)
        {
            VisualEffect vfx = Spawn(caster, target.Position);
            if (_followUnit)
                vfx.AttachToUnit(target);
        }

        public override void Apply(Unit caster, Vector2 target)
        {
            Spawn(caster, target);
        }

        private VisualEffect Spawn(Unit caster, Vector2 target)
        {
            _gameplayPresenter.Inject(this);

            VisualEffect vfx = EntityFactory.Get(_prefab) as VisualEffect;
            if ( ! vfx)
                return null;
            vfx.InitEntity(target, caster, _lifetime);
            return vfx;
        }
    }
}