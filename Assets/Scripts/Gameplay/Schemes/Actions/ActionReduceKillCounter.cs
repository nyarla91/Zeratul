using Gameplay.Player;
using UnityEngine;
using Zenject;

namespace Gameplay.Schemes.Actions
{
    public class ActionReduceKillCounter : SchemeAction
    {
        [Inject] private PlayerControlResources PlayerControlResources { get; set; }
            
        public override void Act()
        {
            PlayerControlResources.ReduceKillCounter();
        }

        private void OnValidate()
        {
            gameObject.name = $"> Reduce Kill Counter";
        }
    }
}