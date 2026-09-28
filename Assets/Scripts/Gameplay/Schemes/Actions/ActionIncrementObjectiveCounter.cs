using Cysharp.Threading.Tasks;
using Gameplay.Schemes.Values;
using Gameplay.Schemes.Values.Variables;
using UnityEngine;

namespace Gameplay.Schemes.Actions
{
    public class ActionIncrementObjectiveCounter : SchemeAction
    {
        [SerializeField] private VariableObjective _objective;
        [SerializeField] private SchemeValue<int> _increment;
        
        public override UniTask Act()
        {
            _objective.Value.Increment(_increment?.Value ?? 1);
            return UniTask.CompletedTask;
        }
        
        private void OnValidate()
        {
            gameObject.name = $"> Increment {_objective?.name} counter by {_increment?.name ?? 1.ToString()}";
        }
    }
}