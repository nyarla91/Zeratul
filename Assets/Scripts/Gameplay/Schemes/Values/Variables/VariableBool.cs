using UnityEngine;

namespace Gameplay.Schemes.Values.Variables
{
    public class VariableBool : SchemeVariable<bool>
    {
        [SerializeField] private bool _defaultValue;

        protected override bool DefaultValue => _defaultValue;
    }
}