using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Statuses
{
    [CreateAssetMenu(menuName = "Gameplay Data/Statuses/Drain Cloak", order = 0)]
    public class DrainCloakStatus : StatusType
    {
        [SerializeField] private int _energyDrain;
        [SerializeField] private int _energyDrainPeriod;
        
        public override void OnAdd(Status status)
        {
            status.Host.Visibility.Cloak(status);
        }

        public override void OnUpdate(Status status)
        {
            if (status.FramesSinceAddition % _energyDrainPeriod != 0)
                return;
            if (status.Host.Abilities.TrySpendEnergy(_energyDrain))
                return;
            status.Remove();
        }

        public override void OnRemove(Status status)
        {
            status.Host.Visibility.Decloak(status);
        }
    }
}