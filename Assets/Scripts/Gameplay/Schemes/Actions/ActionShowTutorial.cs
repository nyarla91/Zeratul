using System;
using Gameplay.Data.Configs;
using Gameplay.UI;
using Gameplay.UI.Menu;
using UnityEngine;
using Zenject;

namespace Gameplay.Schemes.Actions
{
    public class ActionShowTutorial : SchemeAction
    {
        [SerializeField] private TutorialEntry _entry;
        [SerializeField] private bool _tip;
        
        [Inject] private TipWindow TipWindow { get; set; }
        [Inject] private TutorialScreen TutorialScreen { get; set; }
        
        public override void Act()
        {
            if (_tip)
                TipWindow.Show(_entry);
            else
                TutorialScreen.Open(_entry);
        }

        private void OnValidate()
        {
            gameObject.name = $"> Show tutorial ({_entry.Header})";
        }
    }
}