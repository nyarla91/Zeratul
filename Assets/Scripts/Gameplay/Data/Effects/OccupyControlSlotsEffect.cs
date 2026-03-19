using Gameplay.Player;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Data.Effects
{
    [CreateAssetMenu(menuName = "Gameplay Data/Effects/Occupy Control Slot", order = 0)]
    public class OccupyControlSlotsEffect : EffectTargetingUnit
    {
        [SerializeField] private SOInjectPresenter _gameplayInjectPresenter;
        
        [Inject] private PlayerControlResources PlayerControlResources { get; set; }

        public override void Apply(Unit caster, Unit target)
        {
            _gameplayInjectPresenter.Inject(this);
            PlayerControlResources.TryAddUnit(target);
        }
    }
}