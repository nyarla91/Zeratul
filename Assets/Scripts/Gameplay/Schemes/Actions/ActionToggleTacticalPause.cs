using System;
using Gameplay.UI;
using UnityEngine;
using Zenject;

namespace Gameplay.Schemes.Actions
{
    public class ActionToggleTacticalPause : SchemeAction
    {
        [Inject] private TacticalPauseToggle TacticalPauseToggle { get; set; }
        
        public override void Act()
        {
            TacticalPauseToggle.Toggle();
        }

        private void OnValidate()
        {
            gameObject.name = "> Toggle pause";
        }
    }
}