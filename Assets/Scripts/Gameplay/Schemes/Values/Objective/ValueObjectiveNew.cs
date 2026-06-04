using System;
using UnityEngine;

namespace Gameplay.Schemes.Values.Objective
{
    public class ValueObjectiveNew : SchemeValue<_Core.Objective>
    {
        [SerializeField] private string _label;
        [SerializeField] private SchemeValue<int> _counter;
        [SerializeField] private SchemeValue<int> _goal;

        public override _Core.Objective Value => new(_label, _counter?.Value ?? 0, _goal?.Value ?? 0);

        private void OnValidate()
        {
            string counters = _goal ? $"({_counter?.name ?? 0.ToString()}/{_goal.name})" : "";
            gameObject.name = $"(New objective ({_label}) {counters})";
        }
    }
}