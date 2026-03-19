using Gameplay.Player;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Control Reserve", order = 0)]
    public class UnitControlReserveValidator : UnitValidator
    {
        [SerializeField] private SOInjectPresenter _gameplayInjectPresenter;
        
        [Inject] private PlayerControlResources PlayerControlResources { get; set; }
        
        public override bool IsValid(Unit actor, Unit target)
        {
            _gameplayInjectPresenter.Inject(this);
            return target.Type.ControlWorth <= PlayerControlResources.Reserve;
        }
    }
}