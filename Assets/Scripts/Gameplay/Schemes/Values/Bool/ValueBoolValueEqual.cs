using UnityEngine;

namespace Gameplay.Schemes.Values.Bool
{
    public class ValueBoolValueEqual<T> : SchemeValue<bool>
    {
        [SerializeField] private SchemeValue<T> _a;
        [SerializeField] private SchemeValue<T> _b;
        
        public override bool Value => _a.Value?.Equals(_b.Value) ?? false;

        private void OnValidate()
        {
            gameObject.name = $"{_a?.name} equals{_b?.name}";
        }
    }
}