using System;
using Cysharp.Threading.Tasks;
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
        
        [Inject] private TipWindow TipWindow { get; set; }
        
        public override UniTask Act()
        {
            TipWindow.Show(_entry);
            return UniTask.CompletedTask;
        }

        private void OnValidate()
        {
            gameObject.name = $"> Show tutorial ({_entry.Header})";
        }
    }
}