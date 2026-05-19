using UnityEngine;

namespace Gameplay.Schemes.Values
{
    public abstract class SchemeValue<T> : MonoBehaviour
    {
        public abstract T Value { get; }
    }
}