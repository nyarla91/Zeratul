using System;
using UnityEngine;
using Zenject;

namespace Gameplay.Schemes.Actions
{
    public class ActionCompleteScenario : SchemeAction
    {
        [Inject] private IScenarioCompleteService ScenarioCompleteService { get; set; }
        
        public override void Act()
        {
            ScenarioCompleteService.Complete();
        }

        private void OnValidate()
        {
            gameObject.name = "> Complete scenario";
        }
    }
}