using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Gameplay.Schemes.Actions;
using Gameplay.Schemes.Triggers;
using Gameplay.Schemes.Values;
using Gameplay.Schemes.Values.Variables;
using UnityEngine;

namespace Gameplay.Schemes
{
    public class Scheme : MonoBehaviour
    {
        [SerializeField] private SchemeTrigger _trigger;
        [SerializeField] private SchemeValue<bool>[] _conditions;
        [SerializeField] private SchemeAction[] _actions;

        public bool IsInProgress { get; private set; }
        
        private void Awake()
        {
            _trigger.Triggered += () => _ = Launch();
        }

        private async UniTask Launch()
        {
            if ( ! _conditions.All(c => c.Value))
                return;
            
            IsInProgress = true;   
            foreach (SchemeAction action in _actions)
            {
                await action.Act();
            }
            IsInProgress = false;   
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