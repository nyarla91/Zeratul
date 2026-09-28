using System;
using Cysharp.Threading.Tasks;
using Gameplay.UI;
using UnityEngine;
using Zenject;

namespace Gameplay.Schemes.Actions
{
    public class ActionToggleTacticalPause : SchemeAction
    {
        [Inject] private TacticalPauseToggle TacticalPauseToggle { get; set; }
        
        public override UniTask Act()
        {
            TacticalPauseToggle.Toggle();
            return UniTask.CompletedTask;
        }

        private void OnValidate()
        {
            gameObject.name = "> Toggle pause";
        }
    }
}