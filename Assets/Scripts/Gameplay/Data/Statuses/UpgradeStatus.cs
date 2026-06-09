using Gameplay.Units;
using Gameplay.Upgrades;
using UnityEngine;
using Zenject;

namespace Gameplay.Data.Statuses
{
    [CreateAssetMenu(menuName = "Gameplay Data/Statuses/Upgrade", order = 0)]
    public class UpgradeStatus : StatusType
    {
        [SerializeField] private SOInjectPresenter _gameplayInjectPresenter;
        [SerializeField] private Upgrade _upgradeRequired;
        [SerializeField] private StatusType _addedStatus;
        
        [Inject] private UpgradeStorage UpgradeStorage { get; set; }
        
        public override void OnAdd(Status status)
        {
            _gameplayInjectPresenter.Inject(this);
        }

        public override void OnUpdate(Status status)
        {
            Unit host = status.Host;
            if (UpgradeStorage.IsUpgradeResearched(host.Alliance.CurrentOwner, _upgradeRequired))
                host.Statuses.AddStatus(_addedStatus, host);
            else
                host.Statuses.RemoveStatus(_addedStatus);
            
        }

        public override void OnRemove(Status status)
        {
            status.Host.Statuses.RemoveStatus(_addedStatus);
        }
    }
}