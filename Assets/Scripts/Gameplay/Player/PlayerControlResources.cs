using Gameplay.Data.Configs;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerControlResources
    {
        public int Reserve { get; private set; }
        
        public PlayerControlResources(PlayerControlConfig config)
        {
            Reserve = Mathf.Max(config.StartingReserve, 0);
        }

        public void AddReserve(int quantity)
        {
            if (quantity <= 0)
                return;
            Reserve += quantity;
        }

        public bool TrySpendReserve(int quantity)
        {
            if (quantity <= 0 || quantity > Reserve)
                return false;
            Reserve -= quantity;
            return true;
        }
    }
}