using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Gameplay.Player;
using Gameplay.Schemes.Values;
using UnityEngine;
using Zenject;

namespace Gameplay.Schemes.Actions
{
    public class ActionAddControlReserve : SchemeAction
    {
        [SerializeField] private SchemeValue<int> _quantity;
        
        [Inject] private PlayerControlResources PlayerControlResources { get; set; }
        
        public override UniTask Act()
        {
            PlayerControlResources.AddReserve(_quantity.Value);
            return UniTask.CompletedTask;
        }

        private void OnValidate()
        {
            gameObject.name = $"> Add {_quantity?.name} control reserve";    
        }
    }
}