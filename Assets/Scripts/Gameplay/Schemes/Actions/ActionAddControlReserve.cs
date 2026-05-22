using System;
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
        
        public override void Act()
        {
            PlayerControlResources.AddReserve(_quantity.Value);
        }

        private void OnValidate()
        {
            gameObject.name = $"> Add {_quantity?.name} control reserve";    
        }
    }
}