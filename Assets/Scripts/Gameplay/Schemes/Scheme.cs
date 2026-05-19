using System;
using System.Linq;
using Gameplay.Schemes.Actions;
using Gameplay.Schemes.Triggers;
using Gameplay.Schemes.Values.Variables;
using UnityEngine;

namespace Gameplay.Schemes
{
    public class Scheme : MonoBehaviour
    {
        [SerializeField] private SchemeTrigger _trigger;
        [SerializeField] private SchemeAction[] _actions;

        private void Awake()
        {
            _trigger.Triggered += Launch;
        }

        private void Launch()
        {
            foreach (SchemeAction action in _actions)
            {
                action.Act();
            }
        }

        private void OnValidate()
        {
            _trigger = GetComponentInChildren<SchemeTrigger>();
            _actions = GetComponentsInChildren<SchemeAction>()
                .Where(a => a.transform.parent == transform)
                .ToArray();
        }
    }
}