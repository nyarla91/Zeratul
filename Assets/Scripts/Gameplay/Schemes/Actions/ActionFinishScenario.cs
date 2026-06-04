using System;
using Gameplay.UI.Menu;
using UnityEngine;
using Zenject;

namespace Gameplay.Schemes.Actions
{
    public class ActionFinishScenario : SchemeAction
    {
        [SerializeField] private Result _result = Result.Victory;
        
        [Inject] private DefeatMenu DefeatMenu { get; set; }
        [Inject] private VictoryMenu VictoryMenu { get; set; }
        
        public override void Act()
        {
            switch (_result)
            {
                case Result.Defeat:
                    DefeatMenu.Open();
                    break;
                case Result.Victory:
                    VictoryMenu.Open();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnValidate()
        {
            string result = _result switch {
                Result.Defeat => "Defeat",
                Result.Victory => "Victory",
                _ => throw new ArgumentOutOfRangeException()
            };
            gameObject.name = $"> End scenario in {result}";
        }

        private enum Result
        {
            Defeat,
            Victory
        }
    }
}