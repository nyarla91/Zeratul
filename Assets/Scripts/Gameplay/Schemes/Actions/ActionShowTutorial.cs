using System;
using Gameplay.UI;
using UnityEngine;
using Zenject;

namespace Gameplay.Schemes.Actions
{
    public class ActionShowTutorial : SchemeAction
    {
        [SerializeField] private int _tutorialIndex;
        
        [Inject] private TipWindow TipWindow { get; set; }
        
        public override void Act()
        {
            TipWindow.Show(_tutorialIndex);
        }

        private void OnValidate()
        {
            gameObject.name = $"> Show tutorial {_tutorialIndex}";
        }
    }
}