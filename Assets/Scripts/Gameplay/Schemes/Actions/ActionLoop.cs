using System;
using System.Linq;
using Gameplay.Schemes.Values;
using Gameplay.Schemes.Values.Variables;
using UnityEngine;

namespace Gameplay.Schemes.Actions
{
    public class ActionLoop : SchemeAction
    {
        [SerializeField] private SchemeValue<int> _from;
        [SerializeField] private SchemeValue<int> _step;
        [SerializeField] private SchemeValue<int> _to;
        [SerializeField] private SchemeVariable<int> _out;
        [SerializeField] private SchemeAction[] _actions;
        
        public override void Act()
        {
            for (int i = _from?.Value ?? 0; i <= _to.Value; i += _step?.Value ?? 1)
            {
                _out?.Set(i);
                foreach (SchemeAction action in _actions)
                {
                    action.Act();
                }
            }
        }

        private void OnValidate()
        {
            _actions = GetComponentsInChildren<SchemeAction>()
                .Where(a => a.transform.parent == transform)
                .ToArray();

            gameObject.name = $"> Loop {_out?.name} from {_from?.name ?? 0.ToString()} to {_to?.name} with step {_step?.name ?? 1.ToString()}";
        }
    }
}