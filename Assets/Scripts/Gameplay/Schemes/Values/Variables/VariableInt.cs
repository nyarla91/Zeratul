using UnityEngine;

namespace Gameplay.Schemes.Values.Variables
{
    public class VariableInt : SchemeVariable<int>
    {
        [SerializeField] private int _defaultValue;
        
        protected override int DefaultValue => _defaultValue;
        protected override string DisplayDefaultValue => DefaultValue.ToString();
    }
}