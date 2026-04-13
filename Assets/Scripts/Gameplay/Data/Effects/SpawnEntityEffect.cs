using Gameplay.Entities;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Data.Effects
{
    [CreateAssetMenu(menuName = "Gameplay Data/Effects/Spawn Entity", order = 0)]
    public class SpawnEntityEffect : EffectTargetingPoint
    {
        [SerializeField] private SOInjectPresenter _gameplayPresenter;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _lifetime;
        
        [Inject] private PoolFactory<Entity> EntityFactory { get; set; }
        
        public override void Apply(Unit caster, Vector2 target)
        {
            _gameplayPresenter.Inject(this);

            Entity entity = EntityFactory.Get(_prefab);
            entity.Lifetime =  _lifetime;
            entity.transform.position = target;
        }
    }
}