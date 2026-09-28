using Cysharp.Threading.Tasks;
using Gameplay.Player;
using UnityEngine;
using Zenject;

namespace Gameplay.Schemes.Actions
{
    public class ActionReduceKillCounter : SchemeAction
    {
        [Inject] private PlayerControlResources PlayerControlResources { get; set; }
            
        public override UniTask Act()
        {
            PlayerControlResources.ReduceKillCounter();
            return UniTask.CompletedTask;
        }

        private void OnValidate()
        {
            gameObject.name = $"> Reduce Kill Counter";
        }
    }
}