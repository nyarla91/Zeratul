using System;
using UnityEngine;
using Zenject;

namespace Gameplay.Schemes.Actions
{
    public class ActionToggleTacticalPause : SchemeAction
    {
        [Inject] private TacticalPauseControl TacticalPauseControl { get; set; }
        
        public override void Act()
        {
            TacticalPauseControl.TogglePause();
        }

        private void OnValidate()
        {
            gameObject.name = "> Toggle pause";
        }
    }
}