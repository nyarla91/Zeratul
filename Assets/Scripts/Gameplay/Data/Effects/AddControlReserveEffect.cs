using Gameplay.Player;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Data.Effects
{
    [CreateAssetMenu(menuName = "Gameplay Data/Effects/Add Control Reserve", order = 0)]
    public class AddControlReserveEffect : EffectTargetingPoint
    {
        [SerializeField] private SOInjectPresenter _gameplayPresenter;
        [SerializeField] private int _amount;
        
        [Inject] private PlayerControlResources PlayerControlResources { get; set; }
        
        public override void Apply(Unit caster, Vector2 target)
        {
            _gameplayPresenter.Inject(this);
            PlayerControlResources.AddReserve(_amount);
        }
    }
}