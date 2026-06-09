using Gameplay.Units;
using Gameplay.Upgrades;
using UnityEngine;
using Zenject;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Upgrade", order = 0)]
    public class UpgradeValidator : UnitValidator
    {
        [SerializeField] private SOInjectPresenter _gameplayInjectPresenter;
        [SerializeField] private Upgrade _upgradeRequired;
        
        [Inject] private UpgradeStorage UpgradeStorage { get; set; }
        
        public override bool IsValid(Unit actor, Unit target)
        {
            _gameplayInjectPresenter.Inject(this);
            return UpgradeStorage.IsUpgradeResearched(target.Alliance.CurrentOwner, _upgradeRequired);
        }
    }
}