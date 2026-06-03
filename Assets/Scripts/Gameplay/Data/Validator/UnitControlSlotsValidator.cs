using Gameplay.Player;
using Gameplay.Units;
using Settings.Localization;
using UnityEngine;
using Zenject;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Control Slots", order = 0)]
    public class UnitControlSlotsValidator : UnitValidator
    {
        [SerializeField] private SOInjectPresenter _gameplayInjectPresenter;
        [SerializeField] private Localizer _localizer;
        
        [Inject] private PlayerControlResources PlayerControlResources { get; set; }
        
        public override bool IsValid(Unit actor, Unit target)
        {
            _gameplayInjectPresenter.Inject(this);
            return PlayerControlResources.CanFitUnit(target);
        }

        public override string GetInvalidMessage(Unit actor, Unit target)
        {
            string result = _localizer.Translate(RawInvalidMessage);
            return result.Replace("#", (target.Type.ControlSlots - PlayerControlResources.AvailableSlots).ToString());
        }
    }
}