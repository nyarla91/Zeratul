using System;
using System.Linq;
using Gameplay.Schemes.Values;
using UnityEngine;

namespace Gameplay.Schemes.Actions
{
    public class ActionIfElse : SchemeAction
    {
        [SerializeField] private SchemeValue<bool> _condition;
        [SerializeField] private SchemeAction[] _actions;
        [SerializeField] private Transform _elseBlock;
        [SerializeField] private SchemeAction[] _elseActions;
        
        public override void Act()
        {
            if (_condition.Value)
                foreach (SchemeAction action in _actions)
                    action.Act();
            else
                foreach (SchemeAction action in _elseActions)
                    action.Act();
        }

        private void OnValidate()
        {
            _actions = GetComponentsInChildren<SchemeAction>()
                .Where(a => a.transform.parent == transform)
                .ToArray();

            if (_elseBlock != null)
                _elseBlock.name = "> Else";
            _elseActions = _elseBlock ? _elseBlock.GetComponentsInChildren<SchemeAction>()
                .Where(a => a.transform.parent == _elseBlock)
                .ToArray() : Array.Empty<SchemeAction>();

            gameObject.name = $"> If {_condition?.name}";
        }
    }
}